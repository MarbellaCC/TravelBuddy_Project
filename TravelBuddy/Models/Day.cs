using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        [Display(Name= "Restaurant Max Distane")]
        public double? RestaurantMaxDistance { get; set; }
        [Display(Name = "Adventure Max Distance")]
        public double? AdventureMaxDistance { get; set; }
        [Display(Name = "Type of Restaurant")]
        public string TypeOfRestaurant { get; set; }
        [Display(Name = "Type of Adventure")]
        public string TypeOfAdventure { get; set; }
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }

    }
}
