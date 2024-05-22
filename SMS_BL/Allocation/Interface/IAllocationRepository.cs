using SMS_Models.Allocation;
using SMS_ViewModels.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Allocation.Interface
{
    public interface IAllocationRepository
    {
        /// <summary>
        /// Get all subject allocation
        /// </summary>
        /// <returns></returns>
        IEnumerable<SubjectAllocationGroupByTeacherViewModel> GetAllSubjectAllocation();

        /// <summary>
        /// Get subject Allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubjectAllocationBO GetSubjectAllocationByID(long id);

        /// <summary>
        /// Delete subject allocation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteSubjectAllocation(long id, out string msg);

        /// <summary>
        /// check the subject already allocated or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsSubjectAllocated(long id);

        /// <summary>
        /// Save the subject allocation
        /// </summary>
        /// <param name="subjectAllocation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveSubjectAllocation(SubjectAllocationBO subjectAllocation, out string msg);

        /// <summary>
        /// Search the subject allocation
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<SubjectAllocationGroupByTeacherViewModel> SearchSubjectAllocation(string term, string category);

        /// <summary>
        /// Get all student allocation
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> GetAllStudentAllocation(bool? isActive = null);

        /// <summary>
        /// Get the student allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentAllocationBO GetStudenttAllocationByID(long id);

        /// <summary>
        /// Delete the student allocation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteStudentAllocation(long id, out string msg);

        /// <summary>
        /// Delete the all the allocation for the specific student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteAllStudentAllocation(long id, out string msg);

        /// <summary>
        /// Save the student allocation
        /// </summary>
        /// <param name="studentAllocation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveStudentAllocation(StudentAllocationBO studentAllocation, out string msg);

        /// <summary>
        /// Get the allocation subject
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAllocatedSubjects();

        /// <summary>
        /// Get the teacher for the subjects
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<object> GetTeachersBySubject(long id);

        /// <summary>
        /// Get the subject allocation id
        /// </summary>
        /// <param name="subjectID"></param>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        long GetSubjectAllocationID(long subjectID, long teacherID);

        /// <summary>
        /// Search the allocation for the student
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> SearchStudentAllocation(string term, string category);

    }
}
