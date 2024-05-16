/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using SMS_Models.Student;
using System.Collections.Generic;

namespace SMS_ViewModels.Student
{
    public class StudentViewModel
    {
        /// <summary>
        /// get all the student for students
        /// </summary>
        public IEnumerable<StudentBO> StudentList { get; set; }

        /// <summary>
        /// get the specific student
        /// </summary>
        public StudentBO Student { get; set; }

    }
}
