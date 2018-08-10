using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using XamarinMBTA.Directions;
using XamarinMBTA.Events;

namespace XamarinMBTA.ViewModels
{
    class UpcomingScheduleViewModel: INotifyPropertyChanged
    {
        public PlannedEvent currentEvent { get; set; }
        public GoogleRoute route { get; set; }

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
                OnPropertyChanged(nameof(RemainingTime));
            }
        }

        private string eventName;
        public string UpcomingTitle
        {
            get
            {
                return eventName;
            }
            set
            {
                eventName = value;
                OnPropertyChanged("UpcomingTitle");
            }
        }
        private string upcoming_T;
        public string RequiredTime
        {
            get
            {
                return upcoming_T;
            }
            set
            {
                upcoming_T = value;
                OnPropertyChanged("RequiredTime");
            }
        }

        private string upcoming_D;
        public String UpcomingDistance
        {
            get
            {
                return upcoming_D;
            }
            set
            {
                upcoming_D = value;
                OnPropertyChanged("UpcomingDistance");
            }
        }

        private string upcomingEventName;
        public String EventName
        {
            get
            {
                return upcomingEventName;
            }
            set
            {
                upcomingEventName = value;
                OnPropertyChanged("EventName");
            }
        }

        public string eventTime;
        public String EventTime
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Position currentPos, targetPos;

        public UpcomingScheduleViewModel()
        {
            UpcomingDistance = "--";
            RequiredTime = "--";
            EventName = "EVENT_NAME";
        }

        public void loadEvent(PlannedEvent pEvent)
        {
            currentEvent = pEvent;
            EventName = pEvent.EventName;
            EventTime = pEvent.EventTime;
            initPos(pEvent.startPos, pEvent.endPos);

        }


        private void initPos(Position current, Position target)
        {
            
            Task.Run(async () =>
            {
                currentPos = current;
                targetPos = target;
                route = await DataQuery.getDirections(currentPos, targetPos);
                UpcomingDistance = route.getDistance();
                RequiredTime = route.getTime();
                RemainingTime = getTimeRemaing(DateTime.Parse(EventTime));
                currentEvent.UpcomingDistance = UpcomingDistance;
                currentEvent.RemainingTime = RemainingTime;
                currentEvent.RequiredTime = RequiredTime;
            }).Wait();
            
        }

        private string getTimeRemaing(DateTime scheduledTime)
        {
            TimeSpan span = scheduledTime.Subtract(DateTime.Now);
            int days = span.Days, hours = span.Hours, minutes = span.Minutes;
            string remainTime = "";
            if (days > 0)
                remainTime += days + "d";
            if (hours > 0)
                remainTime += hours + "h";
            if (minutes > 0)
                remainTime += minutes + "min";
            return remainTime;
        }
    }
}
