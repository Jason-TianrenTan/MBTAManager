using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinMBTA.Globals;
using XamarinMBTA.Schedules;
using XamarinMBTA.Trips;
using XamarinMBTA.ViewModels;

namespace XamarinMBTA.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LineSchedulePage : ContentPage
	{
        LineScheduleViewModel viewModel;
		public LineSchedulePage ()
		{
			InitializeComponent ();
            
            NavigationPage.SetHasNavigationBar(this, false);
            viewModel = new LineScheduleViewModel();
            

            MessagingCenter.Subscribe<App, string>
                ((App)Xamarin.Forms.Application.Current, "LineSelected", (sender, arg) =>
                {
                    //argument: long name of route
                    viewModel.updateStations(arg);
                    BindingContext = viewModel;
                    ScheduleDirectionLabel.Text = viewModel.RouteDirection.dir_str;
                });

            ScheduleDeparturePicker.ItemsSource = viewModel.StationList;
            ScheduleArrivalPicker.ItemsSource = viewModel.StationList;
            ScheduleListView.ItemsSource = viewModel.DisplayScheduleTimes;
            ScheduleDeparturePicker.SelectedIndexChanged += (sender, args) =>
            {
                updateSchedules();
            };

            ScheduleArrivalPicker.SelectedIndexChanged += (sender, args) =>
            {
                updateSchedules();
            };
        }

        private async void OnLineTagTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SelectLinePage());
        }

        private void updateSchedules()
        {
            viewModel.DisplayScheduleTimes.Clear();
            int arrival_index = ScheduleArrivalPicker.SelectedIndex;
            int departure_index = ScheduleDeparturePicker.SelectedIndex;
            if (departure_index >= 0)
            {
                string selectedDepatureStationName = viewModel.StationList[departure_index];
                string routeID = viewModel.SelectedRoute;
                if (arrival_index < 0)
                {
                    foreach (Schedule schedule in Database.RouteSchedules[routeID])
                    {
                        if (schedule.attributes.departure_time > DateTime.Now
                            && schedule.relationships.stop.id.Equals(selectedDepatureStationName))
                        {
                            viewModel.addSchedule(schedule);
                        }
                    }
                }
                else
                {
                    string selectedArrivalStationName = viewModel.StationList[arrival_index];
                    Schedule departureSchedule;
                    string tripID;
                    int cnt = 0;
                    int maxcount = 10;
                    viewModel.IsBusy = true;
                    foreach (Schedule schedule in Database.RouteSchedules[routeID])
                    {
                        if (schedule.attributes.departure_time > DateTime.Now
                            && schedule.relationships.stop.id.Equals(selectedDepatureStationName))
                        {
                            departureSchedule = schedule;
                            tripID = schedule.relationships.trip.id;
                            Task.Run(async () =>
                            {
                                TripData trip_data = null;
                                List<TripData> tripInfo = await DataQuery.getScheduleOfTrip(routeID,
                                    viewModel.RouteDirection.direction_id, tripID, selectedArrivalStationName);
                                foreach (TripData temp_trip in tripInfo)
                                    if (temp_trip.relationships.trip.data.id.Equals(tripID))
                                    {
                                        trip_data = temp_trip;
                                        break;
                                    }

                                viewModel.addSchedule(departureSchedule, trip_data);
                                cnt++;
                            }).Wait();
                            if (cnt > maxcount)
                                break;

                        }
                    }
                    viewModel.IsBusy = false;
                }
            }
        }
    }
}