using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class LodgingResult
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name ="Google Rating")]
        public float GoogleRating { get; set; }
        public string Address { get; set; }
        [Display(Name ="Latitude")]
        public float LodgingLat { get; set; }
        [Display(Name ="Longitude")]
        public float LodgingLng { get; set; }
        [ForeignKey("Lodging")]
        public int LodgingId { get; set; }
        public Lodging Lodging { get; set; }
    }
}
