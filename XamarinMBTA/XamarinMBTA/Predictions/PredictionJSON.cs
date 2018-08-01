using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Predictions
{

    public class PredictionList
    {
        public Prediction[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class Prediction
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Relationships relationships { get; set; }
        public string type { get; set; }
    }

    public class Attributes
    {
        public DateTime? arrival_time { get; set; }
        public DateTime? departure_time { get; set; }
        public int direction_id { get; set; }
        public string schedule_relationship { get; set; }
        public string status { get; set; }
        public int? stop_sequence { get; set; }
        public object track { get; set; }
    }

    public class Relationships
    {
        public Route route { get; set; }
        public Stop stop { get; set; }
        public Trip trip { get; set; }
    }

    public class Route
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Stop
    {
        public Data data { get; set; }
    }


    public class Trip
    {
        public Data data { get; set; }
    }


}
