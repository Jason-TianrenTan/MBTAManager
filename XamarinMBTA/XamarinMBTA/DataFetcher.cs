using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XamarinMBTA
{
    class DataFetcher
    {
        //curl -X GET "https://api-v3.mbta.com/stops/70076" -H  "accept: application/vnd.api+json"
        public async static Task<String> fetchData(String url)
        {
            //Referencing from David S. Platt
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            string contentString = await response.Content.ReadAsStringAsync();
            return contentString;
        }
    }
}
