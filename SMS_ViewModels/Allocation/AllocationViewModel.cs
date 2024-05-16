/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using SMS_Models.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_ViewModels.Allocation
{
    public class AllocationViewModel
    {
        /// <summary>
        /// Get Subject Allocation List
        /// </summary>
        public IEnumerable<SubjectAllocationBO> SubjectAllocationList { get; set; }

        /// <summary>
        /// Get Student Allocation List
        /// </summary>
        public IEnumerable<StudentAllocationBO> StudentAllocationList { get; set; }
    }
}
