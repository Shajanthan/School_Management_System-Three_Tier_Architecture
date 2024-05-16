/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using SMS_Data;
using SMS_Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMS_BL.Teacher
{
    public class TeacherBL : SMS_DBEntities
    {
        /// <summary>
        /// Get all teachers as a list based on active or not
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public IEnumerable<TeacherBO> GetAllTeachers(bool? isActive = null)
        {
            var allTeachers = Teachers.Select(s => new TeacherBO()
            {
                TeacherID = s.TeacherID,
                TeacherRegNo = s.TeacherRegNo,
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
                allTeachers = allTeachers.Where(s => s.IsEnable == isActive.Value);
            }


            return allTeachers;
        }
        /// <summary>
        /// Get Teacher By passing teacher id as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherBO GetTeacherByID(long id)
        {
            var teacher = Teachers.Select(s => new TeacherBO()
            {
                TeacherID = s.TeacherID,
                TeacherRegNo = s.TeacherRegNo,
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
            }).Where(s => s.TeacherID == id).FirstOrDefault();
            return teacher;
        }

        /// <summary>
        /// Delete teacher when teacher is not allocated to subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteTeacher(long id, out string msg)
        {

            msg = "";

            var teacher = Teachers.SingleOrDefault(s => s.TeacherID == id);

            try
            {

                if (teacher != null)
                {

                    bool teacherAvailable = IsTeacherAllocated(id);

                    if (teacherAvailable)
                    {
                        msg = "Teacher " + teacher.DisplayName + " is allocated to subject.";
                        return false;
                    }

                    Teachers.Remove(teacher);
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
        /// Checking teacher is allocated to subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsTeacherAllocated(long id)
        {
            bool isTeacherAllocated = Teacher_Subject_Allocation.Any(s => s.TeacherID == id);
            return isTeacherAllocated;
        }


        /// <summary>
        /// Check teacher regNo 
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public bool CheckRegNo(string regNo)
        {

            bool isRegNo = Teachers.Any(s => s.TeacherRegNo == regNo);
            if (isRegNo)
            {
                return false;

            }
            return true;
        }

        /// <summary>
        /// Check teacher display name available
        /// </summary>
        /// <param name="teacherDisplayName"></param>
        /// <returns></returns>
        public bool CheckTeacherDisplayName(string teacherDisplayName)
        {

            bool isTeachDisplayName = Teachers.Any(s => s.DisplayName == teacherDisplayName);
            if (isTeachDisplayName)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check Email available
        /// </summary>
        /// <param name="eMail"></param>
        /// <returns></returns>
        public bool CheckEmail(string eMail)
        {

            bool isEmail = Teachers.Any(s => s.Email == eMail);
            if (isEmail)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// toggle teacher availability active or not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ToggleTeacher(long id, bool isEnable, out string msg)
        {

            msg = "";
            var teacher = Teachers.SingleOrDefault(s => s.TeacherID == id);
            if (teacher == null)
            {
                msg = "No teacher found";
                return false;
            }
            if (!isEnable)
            {

                if (IsTeacherAllocated(id))
                {
                    msg = "Teacher " + teacher.DisplayName + " is currently assign to subject";
                    return false;
                }
                teacher.IsEnable = false;
                SaveChanges();
                msg = "Teacher " + teacher.DisplayName + " status is changed to disbled";
                return true;
            }

            teacher.IsEnable = true;
            SaveChanges();
            msg = "Teacher " + teacher.DisplayName + " status is changed to enable";
            return true;

        }

        /// <summary>
        /// Save new teacher or update the teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveTeacher(TeacherBO teacher, out string msg)
        {
            msg = "";

            bool existingTeacher = Teachers.Any(s => s.TeacherID == teacher.TeacherID);

            bool isTeacherAllocated = Teacher_Subject_Allocation.Any(a => a.TeacherID == teacher.TeacherID);

            bool isDisplayNameAvailable = CheckTeacherDisplayName(teacher.DisplayName);
            bool isRegNoAvailable = CheckRegNo(teacher.TeacherRegNo);
            bool isEmailAvailable = CheckEmail(teacher.Email);
           

            try
            {
                if (existingTeacher)
                {
                    if (isTeacherAllocated)
                    {
                        msg = "The Teacher " + teacher.DisplayName + " is teaching subjects to students.";
                        return false;
                    }

                    var editTeacher = Teachers.SingleOrDefault(s => s.TeacherID == teacher.TeacherID);

                    if (editTeacher == null)
                    {
                        msg = "Unable to find the teacher for edit";
                        return false;
                    }

                    isDisplayNameAvailable = Teachers.Any(s => s.DisplayName == teacher.DisplayName && s.TeacherID != teacher.TeacherID);
                    isRegNoAvailable = Teachers.Any(s => s.TeacherRegNo == teacher.TeacherRegNo && s.TeacherID != teacher.TeacherID);
                    isEmailAvailable = Teachers.Any(s => s.Email == teacher.Email && s.TeacherID != teacher.TeacherID);

                    if (isRegNoAvailable)
                    {
                        msg = "Teacher RegNo " + teacher.TeacherRegNo + " already exists.";
                        return false;
                    }

                    if (isDisplayNameAvailable)
                    {
                        msg = "Teacher Display name  " + teacher.DisplayName + " already exists.";
                        return false;
                    }

                    if (isEmailAvailable)
                    {
                        msg = "Teacher E-mail " + teacher.Email + " already exists.";
                        return false;
                    }

                    editTeacher.TeacherRegNo = teacher.TeacherRegNo;
                    editTeacher.FirstName = teacher.FirstName;
                    editTeacher.MiddleName = teacher.MiddleName;
                    editTeacher.LastName = teacher.LastName;
                    editTeacher.DisplayName = teacher.DisplayName;
                    editTeacher.Email = teacher.Email;
                    editTeacher.Gender = teacher.Gender;
                    editTeacher.DOB = teacher.DOB;
                    editTeacher.Address = teacher.Address;
                    editTeacher.ContactNo = teacher.ContactNo;
                    editTeacher.IsEnable = teacher.IsEnable;

                    SaveChanges();
                    msg = "Teacher Details Updated Successfully!";
                    return true;
                }

                if (!isRegNoAvailable)
                {
                    msg = "Teacher RegNo " + teacher.TeacherRegNo + " already exists.";
                    return false;
                }

                if (!isDisplayNameAvailable)
                {
                    msg = "Teacher Display name  " + teacher.DisplayName + " already exists.";
                    return false;
                }

                if (!isEmailAvailable)
                {
                    msg = "Teacher E-mail " + teacher.Email + " already exists.";
                    return false;
                }

                var newTeacher = new SMS_Data.Teacher();
                newTeacher.TeacherRegNo = teacher.TeacherRegNo;
                newTeacher.FirstName = teacher.FirstName;
                newTeacher.MiddleName = teacher.MiddleName;
                newTeacher.LastName = teacher.LastName;
                newTeacher.DisplayName = teacher.DisplayName;
                newTeacher.Email = teacher.Email;
                newTeacher.Gender = teacher.Gender;
                newTeacher.DOB = teacher.DOB;
                newTeacher.Address = teacher.Address;
                newTeacher.ContactNo = teacher.ContactNo;
                newTeacher.IsEnable = teacher.IsEnable;


                Teachers.Add(newTeacher);
                SaveChanges();
                msg = "Teacher Added Successfully!";
                return true;
            }
            catch (Exception error)
            {
                msg = error.Message;
                return false;
            }

        }

        /// <summary>
        /// Search teacher by selected criteria/category
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<TeacherBO> SearchTeacher(string term, string category)
        {
            var allTeachers = GetAllTeachers();

            if (category == "TeacherRegNo")
            {
                allTeachers = allTeachers.Where(s => s.TeacherRegNo.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "FirstName")
            {
                allTeachers = allTeachers.Where(s => s.FirstName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "LastName")
            {
                allTeachers = allTeachers.Where(s => s.LastName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "DisplayName")
            {
                allTeachers = allTeachers.Where(s => s.DisplayName.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else
            {
                allTeachers = allTeachers.Where(s => s.TeacherRegNo.ToUpper().Contains(term.ToUpper()) || s.FirstName.ToUpper().Contains(term.ToUpper()) || s.LastName.ToUpper().Contains(term.ToUpper()) || s.DisplayName.ToUpper().Contains(term.ToUpper())).ToList();
            }

            return allTeachers;
        }


    }
}
