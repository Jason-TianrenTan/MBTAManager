using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Performance
{

    public class PerformanceAccuracyBundle
    {
        public PerformanceAccuracy[] daily_prediction_metrics { get; set; }
    }

    public class PerformanceAccuracy
    {
        public string service_date { get; set; }
        public string route_id { get; set; }
        public string threshold_id { get; set; }
        public string threshold_type { get; set; }
        public string threshold_name { get; set; }
        public string metric_result { get; set; }
    }

}
