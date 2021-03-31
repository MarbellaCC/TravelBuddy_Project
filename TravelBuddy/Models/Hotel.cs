using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string HotelPhotos { get; set; }
        public float HotelGoogleRating { get; set; }
        public string HotelAddress { get; set; }
        public float HotelLat { get; set; }
        public float HotelLng { get; set; }
        public string Lodging { get; set; }
        public string HotelMaxDistance { get; set; }
        [ForeignKey("Traveler")]
        public int TravelerId { get; set; }
        public Traveler Traveler { get; set; }
    }
}
