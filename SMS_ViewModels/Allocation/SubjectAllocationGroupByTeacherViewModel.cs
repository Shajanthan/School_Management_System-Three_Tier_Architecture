/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_ViewModels.Allocation
{
    public class SubjectAllocationGroupByTeacherViewModel
    {

        [DisplayName("Teacher Name")]
        public string TeacherName { get; set; }

        [DisplayName("Teacher Reg No")]
        public string TeacherRegNo { get; set; }

        public IEnumerable<SubjectAllocationViewModel> SubjectAllocations { get; set; }
    }
}
