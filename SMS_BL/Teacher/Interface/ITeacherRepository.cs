using SMS_Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Teacher.Interface
{
    public interface ITeacherRepository
    {
        IEnumerable<TeacherBO> GetAllTeachers(bool? isActive = null);

        TeacherBO GetTeacherByID(long id);

        bool DeleteTeacher(long id, out string msg);

        bool IsTeacherAllocated(long id);

        bool CheckRegNo(string regNo);

        bool CheckTeacherDisplayName(string teacherDisplayName);

        bool CheckEmail(string eMail);

        bool ToggleTeacher(long id, bool isEnable, out string msg);

        bool SaveTeacher(TeacherBO teacher, out string msg);

        IEnumerable<TeacherBO> SearchTeacher(string term, string category);



    }

}
