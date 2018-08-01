using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Vehicle
{
    public class VehicleList
    {
        public VehicleInfo[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class VehicleInfo
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Links links { get; set; }
        public Relationships relationships { get; set; }
        public string type { get; set; }
        public override string ToString()
        {
            string ret = "ID: " + this.id + "\n"
                + "Position: " + this.attributes.longitude + ", " + this.attributes.latitude + "\n"
                + "Current Status: " + this.attributes.current_status + "\n"
                + "Speed = " + this.attributes.speed + "\n"
                + "Route = " + this.relationships.route + "\n"
                + "Stop = " + this.relationships.stop + "\n"
                + "Trip: " + this.relationships.trip + "\n"
                + "Label: " + this.attributes.label + "\n"
                + "DateTime: " + this.attributes.updated_at + "\n";
            return ret;
        }
    }

    public class Attributes
    {
        public double? bearing { get; set; }
        public string current_status { get; set; }
        public int? current_stop_sequence { get; set; }
        public int? direction_id { get; set; }
        public string label { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public double? speed { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
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
        public override string ToString()
        {
            if (data == null)
                return "null";
            return this.data.ToString();
        }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
        public override string ToString()
        {
            return this.id + ", " + this.type;
        }
    }

    public class Stop
    {
        public Data data { get; set; }
        public override string ToString()
        {
            if (data == null)
                return "null";
            return data.ToString();
        }
    }

    public class Trip
    {
        public Data data { get; set; }
        public override string ToString()
        {
            if (data == null)
                return "null";
            return this.data.ToString();
        }
    }

}
