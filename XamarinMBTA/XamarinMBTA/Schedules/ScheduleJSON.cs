using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Schedules
{
    public class ScheduleList
    {
        public Schedule[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class Schedule
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Relationships relationships { get; set; }
        public string type { get; set; }
        public string stationName { get; set; }
    }

    public class Attributes
    {
        public DateTime arrival_time { get; set; }
        public DateTime departure_time { get; set; }
        public int drop_off_type { get; set; }
        public int pickup_type { get; set; }
        public int stop_sequence { get; set; }
        public bool timepoint { get; set; }
    }

    public class Relationships
    {
        public SchedulePrediction prediction { get; set; }
        public Route route { get; set; }
        public Stop stop { get; set; }
        public Trip trip { get; set; }
    }

    public class SchedulePrediction
    {
    }

    public class Route
    {
        public Data data { get; set; }
        public string id
        {
            get { return this.data.id; }
        }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Stop
    {
        public Data data { get; set; }
        public string id
        {
            get { return this.data.id; }
        }
    }

    public class Trip
    {
        public Data data { get; set; }
        public string id
        {
            get { return this.data.id; }
        }
    }

}
