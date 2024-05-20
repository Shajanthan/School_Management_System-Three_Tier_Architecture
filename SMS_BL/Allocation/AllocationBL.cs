using SMS_BL.Student;
using SMS_BL.Subject;
using SMS_Data;
using SMS_Models.Allocation;
using SMS_Models.Student;
using SMS_Models.Subject;
using SMS_ViewModels.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Allocation
{
    public class AllocationBL : SMS_DBEntities
    {

        //-------------------------------------------------------Subject Allocation--------------------------------------------------------------------

        ///// <summary>
        ///// Get All Subject Allocation
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<object> GetAllSubjectAllocation()
        //{
        //    var allSubjectAllocations = Teacher_Subject_Allocation.Include("Subject").Include("Teacher").ToList();


        //    if (allSubjectAllocations.Count > 0)
        //    {
        //        var data = allSubjectAllocations.Select(item => new
        //        {
        //            SubjectAllocationID = item.SubjectAllocationID,
        //            SubjectCode = item.Subject.SubjectCode,
        //            SubjectName = item.Subject.Name,
        //            TeacherRegNo = item.Teacher.TeacherRegNo,
        //            TeacherName = item.Teacher.DisplayName
        //        });

        //        return data;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Get AllSubjectAllocation
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SubjectAllocationGroupByTeacherViewModel> GetAllSubjectAllocation()
        {
            var allSubjectAllocations = Teacher_Subject_Allocation.Include("Subject").Include("Teacher").ToList();


            if (allSubjectAllocations.Count > 0)
            {
                var result = allSubjectAllocations.Select(item => new
                {
                    SubjectAllocationID = item.SubjectAllocationID,
                    SubjectCode = item.Subject.SubjectCode,
                    SubjectName = item.Subject.Name,
                    TeacherRegNo = item.Teacher.TeacherRegNo,
                    TeacherName = item.Teacher.DisplayName
                    

                }).GroupBy(x=>new { x.TeacherName,x.TeacherRegNo }).ToList();

                var data = result.Select(a => new SubjectAllocationGroupByTeacherViewModel
                {
                    TeacherName = a.Key.TeacherName,
                    TeacherRegNo = a.Key.TeacherRegNo,

                    SubjectAllocations = a.Select(b => new SubjectAllocationViewModel
                    {
                        SubjectAllocationID = b.SubjectAllocationID,
                        SubjectCode = b.SubjectCode,
                        SubjectName = b.SubjectName
                    }).ToList()

                });

                return data;
            }
            return null;
        }


        /// <summary>
        /// Get allocation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubjectAllocationBO GetSubjectAllocationByID(long id) {

            var student = Teacher_Subject_Allocation.Select(s => new SubjectAllocationBO()
            {
                SubjectAllocationID = s.SubjectAllocationID,
                SubjectID = s.SubjectID,
                TeacherID = s.TeacherID,
            }).Where(s => s.SubjectAllocationID == id).FirstOrDefault();
            return student;
        }

        /// <summary>
        /// Delete Subject allocation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteSubjectAllocation(long id, out string msg)
        {

            msg = "";

            var subjectAllocation = Teacher_Subject_Allocation.SingleOrDefault(s => s.SubjectAllocationID == id);

            try
            {

                if (subjectAllocation != null)
                {
                    bool studentAllocated = IsSubjectAllocated(id);

                    if (studentAllocated)
                    {
                        msg = "This Subject Allocation (" + subjectAllocation.Subject.Name + " - " + subjectAllocation.Teacher.DisplayName + ") is following by students.";
                        return false;
                    }

                    Teacher_Subject_Allocation.Remove(subjectAllocation);
                    SaveChanges();
                    return true;


                }
                msg = "Already removed";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// Check Subject is allocated or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsSubjectAllocated(long id) {

            bool isSubjectAllocated = Student_Subject_Teacher_Allocation.Any(s => s.SubjectAllocationID == id);
            return isSubjectAllocated;
        }


        /// <summary>
        /// Save or edit subject allocation
        /// </summary>
        /// <param name="subjectAllocation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveSubjectAllocation(SubjectAllocationBO subjectAllocation, out string msg)
        {
            msg = "";

            bool isExistingSubjectAllocation = Teacher_Subject_Allocation.Any(s => s.SubjectAllocationID == subjectAllocation.SubjectAllocationID);

            var editSubjectAllocation = Teacher_Subject_Allocation.SingleOrDefault(s => s.SubjectAllocationID == subjectAllocation.SubjectAllocationID);

            bool isStudentChoose = IsSubjectAllocated(subjectAllocation.SubjectAllocationID);

            bool isAllocated = Teacher_Subject_Allocation.Any(a => a.TeacherID == subjectAllocation.TeacherID && a.SubjectID == subjectAllocation.SubjectID);

            try
            {
                if (isAllocated)
                {
                    msg = "This Allocation Already Exists.";
                    return false;
                }

                if (isExistingSubjectAllocation)
                {
                    if (isStudentChoose)
                    {

                        msg = "The Subject allocation is currently following by students.";
                        return false;

                    }

                    if (editSubjectAllocation == null)
                    {
                        msg = "Unable to find the subject allocation for edit";
                        return false;
                    }

                    editSubjectAllocation.SubjectID = subjectAllocation.SubjectID;

                    SaveChanges();
                    msg = "Allocation Details Updated Successfully!";
                    return true;

                }

                var newSubjectAllocation = new SMS_Data.Teacher_Subject_Allocation();
                newSubjectAllocation.TeacherID = subjectAllocation.TeacherID;
                newSubjectAllocation.SubjectID = subjectAllocation.SubjectID;



                Teacher_Subject_Allocation.Add(newSubjectAllocation);
                SaveChanges();
                msg = "Subject Allocation Added Successfully!";
                return true;
            }
            catch (Exception error)
            {
                msg = error.Message;
                return false;
            }

        }

        /// <summary>
        /// Search Subject allocation
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<SubjectAllocationGroupByTeacherViewModel> SearchSubjectAllocation(string term, string category)
        {
            var allSubjects = GetAllSubjectAllocation();
           
            if (category == "TeacherName")
            {
                allSubjects = allSubjects.Where(s => s.TeacherName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "SubjectName")
            {
                allSubjects = allSubjects.Where(s => s.SubjectAllocations.Any(t => t.SubjectName.ToUpper().Contains(term.ToUpper()))).ToList();
            }
            else
            {
                allSubjects = allSubjects.Where(s => s.TeacherName.ToUpper().Contains(term.ToUpper()) || s.SubjectAllocations.Any(t => t.SubjectName.ToUpper().Contains(term.ToUpper()))).ToList();
            }

            return allSubjects;
        }



        //-------------------------------------------------------Student Allocation--------------------------------------------------------------------

        /// <summary>
        /// Get all Student allocations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> GetAllStudentAllocation(bool? isActive = null)
        {

            var allStudentAllocations = Student_Subject_Teacher_Allocation.Include("Teacher_Subject_Allocation.Subject")
                .Include("Teacher_Subject_Allocation.Teacher")
                .Include("Student").ToList();




            if (allStudentAllocations.Count > 0)
            {
                var resultStudent = allStudentAllocations.Select(item => new
                {
                    StudentAllocationID = item.StudentAllocationID,
                    StudentID = item.StudentID,
                    StudentName = item.Student.DisplayName,
                    StudentRegNo = item.Student.StudentRegNo,
                    SubjectID = item.Teacher_Subject_Allocation.SubjectID,
                    SubjectCode = item.Teacher_Subject_Allocation.Subject.SubjectCode,
                    SubjectName = item.Teacher_Subject_Allocation.Subject.Name,
                    TeacherID = item.Teacher_Subject_Allocation.TeacherID,
                    TeacherRegNo = item.Teacher_Subject_Allocation.Teacher.TeacherRegNo,
                    TeacherName = item.Teacher_Subject_Allocation.Teacher.DisplayName,
                    IsStudentEnable=item.Student.IsEnable

                }).GroupBy(x => new { x.StudentName, x.StudentRegNo,x.StudentID,x.IsStudentEnable }).ToList();


                var result = resultStudent.Select(s => new StudentSubjectAllocationGroupByStudentViewModel
                {
                    StudentName = s.Key.StudentName,
                    StudentRegNo = s.Key.StudentRegNo,        
                    StudentID = s.Key.StudentID,
                    IsStudentEnable=s.Key.IsStudentEnable,
                    TeacherAllocation = s.GroupBy(x => new { x.TeacherName, x.TeacherRegNo })
                    .Select(y => new SubjectAllocationGroupByTeacherViewModel
                    {
                        TeacherName = y.Key.TeacherName,
                        TeacherRegNo = y.Key.TeacherRegNo,
                        SubjectAllocations = y.Select(subject => new SubjectAllocationViewModel
                        {
                            StudentAllocationID = subject.StudentAllocationID,
                            SubjectCode = subject.SubjectCode,
                            SubjectName = subject.SubjectName,
                            TeacherRegNo = subject.TeacherRegNo
                        })
                    }).ToList()
                });

                if (isActive.HasValue)
                {
                    result = result.Where(s => s.IsStudentEnable == isActive.Value);
                }

                return result;

            }
            return null;
        }

        /// <summary>
        /// Get the specific student allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentAllocationBO GetStudenttAllocationByID(long id)
        {

            var student = Student_Subject_Teacher_Allocation.Select(s => new StudentAllocationBO()
            {
                SubjectAllocationID = s.SubjectAllocationID,
                StudentAllocationID = s.StudentAllocationID,
                StudentID = s.StudentID,
            }).Where(s => s.StudentAllocationID == id).FirstOrDefault();
            return student;
        }


        /// <summary>
        /// Delete Student allocation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteStudentAllocation(long id, out string msg)
        {

            msg = "";

            var studentAllocation = Student_Subject_Teacher_Allocation.SingleOrDefault(s => s.StudentAllocationID == id);

            try
            {

                if (studentAllocation != null)
                {
                    Student_Subject_Teacher_Allocation.Remove(studentAllocation);
                    SaveChanges();
                    return true;

                }
                msg = "Already removed";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }
        /// <summary>
        /// Delete all the allocation for the student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteAllStudentAllocation(long id, out string msg)
        {

            msg = "";

            var studentAllocation = Student_Subject_Teacher_Allocation.Where(s => s.StudentID == id).ToList();

            try
            {

                if (studentAllocation != null)
                {
                    Student_Subject_Teacher_Allocation.RemoveRange(studentAllocation);
                    SaveChanges();
                    return true;

                }
                msg = "Already removed";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }


        /// <summary>
        /// Add or edit the student allocation
        /// </summary>
        /// <param name="studentAllocation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveStudentAllocation(StudentAllocationBO studentAllocation, out string msg)
        {
            msg = "";

            bool isExistingStudentAllocation = Student_Subject_Teacher_Allocation.Any(s => s.StudentAllocationID == studentAllocation.StudentAllocationID);

            var editStudenttAllocation = Student_Subject_Teacher_Allocation.SingleOrDefault(s => s.StudentAllocationID == studentAllocation.StudentAllocationID);

            bool isStudentAllocated = Student_Subject_Teacher_Allocation.Any(s => s.SubjectAllocationID == studentAllocation.SubjectAllocationID && s.StudentID == studentAllocation.StudentID);

           

            try
            {
                if (studentAllocation.SubjectAllocationID == 0 || studentAllocation==null) {
                    msg = "Please Fill All Details";
                    return false;
                }

                if (isStudentAllocated)
                {
                    msg = "This Allocation Already Exists.";
                    return false;
                }

                if (isExistingStudentAllocation)
                {
                    

                    if (editStudenttAllocation == null)
                    {
                        msg = "Unable to find the subject allocation for edit";
                        return false;
                    }

                    editStudenttAllocation.SubjectAllocationID = studentAllocation.SubjectAllocationID;

                    SaveChanges();
                    msg = "Allocation Details Updated Successfully!";
                    return true;

                }
                

                var newStudentAllocation = new SMS_Data.Student_Subject_Teacher_Allocation();
                newStudentAllocation.StudentID = studentAllocation.StudentID;
                newStudentAllocation.SubjectAllocationID = studentAllocation.SubjectAllocationID;



                Student_Subject_Teacher_Allocation.Add(newStudentAllocation);
                SaveChanges();
                msg = "Student Allocation Added Successfully!";
                return true;
            }
            catch (Exception error)
            {
                msg = error.Message;
                return false;
            }

        }

        /// <summary>
        /// Get Allocated subject
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAllocatedSubjects() { 
            
            var allocatedSubject=Teacher_Subject_Allocation.Select(a => new { SubjectID = a.SubjectID, Name = a.Subject.SubjectCode + " - " + a.Subject.Name }).Distinct();
            return allocatedSubject;
        }

        /// <summary>
        /// get the teacher for the subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<object> GetTeachersBySubject(long id) {
            var teachers = Teacher_Subject_Allocation.Where(t => t.SubjectID == id).Select(t => new {Value = t.SubjectAllocationID,Text = t.Teacher.DisplayName}).ToList();
            return teachers;
        }

        /// <summary>
        /// Get the subject allocation id by passing the subject id and teacher id
        /// </summary>
        /// <param name="subjectID"></param>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        public long GetSubjectAllocationID(long subjectID,long teacherID) {
            var allocationID = Teacher_Subject_Allocation
                                .Where(s => s.SubjectID == subjectID && s.TeacherID == teacherID)
                                .Select(s => s.SubjectAllocationID)
                                .FirstOrDefault(); 

            return allocationID;
        }

        public IEnumerable<StudentSubjectAllocationGroupByStudentViewModel> SearchStudentAllocation(string term, string category)
        {
            var allStudents = GetAllStudentAllocation();

            if (category == "StudentName")
            {
                allStudents = allStudents.Where(s => s.StudentName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "TeacherName")
            {
                allStudents = allStudents.Where(s => s.TeacherAllocation.Any(t => t.TeacherName.ToUpper().Contains(term.ToUpper()))).ToList();
            }
            else if (category == "SubjectName")
            {
                allStudents = allStudents.Where(s => s.TeacherAllocation.Any(t => t.SubjectAllocations.Any(sub => sub.SubjectName.ToUpper().Contains(term.ToUpper())))).ToList();
            }
            else
            {
                allStudents = allStudents.Where(s => s.StudentName.ToUpper().Contains(term.ToUpper()) || s.TeacherAllocation.Any(t => t.TeacherName.ToUpper().Contains(term.ToUpper())) || s.TeacherAllocation.Any(t => t.SubjectAllocations.Any(sub => sub.SubjectName.ToUpper().Contains(term.ToUpper())))).ToList();
            }

            return allStudents;
        }



    }
}
