using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Globals;
using XamarinMBTA.Performance;
using XamarinMBTA.ViewModels;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AnalysisPage : ContentPage
	{
        
		public AnalysisPage ()
		{
			InitializeComponent ();
            BindingContext = new AnalysisViewModel();
		}

	}
}