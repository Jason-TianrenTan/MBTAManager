using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Directions;
using XamarinMBTA.Events;
using XamarinMBTA.Globals;
using XamarinMBTA.Performance;
using XamarinMBTA.ViewModels;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventDetailPage : ContentPage
	{
        private static readonly CompositeDisposable EventSubscriptions = new CompositeDisposable();
        private readonly PanGestureRecognizer _panGesture = new PanGestureRecognizer();
        private double _transY;
        EventDetailViewModel viewModel;
		public EventDetailPage (PlannedEvent pEvent, GoogleRoute route)
		{
			InitializeComponent ();
            viewModel = new EventDetailViewModel(pEvent, route);
            BindingContext = viewModel;
            StepList.ItemsSource = viewModel.stepModels;
            AccuracyList.ItemsSource = viewModel.routeStatList;

            AccuracyList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null)
                    return;
                Task.Delay(500);
                if (sender is ListView lv) lv.SelectedItem = null;
                Database.currentAccModel = (e.Item as EventDetailViewModel.StatisticModel).AccuracyPerformances;
                loadStatisticsDetail();
            };

            StepList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null)
                    return;
                Task.Delay(500);
                if (sender is ListView lv) lv.SelectedItem = null;
            };
            initLocation(pEvent);
            drawPolylines(route);
		}

        private async void loadStatisticsDetail()
        {
            //page.setAccList(acc);
            await Navigation.PushModalAsync(new AnalysisPage());
        }

        private void initLocation(PlannedEvent pe)
        {
            Position startPos = pe.startPos, endPos = pe.endPos;
            EventDetailMap.Pins.Add(new Pin
            {
                Label = "Start",
                Position = startPos
            });
            EventDetailMap.Pins.Add(new Pin
            {
                Label = "Target",
                Position = endPos
            });
            EventDetailMap.MoveToRegion(new MapSpan(startPos, 0.01, 0.01));
        }

        private void drawPolylines(GoogleRoute route)
        {
            Position start = viewModel.currentEvent.startPos, end = viewModel.currentEvent.endPos;
            var polyLine = new Xamarin.Forms.GoogleMaps.Polyline();
            polyLine.StrokeColor = Color.Orange;
            polyLine.StrokeWidth = 10f;
            polyLine.Positions.Add(start);
            foreach (Step step in route.legs[0].steps)
            {
                string polyStr = step.polyline.points;
                List<Xamarin.Essentials.Location> polyPoints = Configs.DecodePolylinePoints(polyStr);
                foreach (var loc in polyPoints)
                    polyLine.Positions.Add(new Position(loc.Latitude, loc.Longitude));
            }
            polyLine.Positions.Add(end);
            Thread.Sleep(500);
            EventDetailMap.Polylines.Add(polyLine);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeObservables();
            CollapseAllMenus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            EventSubscriptions.Clear();
        }

        private void CollapseAllMenus()
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(200);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Notification.HeightRequest = this.Height - QuickMenuLayout.Height;
                    QuickMenuPullLayout.TranslationY = Notification.HeightRequest;
                });
            });
        }

        private void InitializeObservables()
        {
            var panGestureObservable = Observable
                .FromEventPattern<PanUpdatedEventArgs>(
                    x => _panGesture.PanUpdated += x,
                    x => _panGesture.PanUpdated -= x
                )
                .Subscribe(x => Device.BeginInvokeOnMainThread(() => { CheckQuickMenuPullOutGesture(x); }));

            EventSubscriptions.Add(panGestureObservable);
            QuickMenuInnerLayout.GestureRecognizers.Add(_panGesture);
        }

        private void CheckQuickMenuPullOutGesture(EventPattern<PanUpdatedEventArgs> x)
        {
            var e = x.EventArgs;
            var typeOfAction = x.Sender as StackLayout;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    MethodLockedSync(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            QuickMenuPullLayout.TranslationY = Math.Max(0,
                                Math.Min(Notification.HeightRequest, QuickMenuPullLayout.TranslationY + e.TotalY));
                        });
                    }, 2);

                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    _transY = QuickMenuPullLayout.TranslationY;
                    break;
                case GestureStatus.Canceled:
                    break;
            }
        }

        private CancellationTokenSource _throttleCts = new CancellationTokenSource();
        private void MethodLockedSync(Action method, double timeDelay = 500)
        {
            Interlocked.Exchange(ref _throttleCts, new CancellationTokenSource()).Cancel();
            Task.Delay(TimeSpan.FromMilliseconds(timeDelay), _throttleCts.Token) // throttle time
                .ContinueWith(
                    delegate { method(); },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
