using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinMBTA.Alerts;
using XamarinMBTA.Predictions;
using XamarinMBTA.Routes;
using XamarinMBTA.Schedules;
using XamarinMBTA.Stops;
using XamarinMBTA.Vehicle;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Globals;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace XamarinMBTA
{
    public partial class App : Application
	{
        private static void log(String str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

		public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

        protected override void OnStart()
        {
            // Handle when your app starts
            //initData();
        }

        private async void initData()
        {
            Database.routeList = await DataQuery.getRoutes();

            //getVehicleList();

            //VehicleInfo vehicleInfo = await getVehicleInfo("y1869");
            //log(vehicleInfo.ToString());

            //List<Station> stationList = await DataQuery.getStations();
            //List<Routine> routeList = await DataQuery.getRoutes();
            //List<Schedule> scheduleList = await DataQuery.getSchedule("Green-B");
            //List<Prediction> predictionList = await DataQuery.getPredictions("Green-B");

            //List<Alert> alertList = await DataQuery.getAlerts();
            //AlertData alertData = await DataQuery.getAlertData("202576");
        }



        //Returning info of a specific vehicle
        private async static Task<VehicleInfo> getVehicleInfo(string id)
        {
            return await DataQuery.getVehicleInfo(id);
        }


        //Returning a list of filtered vehicle information
        private async static Task<List<VehicleInfo>> getVehicleList()
        {
            VehicleList list = await DataQuery.getVehicles("current_status", "trip,stop,route", null, 
                null, null, null);
            List<VehicleInfo> vehicles = new List<VehicleInfo>();
            System.Diagnostics.Debug.WriteLine("Results:");
            for (int i=0;i<list.data.Length;i++)
            {
                vehicles.Add(list.data[i]);
                log(list.data[i].ToString());
            }
            return vehicles;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
