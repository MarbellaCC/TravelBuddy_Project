using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TravelBuddy.Models;

namespace TravelBuddy.Services
{
    public class GeocodingService
    {
        public GeocodingService()
        {

        }
        public string GetGeocodingURL(Traveler traveler)
        {
            return $"https://maps.googleapis.com/maps/api/geocode/json?address={traveler.DestinationState}+{traveler.ZipCode}+&key=" + APIKeys.GOOGLE_API_KEY;
        }
        public async Task<Traveler> GetGeocoding(Traveler traveler)
        {
            string apiURL = GetGeocodingURL(traveler);
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiURL);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonResults = JsonConvert.DeserializeObject<JObject>(data);
                    JToken results = jsonResults["results"][0];
                    JToken location = results["geometry"]["location"];

                    traveler.Latitude = (double)location["lat"];
                    traveler.Longitude = (double)location["lng"];
                }
            }
            return traveler;
        } 
    }
}
