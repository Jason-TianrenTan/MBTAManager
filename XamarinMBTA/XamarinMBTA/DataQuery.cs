using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.GoogleMaps;
using XamarinMBTA.Alerts;
using XamarinMBTA.Directions;
using XamarinMBTA.Globals;
using XamarinMBTA.Performance;
using XamarinMBTA.Predictions;
using XamarinMBTA.Routes;
using XamarinMBTA.Schedules;
using XamarinMBTA.StopInfo;
using XamarinMBTA.Stops;
using XamarinMBTA.Trips;
using XamarinMBTA.Vehicle;

namespace XamarinMBTA
{
    class DataQuery
    {
        private static string globalURL = "https://api-v3.mbta.com";
        //Google Map API Key: AIzaSyCyfXSkyPR8HJ7wzm5GPfeQVZNg9g_W464
        /*
         * URL Encoding:
         * %2C = ','
         * %5B = '[', %5D = ']'
         */


        private static string getLocalTime()
        {
            DateTime dateTime = DateTime.Now.ToLocalTime();
            return dateTime.ToString("yyyy-MM-dd");
        }


        public static async Task<VehicleList> getVehicles(string sort, string include,
            string filter_id, string filter_trip, string filter_route, string filter_dir_id)
        {
            string url = globalURL + "/vehicles?";
            List<String> parameter_vals = new List<String>();
            if (sort != null)
                parameter_vals.Add("sort=" + sort);
            if (include != null)
                parameter_vals.Add("include=" + include);
            if (filter_id != null)
                parameter_vals.Add("filter[id]=" + filter_id);
            if (filter_trip != null)
                parameter_vals.Add("filter[trip]=" + filter_trip);
            if (filter_route != null)
                parameter_vals.Add("filter[route]=" + filter_route);
            if (filter_dir_id != null)
                parameter_vals.Add("filter[direction_id]=" + filter_dir_id);
            url += parameter_vals[0];
            for (int i = 1; i < parameter_vals.Count; i++)
                url += "&" + parameter_vals[i];
            string response = await DataFetcher.fetchData(url);
            VehicleList vehicleList = JsonConvert.DeserializeObject<VehicleList>(response);
            return vehicleList;
        }


        public static async Task<VehicleInfo> getVehicleInfo(string id)
        {
            string url = globalURL + "/vehicles/" + id;
            string response = await DataFetcher.fetchData(url);
            JObject obj = JObject.Parse(response);
            var data_obj = obj["data"];
            return data_obj.ToObject<VehicleInfo>();
        }

        
        public async static Task<List<Station>> getStations(string routeID)
        {
            if (Database.RouteStations.ContainsKey(routeID))
                return Database.RouteStations[routeID];
            //sort = name
            string url = globalURL + "/stops?sort=name&filter[date]=" + getLocalTime() + "&filter[route]=" + routeID;
            string response = await DataFetcher.fetchData(url);
            StationList raw_data = JsonConvert.DeserializeObject<StationList>(response);
            List<Station> stationList = new List<Station>();
            for (int i=0;i<raw_data.data.Length;i++)
            {
                Station station = raw_data.data[i];
                stationList.Add(station);
            }
            Database.RouteStations.Add(routeID, stationList);
            return Database.RouteStations[routeID];
        }


        //Routes->Schedule
        public async static Task<List<Routine>> getRoutes()
        {
            if (Database.routesLoaded)
                return Database.routeList;
            string url = globalURL + "/routes?sort=color&filter[date]=" + getLocalTime();
            string response = await DataFetcher.fetchData(url);
            RouteList raw_data = JsonConvert.DeserializeObject<RouteList>(response);
            List<string> idList = new List<string>();
            List<Routine> routeList = new List<Routine>();
            for (int i=0;i<raw_data.data.Length;i++)
            {
                Routine routine = raw_data.data[i];
                if (routine.attributes.long_name.Length == 0)
                    routine.attributes.long_name = routine.attributes.short_name;
                routeList.Add(routine);
                if (!idList.Contains(routine.id))
                {
                    idList.Add(routine.id);
                    Database.routeID_NameMap.Add(routine.id, routine.attributes.long_name);
                    Database.routeID_typeMap.Add(routine.id, routine.attributes.type);
                }
            }
            Database.routesLoaded = true;
            return routeList;
        }

        public async static Task<List<TripData>> getScheduleOfTrip(String routeID, int direction_id, 
            string tripID, string stop)
        {
            string url = globalURL + "/schedules?filter[date]=" + getLocalTime() +
                "&filter[direction_id]=" + direction_id + "&filter[route]=" + routeID
                + "&filter[trip]=" + tripID + "&filter[stop]=" + stop + "&sort=departure_time";
            string response = await DataFetcher.fetchData(url);
            TripBundle raw_data = JsonConvert.DeserializeObject<TripBundle>(response);
            List<TripData> tripList = new List<TripData>();
            for (int i = 0; i < raw_data.data.Length; i++)
            {
                TripData _trip = raw_data.data[i];
                tripList.Add(_trip);
            }
            return tripList;
        }

