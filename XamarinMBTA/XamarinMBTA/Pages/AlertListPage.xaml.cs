using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Globals;
using XamarinMBTA.ViewModels;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlertListPage : ContentPage
	{
        AlertListPageViewModel viewModel;
		public AlertListPage ()
		{
			InitializeComponent ();
            viewModel = new AlertListPageViewModel();
            AlertListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null)
                    return;
                Task.Delay(500);
                if (sender is ListView lv) lv.SelectedItem = null;
                int index = (e.Item as AlertListPageViewModel.AlertDisplayModel).index;
                loadAlertDetail(index);
            };

            MessagingCenter.Subscribe<App, string>
                ((App)Xamarin.Forms.Application.Current, "LineSelected", (sender, arg) =>
            {
                //argument: long name of route
                viewModel.selectedLine = arg;
                updateList();
            });

            updateList();
		}

 
        private void updateList()
        {
            viewModel.loadAlerts();
            AlertListView.ItemsSource = viewModel.displayAlertList;
            System.Diagnostics.Debug.WriteLine("Length = " + viewModel.displayAlertList.Count);
            update();
        }

        async void loadAlertDetail(int index)
        {
            await this.Navigation.PushAsync(new AlertDetailPage(index));
        }

        async void OnSelectLineTapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new SelectLinePage());
        }

        private void update()
        {
            this.BindingContext = viewModel;
        }
	}
}