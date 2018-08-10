using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinMBTA.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Globals;
using XamarinMBTA.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms.GoogleMaps;
using XamarinMBTA.Events;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MBTAPage : ContentPage
	{
        MBTAViewModel viewModel;

		public MBTAPage ()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            FromStationView.setText("Harvard");
            DestStationView.setText("Park Street");
            viewModel = new MBTAViewModel();
            requestPermission();
        }

        private async void requestPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Need location", "Gunna need that location", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }
        }

        async void OnScheduleTapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new NavigationPage(new ScheduleViewPage()));
        }


        async void OnAlertsViewMoreTapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new NavigationPage(new AlertListPage()));
        }


        async void OnAlertContentTapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new NavigationPage(new AlertDetailPage(0)));
        }

    }
}