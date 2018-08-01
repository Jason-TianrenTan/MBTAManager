using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.ViewModels;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlertDetailPage : ContentPage
	{
        private int alertIndex;
        AlertDetailViewModel alert_viewModel;
		public AlertDetailPage (int index)
		{
            alertIndex = index;
            alert_viewModel = new AlertDetailViewModel(index);
            BindingContext = alert_viewModel;
            InitializeComponent ();
            Title = "Alert Details";
		}
	}
}