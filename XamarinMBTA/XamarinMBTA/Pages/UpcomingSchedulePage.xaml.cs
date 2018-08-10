using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Essentials;
using XamarinMBTA.Globals;
using XamarinMBTA.ViewModels;
using XamarinMBTA.Events;

namespace XamarinMBTA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingSchedulePage : ContentPage
    {
        UpcomingScheduleViewModel viewModel;
       
        public UpcomingSchedulePage()
        {
            InitializeComponent();
            viewModel = new UpcomingScheduleViewModel();
            initLocation();
        }


        public async void OnEventTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventDetailPage(viewModel.currentEvent,
                viewModel.route));
        }


        private async void initLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Position current, target;
                    //for test
                    Configs.Lat = 42.3700530;
                    Configs.Lng = -71.1174180;
                    //Configs.Lat = location.Latitude;
                    //Configs.Lng = location.Longitude;
                    current = new Position(Configs.Lat, Configs.Lng);/*
                    ScheduleMap.MoveToRegion(new MapSpan(current, 0.01, 0.01));
                    ScheduleMap.Pins.Add(new Pin
                    {
                        Label = "Xamarin Test",
                        Position = current
                    });*/
                    double upcomingLat = 42.3549540;//Boston Common
                    double upcomingLng = -71.0654890;
                    target = new Position(upcomingLat, upcomingLng);
                    /*
                    ScheduleMap.Pins.Add(new Pin
                    {
                        Label = "Target",
                        Position = target
                    });*/
                    DateTime temp = new DateTime(2018, 8, 4, 9, 0, 0);
                    PlannedEvent pEvent = new PlannedEvent
                    {
                        EventName = "Planned Test",
                        startPos = current,
                        endPos = target,
                        EventTime = temp.ToString("yyyy-MM-dd hh:mm")
                    };
                    viewModel.loadEvent(pEvent);
                    BindingContext = viewModel;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                System.Diagnostics.Debug.WriteLine(fnsEx.Message);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                System.Diagnostics.Debug.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                // Unable to get location
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }


}