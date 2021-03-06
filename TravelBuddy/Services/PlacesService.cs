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
        public async Task<Locations> GetLodging(Traveler traveler, Lodging lodging)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius={lodging.MaxDistance}&keyword={lodging.LodgingType}&key={APIKeys.GOOGLE_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Locations>(json);
            }
            return null;
        }
        public async Task<Locations> GetActivity(Traveler traveler, Activity activity)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius={activity.MaxDistance}&type={activity.TypeOfActivity}&keyword={activity.TypeOfAdventureOrRestaurant}&key={APIKeys.GOOGLE_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Locations>(json);
            }
            return null;
        }
        public async Task<Locations> GetInterestActivity(Traveler traveler, Activity activity, Interest interests)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius={activity.MaxDistance}&type=tourist_attraction&keyword={interests.Name}&key={APIKeys.GOOGLE_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Locations>(json);
            }
            return null;
        }
    }
}
