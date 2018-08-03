using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace lmsextreg.Models
{
    public class LMSProgram
    { 
        [Required]
        public int LMSProgramID { get; set; }
        [Required]
        [Display(Name = "Program Code")]
        public string ShortName { get; set; }
        [Required]
        [Display(Name = "Program Name")]
        public string LongName { get; set; }
        public string CommonInbox { get; set; }
        public ICollection<ProgramApprover> ProgramApprovers { get; set; }
    }
}