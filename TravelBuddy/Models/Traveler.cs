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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName {get; set; }
        public string Interests { get; set; }
        [Display(Name = "City")]
        public string DestinationCity { get; set; }
        [Display(Name = "State")]
        public string DestinationState { get; set; }
        [Display(Name = "Country")]
        public string DestinationCountry { get; set; }
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }
        public string Lodging { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserID { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
