/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using SMS_Models.Teacher;
using System.Collections.Generic;

namespace SMS_ViewModels.Teacher
{
    public class TeacherViewModel
    {
        /// <summary>
        /// GEt all teachers as a list
        /// </summary>
        public IEnumerable<TeacherBO> TeacherList { get; set; }

        /// <summary>
        /// get one teacher
        /// </summary>
        public TeacherBO Teacher { get; set; }
    }
}
