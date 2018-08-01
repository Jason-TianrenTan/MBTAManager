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