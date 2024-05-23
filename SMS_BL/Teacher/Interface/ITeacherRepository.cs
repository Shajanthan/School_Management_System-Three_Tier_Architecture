/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
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
        /// <summary>
        /// Get the all teacher 
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEnumerable<TeacherBO> GetAllTeachers(bool? isActive = null);

        /// <summary>
        /// Get the specific teacher details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TeacherBO GetTeacherByID(long id);

        /// <summary>
        /// Delete the teacher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteTeacher(long id, out string msg);

        /// <summary>
        /// check the teacher is allocated to the subject or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsTeacherAllocated(long id);

        /// <summary>
        /// check the teacher reg No alreadt taken or not
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        bool CheckRegNo(string regNo);

        /// <summary>
        /// Check the teacher display name already taken or not
        /// </summary>
        /// <param name="teacherDisplayName"></param>
        /// <returns></returns>
        bool CheckTeacherDisplayName(string teacherDisplayName);

        /// <summary>
        /// Check the teacher mail already taken or not
        /// </summary>
        /// <param name="eMail"></param>
        /// <returns></returns>
        bool CheckEmail(string eMail);

        /// <summary>
        /// Toggle the teacher status 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ToggleTeacher(long id, bool isEnable, out string msg);

        /// <summary>
        /// Save the teacher information
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveTeacher(TeacherBO teacher, out string msg);

        /// <summary>
        /// Search the teacher
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<TeacherBO> SearchTeacher(string term, string category);



    }

}
