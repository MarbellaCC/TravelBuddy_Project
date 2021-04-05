using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Lodging
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photos { get; set; }
        public float GoogleRating { get; set; }
        public string Address { get; set; }
        public float LodgingLat { get; set; }
        public float LodgingLng { get; set; }
        public string LodgingType { get; set; }
        public string MaxDistance { get; set; }
        [ForeignKey("Traveler")]
        public int TravelerId { get; set; }
        public Traveler Traveler { get; set; }
    }
}
