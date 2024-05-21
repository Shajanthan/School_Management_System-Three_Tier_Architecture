using SMS_Models.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Student.Interface
{
    public interface IStudentRepository
    {
        IEnumerable<StudentBO> GetAllStudents(bool? isActive = null);

        StudentBO GetStudentByID(long id);

        bool DeleteStudent(long id, out string msg);

        bool IsStudentAllocated(long id);

        bool CheckRegNo(string regNo);

        bool CheckStudentDisplayName(string studentDisplayName);

        bool CheckEmail(string eMail);

        bool ToggleStudent(long id, bool isEnable, out string msg);

        bool SaveStudent(StudentBO student, out string msg);

        bool UpdateStudentDetails(StudentBO student, out string msg, SMS_Data.Student editStudent);

        IEnumerable<StudentBO> SearchStudent(string term, string category);
    }
}
