/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using SMS_Models.Subject;
using System.Collections.Generic;

namespace SMS_ViewModels.Subject
{
    public class SubjectViewModel
    {
        /// <summary>
        /// Get the all subjects as list
        /// </summary>
        public IEnumerable<SubjectBO> SubjectList { get; set; }
    }
}
