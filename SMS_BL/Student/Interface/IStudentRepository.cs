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
        /// <summary>
        /// Get All Student
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEnumerable<StudentBO> GetAllStudents(bool? isActive = null);

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentBO GetStudentByID(long id);

        /// <summary>
        /// Delete student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteStudent(long id, out string msg);

        /// <summary>
        /// check student is allocated or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsStudentAllocated(long id);

        /// <summary>
        /// check the student RegNo already taken or not
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        bool CheckRegNo(string regNo);

        /// <summary>
        /// check the student display name already taken or not
        /// </summary>
        /// <param name="studentDisplayName"></param>
        /// <returns></returns>
        bool CheckStudentDisplayName(string studentDisplayName);
        
        /// <summary>
        /// check the student email already taken or not
        /// </summary>
        /// <param name="eMail"></param>
        /// <returns></returns>
        bool CheckEmail(string eMail);

        /// <summary>
        /// toggle the status of the sudent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ToggleStudent(long id, bool isEnable, out string msg);

        /// <summary>
        /// Save the sudent details
        /// </summary>
        /// <param name="student"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveStudent(StudentBO student, out string msg);

        /// <summary>
        /// Update the student details
        /// </summary>
        /// <param name="student"></param>
        /// <param name="msg"></param>
        /// <param name="editStudent"></param>
        /// <returns></returns>
        bool UpdateStudentDetails(StudentBO student, out string msg, SMS_Data.Student editStudent);

        /// <summary>
        /// Search the student
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<StudentBO> SearchStudent(string term, string category);
    }
}
