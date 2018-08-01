using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinMBTA.Globals;
using XamarinMBTA.Schedules;
using XamarinMBTA.Stops;
using XamarinMBTA.Trips;

namespace XamarinMBTA.ViewModels
{
    class LineScheduleViewModel : INotifyPropertyChanged
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        public class ScheduleTime
        {
            public string DepartingTime { get; set; }
            public string ArriveTime { get; set; }
            public ScheduleTime(Schedule departure_schedule)
            {
                this.DepartingTime = departure_schedule.attributes.departure_time.ToString("hh:mm");
                //this.ArriveTime = schedule.attributes.arrival_time.ToString("hh:mm");
            }

            public ScheduleTime(Schedule departure_schedule, TripData arrival_schedule)
            {
                this.DepartingTime = departure_schedule.attributes.departure_time.ToString("hh:mm");
                this.ArriveTime = arrival_schedule.attributes.arrival_time.ToString("hh:mm");
            }
        }

        public ObservableCollection<ScheduleTime> DisplayScheduleTimes;

        public void addSchedule(Schedule schedule)
        {
            this.DisplayScheduleTimes.Add(new ScheduleTime(schedule));
        }

        public void addSchedule(Schedule depature_schedule, TripData arrival_schedule)
        {
            this.DisplayScheduleTimes.Add(new ScheduleTime(depature_schedule, arrival_schedule));
        }

        public ObservableCollection<string> StationList { get; set; }
        public class Direction
        {
            public int direction_id { get; set; } = 0;
            public void flip()
            {
                direction_id = 1 - direction_id;
            }
            public string final_station { get; set; } = "Final";
            public string start_station { get; set; } = "Start";

            public string dir_str
            {
                get
                {
                    if (direction_id == 0)
                        return start_station + "=>" + final_station;
                    else return final_station + "=>" + start_station;
                }
            }

        }

        private Direction _dir;
        public Direction RouteDirection
        {
            get
            {
                return _dir;
            }
            set
            {
                _dir = value;
                OnPropertyChanged(nameof(RouteDirection));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _lineColor;
        public string LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                _lineColor = value;
                OnPropertyChanged(nameof(LineColor));
            }
        }

        private string _lineCode;
        public string LineCode
        {
            get
            {
                return _lineCode;
            }
            set
            {
                _lineCode = value;
                OnPropertyChanged(nameof(LineCode));
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                this._selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }


        private string _departing;
        public string Departing
        {
            get
            {
                return _departing;
            }
            set
            {
                _departing = value;
                OnPropertyChanged(Departing);
            }
        }

        private string _arriving;
        public string Arriving
        {
            get
            {
                return _arriving;
            }
            set
            {
                _arriving = value;
                OnPropertyChanged(nameof(Arriving));
            }
        }
        
        public LineScheduleViewModel()
        {
            DisplayScheduleTimes = new ObservableCollection<ScheduleTime>();
            StationList = new ObservableCollection<string>();
            SelectedDate = DateTime.Now.ToString("yyyy-MM-dd");
            Departing = "Tap to select";
            Arriving = "Tap to select";
            LineCode = "RL";
            LineColor = "#E51C23";
            RouteDirection = new Direction();
        }

        public string SelectedRoute { get; set; }

        public List<Schedule> ScheduleList { get; set; }
        public void updateStations(string routeName)//
        {
            StationList.Clear();
            Task.Run(async () =>
            {
                string routeID = Database.routeID_NameMap.FirstOrDefault(x => x.Value == routeName).Key;
                SelectedRoute = routeID;
                ScheduleList = await DataQuery.getSchedule(routeID, RouteDirection.direction_id);

                if (Database.routeAbrevMap.ContainsKey(routeName))
                {
                    LineCode = Database.routeAbrevMap[routeName];
                    LineColor = Configs.lineColorMap[LineCode];
                }
                else
                {
                    LineCode = routeName;
                    LineColor = "#AEA29F";
                }

                List<Station> stationList = await DataQuery.getStations(routeID);
                Station start_st = stationList[0], final_st = stationList[stationList.Count - 1];
                RouteDirection.start_station = start_st.attributes.name;
                RouteDirection.final_station = final_st.attributes.name;
                foreach (Station station in stationList)
                    StationList.Add(station.attributes.name);
                
            }).Wait();
        }
    }
}
