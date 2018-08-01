using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Trips
{

    public class TripBundle
    {
        public TripData[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
        public Links links { get; set; }
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class Links
    {
        public string first { get; set; }
        public string last { get; set; }
        public string next { get; set; }
    }

    public class TripData
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Relationships relationships { get; set; }
        public string type { get; set; }
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
        public TripPrediction prediction { get; set; }
        public Route route { get; set; }
        public Stop stop { get; set; }
        public Trip trip { get; set; }
    }

    public class TripPrediction
    {
    }

    public class Route
    {
        public Data data { get; set; }
    }

    public class Stop
    {
        public Data data { get; set; }
    }

    public class Trip
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
    }


}
