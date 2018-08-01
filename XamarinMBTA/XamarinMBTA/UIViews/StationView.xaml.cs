using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinMBTA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StationView : ContentView
	{
        
		public StationView ()
		{
			InitializeComponent ();
		}

        public void setText(string s)
        {
            stationlabel.Text = s;
        }
	}
}