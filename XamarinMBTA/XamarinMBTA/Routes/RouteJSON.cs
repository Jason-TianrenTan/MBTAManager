using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Routes
{
    /*
     * Route types:
     * 
     *    0 - Tram, Streetcar, Light rail. Any light rail or street level system within a metropolitan area.
	    * 1 - Subway, Metro. Any underground rail system within a metropolitan area.
		* 2 - Rail. Used for intercity or long-distance travel.
		* 3 - Bus. Used for short- and long-distance bus routes.
		* 4 - Ferry. Used for short- and long-distance boat service.
		* 5 - Cable car. Used for street-level cable cars where the cable runs beneath the car.
		* 6 - Gondola, Suspended cable car. Typically used for aerial cable cars where the car is suspended from the cable.
		* 7 - Funicular. Any rail system designed for steep inclines.
        */
    public class RouteList
    {
        public Routine[] data { get; set; }
        public Jsonapi jsonapi { get; set; }
    }

    public class Jsonapi
    {
        public string version { get; set; }
    }

    public class Routine
    {
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
    }

    public class Attributes
    {
        public string color { get; set; }
        public string description { get; set; }
        public string[] direction_names { get; set; }
        public string long_name { get; set; }
        public string short_name { get; set; }
        public int sort_order { get; set; }
        public string text_color { get; set; }
        public int type { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

}
