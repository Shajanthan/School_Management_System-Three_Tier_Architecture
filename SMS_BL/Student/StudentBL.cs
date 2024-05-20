/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using SMS_Data;
using SMS_Models.Student;
using SMS_Models.Subject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMS_BL.Student
{
    public class StudentBL:SMS_DBEntities
    {
       
        /// <summary>
        /// Get all the Students details
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public IEnumerable<StudentBO> GetAllStudents(bool? isActive = null)
        {
            var allStudents = Students.Select(s => new StudentBO()
            {
                StudentID = s.StudentID,
                StudentRegNo = s.StudentRegNo,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                DisplayName = s.DisplayName,
                Email = s.Email,
                Gender = s.Gender,
                DOB = s.DOB,
                Address = s.Address,
                ContactNo = s.ContactNo,
                IsEnable = s.IsEnable,

            });

            if (isActive.HasValue)
            {
                allStudents = allStudents.Where(s => s.IsEnable == isActive.Value);
            }


            return allStudents;
        }

        /// <summary>
        /// Get a student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentBO GetStudentByID(long id)
        {
            var student = Students.Select(s => new StudentBO()
            {
                StudentID = s.StudentID,
                StudentRegNo = s.StudentRegNo,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                DisplayName = s.DisplayName,
                Email = s.Email,
                Gender = s.Gender,
                DOB = s.DOB,
                Address = s.Address,
                ContactNo = s.ContactNo,
                IsEnable = s.IsEnable,
            }).Where(s => s.StudentID == id).FirstOrDefault();
            return student;
        }

        /// <summary>
        /// Delete student by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteStudent(long id, out string msg)
        {

            msg = "";

            var student = Students.SingleOrDefault(s => s.StudentID == id);

            try
            {

                if (student != null)
                {
                    bool studentAllocated = IsStudentAllocated(id);

                    if (student.IsEnable == true)
                    {

                        if (studentAllocated)
                        {
                            msg = "Student " + student.DisplayName + " is following a subject.";
                            return false;
                        }

                        Students.Remove(student);
                        SaveChanges();
                        return true;

                    }

                    var studentAllocation = Student_Subject_Teacher_Allocation.Where(s => s.StudentID == id).ToList();

                    Student_Subject_Teacher_Allocation.RemoveRange(studentAllocation);
                    SaveChanges();
                    Students.Remove(student);
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
        /// check if the student follow any subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsStudentAllocated(long id)
        {
            bool isStudentAllocated = Student_Subject_Teacher_Allocation.Any(s => s.StudentID == id);
            return isStudentAllocated;
        }

       /// <summary>
       /// check regNo already taken or not
       /// </summary>
       /// <param name="regNo"></param>
       /// <returns></returns>
        public bool CheckRegNo(string regNo)
        {

            bool isRegNo = Students.Any(s => s.StudentRegNo == regNo);
            if (isRegNo)
            {
                return false;

            }
            return true;
        }

       /// <summary>
       /// check Student display name is already taken or not
       /// </summary>
       /// <param name="studentDisplayName"></param>
       /// <returns></returns>
        public bool CheckStudentDisplayName(string studentDisplayName)
        {

            bool isStudentDisplayName = Students.Any(s => s.DisplayName == studentDisplayName);
            if (isStudentDisplayName)
            {
                return false;
            }
            return true;
        }

       /// <summary>
       /// check student email
       /// </summary>
       /// <param name="eMail"></param>
       /// <returns></returns>
        public bool CheckEmail(string eMail)
        {

            bool isEmail = Students.Any(s => s.Email == eMail);
            if (isEmail)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Toggle student status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ToggleStudent(long id, bool isEnable, out string msg)
        {

            msg = "";
            var student = Students.SingleOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                msg = "No Student found";
                return false;
            }
            if (!isEnable)
            {
                //if (IsStudentAllocated(id))
                //{
                //    var allocationsToRemove = Student_Subject_Teacher_Allocation.Where(a => a.StudentID == id).ToList();

                //    Student_Subject_Teacher_Allocation.RemoveRange(allocationsToRemove);

                //    SaveChanges();
                //}

                student.IsEnable = false;
                SaveChanges();
                msg = "Student " + student.DisplayName + " status is changed to disbled";
                return true;
            }

            student.IsEnable = true;
            SaveChanges();
            msg = "Student " + student.DisplayName + " status is changed to enable";
            return true;

        }

        /// <summary>
        /// Save / Edit student details
        /// </summary>
        /// <param name="student"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveStudent(StudentBO student, out string msg)
        {
            msg = "";

            bool existingStudent = Students.Any(s => s.StudentID == student.StudentID);

            var editStudent = Students.SingleOrDefault(s => s.StudentID == student.StudentID);

            bool isStudentAllocated = IsStudentAllocated(student.StudentID);

            bool isDisplayNameAvailable = CheckStudentDisplayName(student.DisplayName);
            bool isRegNoAvailable = CheckRegNo(student.StudentRegNo);
            bool isEmailAvailable = CheckEmail(student.Email);


            try
            {
                if (existingStudent)
                {
                    if (isStudentAllocated)
                    {
                        if (student.IsEnable == editStudent.IsEnable)
                        {

                            msg = "The Student " + student.DisplayName + " is currently following subjects.";
                            return false;

                        }

                        SaveChanges();
                        return UpdateStudentDetails(student, out msg, editStudent);

                    }



                    if (editStudent == null)
                    {
                        msg = "Unable to find the teacher for edit";
                        return false;
                    }

                    isDisplayNameAvailable = Students.Any(s => s.DisplayName == student.DisplayName && s.StudentID != student.StudentID);
                    isRegNoAvailable = Students.Any(s => s.StudentRegNo == student.StudentRegNo && s.StudentID != student.StudentID);
                    isEmailAvailable = Students.Any(s => s.Email == student.Email && s.StudentID != student.StudentID);

                    if (isRegNoAvailable)
                    {
                        msg = "Student RegNo " + student.StudentRegNo + " already exists.";
                        return false;
                    }

                    if (isDisplayNameAvailable)
                    {
                        msg = "Student Display name  " + student.DisplayName + " already exists.";
                        return false;
                    }

                    if (isEmailAvailable)
                    {
                        msg = "Student E-mail " + student.Email + " already exists.";
                        return false;
                    }

                    return UpdateStudentDetails(student, out msg, editStudent);
                }

                if (!isRegNoAvailable)
                {
                    msg = "Student RegNo " + student.StudentRegNo + " already exists.";
                    return false;
                }

                if (!isDisplayNameAvailable)
                {
                    msg = "Student Display name  " + student.DisplayName + " already exists.";
                    return false;
                }

                if (!isEmailAvailable)
                {
                    msg = "Student E-mail " + student.Email + " already exists.";
                    return false;
                }

                var newStudent = new SMS_Data.Student();
                newStudent.StudentRegNo = student.StudentRegNo;
                newStudent.FirstName = student.FirstName;
                newStudent.MiddleName = student.MiddleName;
                newStudent.LastName = student.LastName;
                newStudent.DisplayName = student.DisplayName;
                newStudent.Email = student.Email;
                newStudent.Gender = student.Gender;
                newStudent.DOB = student.DOB;
                newStudent.Address = student.Address;
                newStudent.ContactNo = student.ContactNo;
                newStudent.IsEnable = student.IsEnable;


                Students.Add(newStudent);
                SaveChanges();
                msg = "Student Added Successfully!";
                return true;
            }
            catch (Exception error)
            {
                msg = error.Message;
                return false;
            }

        }

        /// <summary>
        /// Update student details
        /// </summary>
        /// <param name="student"></param>
        /// <param name="msg"></param>
        /// <param name="editStudent"></param>
        /// <returns></returns>
        private bool UpdateStudentDetails(StudentBO student, out string msg, SMS_Data.Student editStudent)
        {
            editStudent.StudentRegNo = student.StudentRegNo;
            editStudent.FirstName = student.FirstName;
            editStudent.MiddleName = student.MiddleName;
            editStudent.LastName = student.LastName;
            editStudent.DisplayName = student.DisplayName;
            editStudent.Email = student.Email;
            editStudent.Gender = student.Gender;
            editStudent.DOB = student.DOB;
            editStudent.Address = student.Address;
            editStudent.ContactNo = student.ContactNo;
            editStudent.IsEnable = student.IsEnable;

            SaveChanges();
            msg = "Student Details Updated Successfully!";
            return true;
        }

        /// <summary>
        /// search student by giving criteria and term
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<StudentBO> SearchStudent(string term, string category)
        {
            var allStudents = GetAllStudents();

            if (category == "TeacherRegNo")
            {
                allStudents = allStudents.Where(s => s.StudentRegNo.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "FirstName")
            {
                allStudents = allStudents.Where(s => s.FirstName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "LastName")
            {
                allStudents = allStudents.Where(s => s.LastName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "DisplayName")
            {
                allStudents = allStudents.Where(s => s.DisplayName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else
            {
                allStudents = allStudents.Where(s => s.StudentRegNo.ToUpper().Contains(term.ToUpper()) || s.FirstName.ToUpper().Contains(term.ToUpper()) || s.LastName.ToUpper().Contains(term.ToUpper()) || s.DisplayName.ToUpper().Contains(term.ToUpper())).ToList();
            }

            return allStudents;
        }


    }
}
