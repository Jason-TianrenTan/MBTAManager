using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinMBTA.Directions;
using XamarinMBTA.Events;
using XamarinMBTA.Globals;
using XamarinMBTA.Performance;

namespace XamarinMBTA.ViewModels
{
    public class EventDetailViewModel: INotifyPropertyChanged
    {
        private GoogleRoute route;
        public PlannedEvent currentEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string eventTitle;
        public string EventTitle
        {
            get
            {
                return eventTitle;
            }
            set
            {
                eventTitle = value;
                OnPropertyChanged("EventTitle");
            }
        }

        private string requiredTime;
        public string RequiredTime
        {
            get
            {
                return requiredTime;
            }
            set
            {
                requiredTime = value;
                OnPropertyChanged(nameof(RequiredTime));
            }
        }

        private string remainingTime;
        public string RemainingTime
        {
            get
            {
                return remainingTime;
            }
            set
            {
                remainingTime = value;
                OnPropertyChanged("RemainingTime");
            }
        }

        private string upcomingDistance;
        public string UpcomingDistance
        {
            get
            {
                return upcomingDistance;
            }
            set
            {
                upcomingDistance = value;
                OnPropertyChanged("UpcomingDistance");
            }
        }

        private string eventTime;
        public string EventTime
        {
            get
            {
                return eventTime;
            }
            set
            {
                eventTime = value;
                OnPropertyChanged("EventTime");
            }
        }

        private string startPos, endPos;
        public string StartPos
        {
            get
            {
                return startPos;
            }
            set
            {
                startPos = value;
                OnPropertyChanged("StartPos");
            }
        }

        public string EndPos
        {
            get
            {
                return endPos;
            }
            set
            {
                endPos = value;
                OnPropertyChanged("StartPos");
            }
        }

        public EventDetailViewModel(PlannedEvent pEvent, GoogleRoute rt)
        {
            currentEvent = pEvent;
            route = rt;
            EventTitle = pEvent.EventName;
            EventTime = pEvent.EventTime;
            StartPos = route.getStartLocation();
            EndPos = route.getEndLocation();
            RemainingTime = pEvent.RemainingTime;
            RequiredTime = pEvent.RequiredTime;
            stepModels = new ObservableCollection<StepDisplayModel>();
            loadRouteDetails();
            loadStatistics();
        }


        private static DateTime currentTime { get; set; }

        private static HashSet<string> usedRoutes = new HashSet<string>();
        public class StepDisplayModel
        {
            public string stepDescription { get; set; }
            public string distance { get; set; }
            public string duration { get; set; }
            public string departureTime { get; set; }
            public string arrivalTime { get; set; }
            public string arrivalStop { get; set; }
            public string depatureStop { get; set; }
            public string routeLine { get; set; }
            public Boolean hasRoute { get; set; }
            public string LineColor { get; set; } = "#AEA29F";
            public string LineCode { get; set; } = "WALK";
            public StepDisplayModel(Step step)
            {
                stepDescription = step.html_instructions;

                double distValue = 0;
                string distUnit = "";
                double distMiles = step.distance.value / 1609.344;
                if (distMiles < 0.1)
                {
                    distValue = distMiles * 5280;
                    distUnit = " ft";
                }
                else
                {
                    distValue = Math.Round(distMiles,2);
                    distUnit = " miles";
                }
                distance = Math.Round(distValue, 2) + distUnit;
                duration = Math.Round((double)(step.duration.value / 60), 2) + " minutes";
                hasRoute = false;
                
                if (step.transit_details != null)
                {
                    arrivalTime = step.transit_details.arrival_time.text;
                    departureTime = step.transit_details.departure_time.text;
                    arrivalStop = step.transit_details.arrival_stop.name;
                    departureTime = step.transit_details.departure_stop.name;
                    routeLine = step.transit_details.line.name;
                    if (routeLine != null)
                    {
                        hasRoute = true;
                        usedRoutes.Add(routeLine);
                        if (Database.routeAbrevMap.ContainsKey(routeLine))
                        {
                            LineCode = Database.routeAbrevMap[routeLine];
                            LineColor = Configs.lineColorMap[LineCode];
                        }
                        else
                        {
                            LineCode = routeLine;
                            LineColor = "#AEA29F";
                        }
                        
                    }
                }



            }
        }

        public ObservableCollection<StepDisplayModel> stepModels { get; set; }
        private void loadRouteDetails()
        {
            currentTime = DateTime.Now;
            foreach (Step step in route.legs[0].steps)
                stepModels.Add(new StepDisplayModel(step));
        }

        public class StatisticModel
        {
            public string LineColor { get; set; }
            public string LineCode { get; set; }
            public double MaxErr { get; set; }
            public double MinErr { get; set; }
            public double AverErr { get; set; }
            public List<DailyAccuracyModel> AccuracyPerformances { get; set; }
        }

        public ObservableCollection<StatisticModel> routeStatList = new ObservableCollection<StatisticModel>();
        private void loadStatistics()
        {
            foreach (string routeName in usedRoutes)
            {
                string lineCode, lineColor;
                if (Database.routeAbrevMap.ContainsKey(routeName))
                {
                    lineCode = Database.routeAbrevMap[routeName];
                    lineColor = Configs.lineColorMap[lineCode];
                }
                else
                {
                    lineCode = routeName;
                    lineColor = "#AEA29F";
                }

                Task.Run(async () =>
                {
                    await DataQuery.getRoutes();
                    string routeID = Database.routeID_NameMap.FirstOrDefault(x => x.Value == routeName).Key;
                    List<DailyAccuracyModel> acModels = new List<DailyAccuracyModel>();
                    acModels = await DataQuery.getPredictionAccuracy(routeID);

                    StatisticModel sModel = new StatisticModel
                    {
                        LineCode = lineCode,
                        LineColor = lineColor,
                        AccuracyPerformances = acModels
                    };
                    double currentMin = 1000, currentMax = -1, currentAver = 0;
                    foreach (DailyAccuracyModel dAModel in acModels)
                    {
                        currentMin = currentMin < dAModel.MinErr ? currentMin : dAModel.MinErr;
                        currentMax = currentMax > dAModel.MaxErr ? currentMax : dAModel.MaxErr;
                        currentAver += (dAModel.MinErr + dAModel.MaxErr) / 2;
                    }
                    currentAver /= acModels.Count;
                    sModel.AverErr = Math.Round(currentAver, 2);
                    sModel.MinErr = Math.Round(currentMin, 2);
                    sModel.MaxErr = Math.Round(currentMax, 2);
                    routeStatList.Add(sModel);
                }).Wait();

                
            }
        }

    }
}