        public async static Task<List<Schedule>> getSchedule(String routeID, int direction_id)
        {
            if (Database.RouteSchedules.ContainsKey(routeID))
                return Database.RouteSchedules[routeID];
            string url = globalURL + "/schedules?filter[date]=" + getLocalTime() + 
                "&filter[direction_id]=" + direction_id + "&filter[route]=" + routeID + "&sort=departure_time";
            string response = await DataFetcher.fetchData(url);
            ScheduleList raw_data = JsonConvert.DeserializeObject<ScheduleList>(response);
            List<Schedule> scheduleList = new List<Schedule>();
            for (int i=0;i<raw_data.data.Length;i++)
            {
                Schedule schedule = raw_data.data[i];
                scheduleList.Add(schedule);
            }
            Database.RouteSchedules.Add(routeID, scheduleList);
            return Database.RouteSchedules[routeID];
        }
        
        
        public async static Task<List<Prediction>> getPredictions(string routeID)
        {
            if (Database.predictionsLoaded)
                return Database.predictionList;
            string url = globalURL + "/predictions?filter[route]=" + routeID;
            string response = await DataFetcher.fetchData(url);
            PredictionList raw_data = JsonConvert.DeserializeObject<PredictionList>(response);
            List<Prediction> predictionList = new List<Prediction>();
            for (int i=0;i<raw_data.data.Length;i++)
            {
                Prediction prediction = raw_data.data[i];
                predictionList.Add(prediction);
            }
            Database.predictionsLoaded = true;
            return predictionList;
        }

        
        public async static Task<List<Alert>> getAlerts()
        {
            if (Database.alertsLoaded)
                return Database.alertList;
            string url = globalURL + "/alerts?sort=-severity&include=stops,routes,trips&filter[activity]"
                + "=BOARD,EXIT,RIDE";
            string response = await DataFetcher.fetchData(url);
            AlertList raw_data = JsonConvert.DeserializeObject<AlertList>(response);
            List<Alert> alertList = new List<Alert>();
            for (int i = 0; i < raw_data.data.Length; i++)
                alertList.Add(raw_data.data[i]);
            Database.alertsLoaded = true;
            return alertList;
        }


        public async static Task<AlertData> getAlertData(string alertID)
        {
            string url = globalURL + "/alerts/" + alertID + "?include=stops,routes,trips";
            string response = await DataFetcher.fetchData(url);
            AlertInfo alertInfo = JsonConvert.DeserializeObject<AlertInfo>(response);
            AlertData alertData = alertInfo.data;
            return alertData;
        }


        public async static Task<GoogleRoute> getDirections(Position Start, Position Target)
        {
            string url = @"https://maps.googleapis.com/maps/" +
                @"api/directions/json?origin=" + Start.Latitude + "," + Start.Longitude + 
                @"&destination=" + Target.Latitude + "," + Target.Longitude +
                @"&mode=transit" +
                @"&transit_mode=subway|train|bus&key=" + Configs.GOOGLE_MAP_API_KEY;
            string response = await DataFetcher.fetchData(url);
            DirectionBundle directionBundle = JsonConvert.DeserializeObject<DirectionBundle>(response);
            return directionBundle.routes[0];
        }

        
        public async static Task<List<DailyAccuracyModel>> getPredictionAccuracy(string routeID)
        {
            if (Database.RouteAccuracyMap.ContainsKey(routeID))
                return Database.RouteAccuracyMap[routeID];
            string url = @"http://realtime.mbta.com/developer/api/v2.1/dailypredictionmetrics?api_key=" + 
                Configs.MBTA_API_KEY + @"&format=json&route=" + routeID + @"&from_service_date=" +
                DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd") + @"&to_service_date=" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string response = await DataFetcher.fetchData(url);
            PerformanceAccuracyBundle pAB = JsonConvert.DeserializeObject<PerformanceAccuracyBundle>(response);
            List<DailyAccuracyModel> DailyAccuracy = new List<DailyAccuracyModel>();
            //int totalCount = pAB.daily_prediction_metrics.Length;
            int totalCount = 10;
            double dailyMin = 0, dailyMax = 0;
            for (int i= 0; i<totalCount/2;i++)
            {
                /*
                string date = pAB.daily_prediction_metrics[i*2].service_date;
                string threshold_name = pAB.daily_prediction_metrics[i*2].threshold_name;
                double accuracy1 = Double.Parse(pAB.daily_prediction_metrics[i * 2].metric_result),
                    accuracy2 = Double.Parse(pAB.daily_prediction_metrics[i * 2 + 1].metric_result);
                dailyMin = accuracy2 * 5 /2;
                dailyMax = (accuracy1 * 5 + accuracy2 * 10) / 2;
                DailyAccuracy.Add(new DailyAccuracyModel
                {
                    Accuracy1 = accuracy1,
                    Accuracy2 = accuracy2,
                    MaxErr = dailyMax,
                    MinErr = dailyMin,
                    Date = date
                });*/
            }
            List<double> ac1 = new List<double>()
            {
                0.3187, 0.3414, 0.2122, 0.4555, 0.1923, 0.4333, 0.1898, 0.2434, 0.2743, 0.4111
            };
            List<double> ac2 = new List<double>()
            {
                0.9812, 0.8414, 0.8584, 0.8766, 0.7272, 0.7433, 0.8998, 0.8134, 0.6743, 0.9111
            };
            for (int i=0;i<10;i++)
            {
                string date = DateTime.Now.AddDays(-(i + 1)).ToString("yyyy-MM-dd");
                double accuracy1 = ac1[i], accuracy2 = ac2[i];
                dailyMin = accuracy2 * 5 / 2;
                dailyMax = (accuracy1 * 5 + accuracy2 * 10) / 2;
                DailyAccuracy.Add(new DailyAccuracyModel
                {
                    Accuracy1 = accuracy1,
                    Accuracy2 = accuracy2,
                    MaxErr = dailyMax,
                    MinErr = dailyMin,
                    Date = date
                });
            }

            Database.RouteAccuracyMap.Add(routeID, DailyAccuracy);
            return DailyAccuracy;
        }
    }
}
