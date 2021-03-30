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
    public class PlacesService
    {
        public PlacesService()
        {

        }
        public string GetPlacesURL(Traveler traveler)
        {
            return $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius=1500&keyword={traveler.Lodging}&key="; //+ APIKeys.GOOGLE_API_KEY;
        }
        public async Task<Traveler> GetPlaces(Traveler traveler)
        {
            string apiURL = GetPlacesURL(traveler);
            using (HttpClient client = new HttpClient())
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
                    JToken name = results["name"];
                    JToken photos = results["photos"];
                    JToken rating = results["rating"];
                    JToken address = results["vicinity"];

                    traveler.Latitude = (double)location["lat"];
                    traveler.Longitude = (double)location["lng"];
                    traveler.HotelName = (string)name;
                    traveler.HotelPhotos = (string)photos;
                    traveler.HotelRating = (string)rating;
                    traveler.HotelAddress = (string)address;

                }
            }
            return traveler;
        }
    }
}
