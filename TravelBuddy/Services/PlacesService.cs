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
        //public string GetPlacesURL(Traveler traveler)
        //{
        //    return $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius={traveler.HotelMaxDistance}&keyword={traveler.Lodging}&key=" + APIKeys.GOOGLE_API_KEY;
        //}
        //public async Task<Location> GetPlaces(Traveler traveler)
        //{
        //    string apiURL = GetPlacesURL(traveler);
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(apiURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        HttpResponseMessage response = await client.GetAsync(apiURL);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<Location>(data);
        //            //JToken results = jsonResults["results"][0];
        //            //JToken location = results["geometry"]["location"];
        //            //JToken name = results["name"];
        //            //JToken photos = results["photos"][0];
        //            //JToken rating = results["rating"];
        //            //JToken address = results["vicinity"];

        //            //traveler.Latitude = (double)location["lat"];
        //            //traveler.Longitude = (double)location["lng"];
        //            //traveler.HotelName = (string)name;
        //            //traveler.HotelPhotos = (string)photos["html_attributions"][0];
        //            //traveler.HotelRating = (string)rating;
        //            //traveler.HotelAddress = (string)address;

        //        }
        //    }
        //    return null;
        //}
        public async Task<Locations> GetHotels(Traveler traveler, Hotel hotel)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={traveler.Latitude},{traveler.Longitude}&radius={hotel.HotelMaxDistance}&keyword={hotel.Lodging}&key={APIKeys.GOOGLE_API_KEY}");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Locations>(json);
            }
            return null;
        }
    }
}
