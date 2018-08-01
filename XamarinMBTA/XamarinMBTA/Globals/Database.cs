using System;
using System.Collections.Generic;
using System.Text;
using XamarinMBTA.Alerts;
using XamarinMBTA.Predictions;
using XamarinMBTA.Routes;
using XamarinMBTA.Schedules;
using XamarinMBTA.Stops;
using XamarinMBTA.Vehicle;
using System.Threading;
namespace XamarinMBTA.Globals
{
    public class Database
    {
        public static bool stationsLoaded = false,
            routesLoaded = false,
            schedulesLoaded = false,
            predictionsLoaded = false,
            vehiclesLoaded = false,
            alertsLoaded = false;

        public static Dictionary<string, List<Station>> RouteStations = new Dictionary<string, List<Station>>();
        public static List<Routine> routeList = new List<Routine>();
        public static Dictionary<string, List<Schedule>> RouteSchedules = new Dictionary<string, List<Schedule>>();
        public static List<Prediction> predictionList = new List<Prediction>();
        public static List<VehicleInfo> vehicleList = new List<VehicleInfo>();
        public static List<Alert> alertList = new List<Alert>();

        public static Dictionary<string, string> routeAbrevMap= new Dictionary<string, string>()
            {
                {"Blue Line","BL"},
                {"Green Line C","GL"},
                {"Green Line E","GL"},
                {"Green Line D","GL"},
                {"Green Line B","GL"},
                {"Charlestown Ferry","F"},
                {"Hingham/Hull Ferry","F"},
                {"Silver Line SL4","SL4"},
                {"Silver Line SL2","SL2"},
                {"Silver Line SL1","SL1"},
                {"Silver Line SL5","SL5"},
                {"Silver Line SL3","SL3"},
                {"Fitchburg Line","CR"},
                {"Foxboro (Special Events)","CR"},
                {"Newburyport/Rockport Line","CR"},
                {"Franklin Line","CR"},
                {"Needham Line","CR"},
                {"Greenbush Line","CR"},
                {"Lowell Line","CR"},
                {"Kingston/Plymouth Line","CR"},
                {"Middleborough/Lakeville Line","CR"},
                {"Haverhill Line","CR"},
                {"Fairmount Line","CR"},
                {"Framingham/Worcester Line","CR"},
                {"Providence/Stoughton Line","CR"},
                {"Mattapan Trolley","MT"},
                {"Red Line","RL"},
                {"Orange Line","OL"}
            };

        public static Dictionary<string, string> routeID_NameMap = new Dictionary<string, string>();

        /*  
         * 0 - 1 Subway
         * 2 Commuter Rail
         * 3 Bus
         * 4 Ferry
         */ 
        public static Dictionary<string, int> routeID_typeMap = new Dictionary<string, int>();

        public static Dictionary<string, int> routeAlertCountMap = new Dictionary<string, int>();
    }
}
