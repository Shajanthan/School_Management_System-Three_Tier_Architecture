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
    public class StudentAllocationBO
    {
        public long StudentAllocationID { get; set; }
        [Required(ErrorMessage = "Student is required")]
        [DisplayName("Student")]
        public long StudentID { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [DisplayName("Subject")]
        public long SubjectAllocationID { get; set; }
    }
}
