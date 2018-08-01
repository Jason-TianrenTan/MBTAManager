using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinMBTA.Pages;

namespace XamarinMBTA
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage myMBTAPage = new NavigationPage(new MBTAPage());
            NavigationPage.SetHasNavigationBar(myMBTAPage, false);
            myMBTAPage.Title = "My MBTA";
            this.Children.Add(myMBTAPage);

            NavigationPage schedulePage = new NavigationPage(new LineSchedulePage());
            NavigationPage.SetHasNavigationBar(schedulePage, false);
            schedulePage.Title = "Schedules";
            this.Children.Add(schedulePage);

		}
	}
}
