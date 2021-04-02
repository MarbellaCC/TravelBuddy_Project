using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }
        public string PlaceName { get; set; }
        [Range(0, 5)]
        [NotMapped]
        public List<double> RatingList { get; set; }
        public double RatingAdded { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
        public float GoogleRating { get; set; }
        public string Address { get; set; }
        [Display(Name = "Max Distane")]
        public double? MaxDistance { get; set; }
        [Display(Name = "Type of Activity")]
        public string TypeOfActivity { get; set; }
        [Display(Name = "Adventure/Restaurant Type")]
        public string TypeOfAdventureOrRestaurant { get; set; }
        public float ActivityLat { get; set; }
        public float ActivityLng { get; set; }
        [ForeignKey("Day")]
        public int DayId { get; set; }
        public Day Day { get; set; } 
    }
}
