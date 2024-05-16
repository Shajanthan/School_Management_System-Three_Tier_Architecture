/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Models.Allocation
{
    public class SubjectAllocationBO
    {
        public long SubjectAllocationID { get; set; }

        [Required(ErrorMessage = "Teacher is required")]
        [DisplayName("Teacher ID")]
        public long TeacherID { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [DisplayName("Registration Number")]
        public long SubjectID { get; set; }

    }
}
