using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using lmsextreg.Models;

namespace lmsextreg.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string JobTitle { get; set; }   
        
        [Required]
        public string AgencyID { get; set; }
                
        [Required]
        public string SubAgencyID { get; set; }
              
        [Required]
        public DateTime DateRegistered { get; set; }
        
        [Required]
        public DateTime DateAccountExpires { get; set; }
        
        [Required]
        public DateTime DatePasswordExpires { get; set; }

        [Required]
        public bool RulesOfBehaviorAgreedTo { get; set; }

        ///////////////////////////////////
        // Full Name: Derived Value
        ///////////////////////////////////        
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        ///////////////////////////////////
        // Navigation Property
        ///////////////////////////////////        
        public Agency Agency { get; set; }
        
        ///////////////////////////////////
        // Navigation Property
        ///////////////////////////////////        
        public SubAgency SubAgency { get; set; }
    }
}
