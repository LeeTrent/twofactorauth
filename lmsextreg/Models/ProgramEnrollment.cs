using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using lmsextreg.Data;

namespace lmsextreg.Models
{
    public class ProgramEnrollment
    {
        ////////////////////////////////////////////////////////////
        // ProgramEnrollmentID:
        // Primary-key (auto-generated)
        ////////////////////////////////////////////////////////////
        public int ProgramEnrollmentID { get; set; }
        ////////////////////////////////////////////////////////////
        // LMSProgramID:
        // Foreign-key reference to Program table
        ///////////////////////////////////////////////////////////
       [Required]
        public int LMSProgramID { get; set; }

        ////////////////////////////////////////////////////////////
        // LMSProgram:
        // Navigation property to Program entity
        ///////////////////////////////////////////////////////////
        [Display(Name = "Program")]
        public LMSProgram LMSProgram { get; set; }

        /////////////////////////////////////////////////////////////
        // StudentUserId:
        // Same value as the 'Id' column in the 'AspNetUser' table
        // Foreign-key reference to ApplicationUser for STUDENT
        ////////////////////////////////////////////////////////////
        [Required]
        [Display(Name = "Student")]
        public string StudentUserId { get; set; }

        //////////////////////////////////////////////////////////////
        // Student:
        // Navigation property to ApplicationUser entity for STUDENT
        /////////////////////////////////////////////////////////////
        [Display(Name = "Student")]
        public ApplicationUser Student { get; set; }

        ////////////////////////////////////////////////////////////
        // ApproverUserId:
        // Same value as the 'Id' column in the 'AspNetUser' table
        ///////////////////////////////////////////////////////////
        [Display(Name = "Approver")]
        public string ApproverUserId { get; set; }

        ///////////////////////////////////////////////////////////////
        // Approver:
        // Navigation property to ApplicationUser entity for APPROVER
        //////////////////////////////////////////////////////////////
        [Display(Name = "Approver")]
        public ApplicationUser Approver { get; set; }

        [Required]
        public string StatusCode { get; set; }

        [Display(Name = "Status")]
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public ICollection<EnrollmentHistory> EnrollmentHistory { get; set; }

        ////////////////////////////////////////////////////////////
        // UserCreated:
        // User who inserted this row
        // (Same value as the 'Id' column in the 'AspNetUser' table)
        ////////////////////////////////////////////////////////////
        [Required]
        [Display(Name = "Requested By")]
        public string UserCreated { get; set; }

        ////////////////////////////////////////////////////////////
        // DateCreated:
        // Date that row was originally inserted
        ///////////////////////////////////////////////////////////
        [Required]
        [Display(Name = "Date Requested")]
        public DateTime DateCreated { get; set; }

        ////////////////////////////////////////////////////////////
        // UserCreated:
        // User who last updated this row
        // (Same value as the 'Id' column in the 'AspNetUser' table)        
        ///////////////////////////////////////////////////////////
        public string UserLastUpdated { get; set; }        

        ////////////////////////////////////////////////////////////
        // DateCreated:
        // Date that row was last updated
        ///////////////////////////////////////////////////////////
        public DateTime DateLastUpdated { get; set; }
    }
}