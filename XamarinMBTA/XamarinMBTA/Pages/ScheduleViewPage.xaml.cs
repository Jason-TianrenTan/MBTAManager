using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScheduleViewPage : TabbedPage
	{
		public ScheduleViewPage ()
		{
            Title = "My Plans";
			InitializeComponent ();
            
        }
	}
}