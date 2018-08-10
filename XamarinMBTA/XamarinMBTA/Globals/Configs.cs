using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamarinMBTA.Globals
{
    public class Configs
    {
        public static string GOOGLE_MAP_API_KEY = "AIzaSyB0bHN-sZg_J9721Gb1VUjhpdDeu30bQxA";
        public static string MBTA_API_KEY = "wX9NwuHnZU2ToO7GmGR9uw";
        public static Dictionary<string, string> lineColorMap = new Dictionary<string, string>()
        {
            { "RL", "#F01014"},
            { "BL", "#003DA5"},
            { "GL", "#00843D"},
            { "OL", "#ED8B00"},
            { "MT", "#DA291C"},
            { "SL1", "#7C878E"},
            { "SL2", "#7C878E"},
            { "SL3", "#7C878E"},
            { "SL4", "#7C878E"},
            { "SL5", "#7C878E"},
            { "BUS", "#FFC72C"},
            { "CR", "#80276C"},
            { "F", "#008EAA" }
        };

        public static double Lat = 0, Lng = 0;


        public static List<Location> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<Location> poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location();
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }
    }
}
