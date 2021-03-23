using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBuddy.Models
{
    public class Traveler
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("DayId")]
        public int DayId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName {get; set; }
        public string Bio { get; set; }
        [Display(Name = "City")]
        public string DestinationCity { get; set; }
        [Display(Name = "State")]
        public string DestinationState { get; set; }
        [Display(Name = "Country")]
        public string DestinationCountry { get; set; }
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public string Lodging { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserID { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
