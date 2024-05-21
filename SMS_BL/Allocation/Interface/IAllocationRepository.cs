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
        IEnumerable<SubjectAllocationGroupByTeacherViewModel> GetAllSubjectAllocation();

        SubjectAllocationBO GetSubjectAllocationByID(long id);

        bool DeleteSubjectAllocation(long id, out string msg);

        bool IsSubjectAllocated(long id);

        bool SaveSubjectAllocation(SubjectAllocationBO subjectAllocation, out string msg);

        IEnumerable<SubjectAllocationGroupByTeacherViewModel> SearchSubjectAllocation(string term, string category);

        IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> GetAllStudentAllocation(bool? isActive = null);

        StudentAllocationBO GetStudenttAllocationByID(long id);

        bool DeleteStudentAllocation(long id, out string msg);

        bool DeleteAllStudentAllocation(long id, out string msg);

        bool SaveStudentAllocation(StudentAllocationBO studentAllocation, out string msg);

        IEnumerable<object> GetAllocatedSubjects();

        IEnumerable<object> GetTeachersBySubject(long id);

        long GetSubjectAllocationID(long subjectID, long teacherID);

        IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> SearchStudentAllocation(string term, string category);

    }
}
