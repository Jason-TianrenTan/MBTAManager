using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Alerts
{

    public class AlertInfo
    {
        public AlertData data { get; set; }
        public Jsonapi jsonapi { get; set; }
    }

    public class AlertData
    {
        public Attrs attributes { get; set; }
        public string id { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
    }

    public class Attrs
    {
        public Active_Period[] active_period { get; set; }
        public object banner { get; set; }
        public string cause { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
        public string effect { get; set; }
        public string header { get; set; }
        public Informed_Entity[] informed_entity { get; set; }
        public string lifecycle { get; set; }
        public string service_effect { get; set; }
        public int severity { get; set; }
        public string short_header { get; set; }
        public string timeframe { get; set; }
        public DateTime updated_at { get; set; }
        public object url { get; set; }
    }

    public class _Active_Period
    {
        public DateTime end { get; set; }
        public DateTime start { get; set; }
    }

    public class _Informed_Entity
    {
        public string[] activities { get; set; }
        public string route { get; set; }
        public int route_type { get; set; }
    }

    public class _Links
    {
        public string self { get; set; }
    }

    public class _Jsonapi
    {
        public string version { get; set; }
    }

}
