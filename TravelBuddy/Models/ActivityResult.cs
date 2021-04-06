using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class ActivityResult
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Name")]
        public string PlaceName { get; set; }
        [Display(Name ="Google Rating")]
        public float GoogleRating { get; set; }
        public string Address { get; set; }
        [Display(Name ="Latitude")]
        public float ActivityLat { get; set; }
        [Display(Name ="Longitude")]
        public float ActivityLng { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
