using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        [ForeignKey("Traveler")]
        public int TravelerId { get; set; }
        public Traveler Traveler { get; set; }
    }
}
