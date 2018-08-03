
using System.ComponentModel.DataAnnotations;

namespace lmsextreg.Models
{
    public class EnrollmentStatus
    {
        [Key]
        [Required]
        [Display(Name = "Status Code")]
        public string StatusCode { get; set; }
        [Required]
         [Display(Name = "Status")]
        public string StatusLabel { get; set; }
    }
}