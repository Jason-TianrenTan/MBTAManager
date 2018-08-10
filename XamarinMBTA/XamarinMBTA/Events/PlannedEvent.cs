using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace XamarinMBTA.Events
{
    public class PlannedEvent
    {
        public Position startPos { get; set; }
        public Position endPos { get; set; }
        public string EventTime { get; set; }
        public string EventName { get; set;}
        public string RemainingTime { get; set; }
        public string RequiredTime { get; set; }
        public string UpcomingDistance { get; set; }
    }
}
