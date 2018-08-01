using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Stops
{
    public class StationList
    {
        public Station[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
        
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class Station
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Links links { get; set; }
        public Relationships relationships { get; set; }
        public string type { get; set; }
    }

    public class Attributes
    {
        public object address { get; set; }
        public object description { get; set; }
        public float latitude { get; set; }
        public int location_type { get; set; }
        public float longitude { get; set; }
        public string name { get; set; }
        public object platform_code { get; set; }
        public object platform_name { get; set; }
        public int wheelchair_boarding { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class Relationships
    {
        public Child_Stops child_stops { get; set; }
        public Facilities facilities { get; set; }
        public Parent_Station parent_station { get; set; }
    }

    public class Child_Stops
    {
        public object[] data { get; set; }
    }

    public class Facilities
    {
        public FacilityLink links { get; set; }
    }

    public class FacilityLink
    {
        public string related { get; set; }
    }

    public class Parent_Station
    {
        public object data { get; set; }
    }

}
