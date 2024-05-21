using SMS_BL.Subject.Interface;
using SMS_Data;
using SMS_Models.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Subject
{
    public class SubjectRepository : ISubjectRepository
    {

        private readonly SMS_DBEntities _dbEntities;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbEntities"></param>
        public SubjectRepository(SMS_DBEntities dbEntities)
        {
            _dbEntities=dbEntities;
        }

        /// <summary>
        /// Check Subject code is taken or not
        /// </summary>
        /// <param name="subCode"></param>
        /// <returns></returns>
        public bool CheckSubCode(string subCode)
        {
            bool isSubjectCode = _dbEntities.Subjects.Any(s => s.SubjectCode == subCode);
            if (isSubjectCode)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// check subject name is taken or not
        /// </summary>
        /// <param name="subName"></param>
        /// <returns></returns>
        public bool CheckSubname(string subName)
        {
            bool isSubjectName = _dbEntities.Subjects.Any(s => s.Name == subName);
            if (isSubjectName)
            {
                return false;

            }
            return true;
        }

        /// <summary>
        /// delete the subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteSubject(long id, out string msg)
        {
            msg = "";

            var subject = _dbEntities.Subjects.SingleOrDefault(s => s.SubjectID == id);

            try
            {

                if (subject != null)
                {

                    bool isSubjectUse = IsSubjectInUse(id);

                    if (isSubjectUse)
                    {
                        msg = "Subject " + subject.Name + " is in Use.";
                        return false;
                    }

                    _dbEntities.Subjects.Remove(subject);
                    _dbEntities.SaveChanges();
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
        /// Get all the subject details
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public IEnumerable<SubjectBO> GetAllSubject(bool? isActive = null)
        {
            var allSubjects = _dbEntities.Subjects.Select(s => new SubjectBO()
            {
                SubjectID = s.SubjectID,
                SubjectCode = s.SubjectCode,
                Name = s.Name,
                IsEnable = s.IsEnable

            });

            if (isActive.HasValue)
            {
                allSubjects = allSubjects.Where(s => s.IsEnable == isActive.Value);
            }


            return allSubjects;
        }

        /// <summary>
        /// Get Subject by passing the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubjectBO GetSubjetByID(long id)
        {
            var subject = _dbEntities.Subjects.Select(s => new SubjectBO()
            {
                SubjectID = s.SubjectID,
                SubjectCode = s.SubjectCode,
                Name = s.Name,
                IsEnable = s.IsEnable
            }).Where(s => s.SubjectID == id).FirstOrDefault();
            return subject;
        }

        /// <summary>
        /// check the subject is use or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsSubjectInUse(long id)
        {
            bool isSubjectUse = _dbEntities.Teacher_Subject_Allocation.Any(s => s.SubjectID == id);
            return isSubjectUse;
        }

        /// <summary>
        /// Save the subject details
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SaveSubject(SubjectBO subject, out string msg)
        {
            msg = "";

            bool existingSubject = _dbEntities.Subjects.Any(s => s.SubjectID == subject.SubjectID);

            bool SubjectInUse = _dbEntities.Teacher_Subject_Allocation.Any(a => a.SubjectID == subject.SubjectID);

            bool isSubCodeAvailable = CheckSubCode(subject.SubjectCode);
            bool isSubNameAvailable = CheckSubname(subject.Name);

            try
            {
                if (existingSubject)
                {
                    if (SubjectInUse)
                    {
                        msg = "The Subject " + subject.Name + " is Followed by a sudent";
                        return false;
                    }

                    var editSubjet = _dbEntities.Subjects.SingleOrDefault(s => s.SubjectID == subject.SubjectID);

                    if (editSubjet == null)
                    {
                        msg = "Unable to find the subject for edit";
                        return false;
                    }

                    bool isSubjectName = _dbEntities.Subjects.Any(s => s.Name == subject.Name && s.SubjectID != subject.SubjectID);
                    bool isSubjectCode = _dbEntities.Subjects.Any(s => s.SubjectCode == subject.SubjectCode && s.SubjectID != subject.SubjectID);


                    if (isSubjectCode)
                    {
                        msg = "Subject code " + subject.SubjectCode + " already exists.";
                        return false;
                    }

                    if (isSubjectName)
                    {
                        msg = "Subject Name " + subject.Name + " already exists.";
                        return false;
                    }

                    editSubjet.SubjectCode = subject.SubjectCode;
                    editSubjet.Name = subject.Name;
                    editSubjet.IsEnable = subject.IsEnable;
                    _dbEntities.SaveChanges();
                    msg = "Subject Updated Successfully!";
                    return true;

                }

                if (!isSubCodeAvailable)
                {
                    msg = "Subject code " + subject.SubjectCode + " already exists.";
                    return false;
                }

                if (!isSubNameAvailable)
                {
                    msg = "Subject Name " + subject.Name + " already exists.";
                    return false;
                }

                var newSubject = new SMS_Data.Subject();
                newSubject.SubjectCode = subject.SubjectCode;
                newSubject.Name = subject.Name;
                newSubject.IsEnable = subject.IsEnable;
                _dbEntities.Subjects.Add(newSubject);
                _dbEntities.SaveChanges();
                msg = "Subject Added Successfully!";
                return true;
            }
            catch (Exception error)
            {
                msg = error.Message;
                return false;
            }
        }

        /// <summary>
        /// Search the subject by code
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<SubjectBO> SearchSubject(string term, string category)
        {
            var allSubjects = GetAllSubject();

            if (category == "SubjectCode")
            {
                allSubjects = allSubjects.Where(s => s.SubjectCode.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else if (category == "Name")
            {
                allSubjects = allSubjects.Where(s => s.Name.ToUpper().Contains(term.ToUpper())).ToList();
            }
            else
            {
                allSubjects = allSubjects.Where(s => s.SubjectCode.ToUpper().Contains(term.ToUpper()) || s.Name.ToUpper().Contains(term.ToUpper())).ToList();
            }

            return allSubjects;
        }

        /// <summary>
        /// Toggle the status of the subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ToggleSubject(long id, bool isEnable, out string msg)
        {
            msg = "";
            var subject = _dbEntities.Subjects.SingleOrDefault(s => s.SubjectID == id);
            if (subject == null)
            {
                msg = "No subject found";
                return false;
            }
            if (!isEnable)
            {

                if (IsSubjectInUse(id))
                {
                    msg = "Subject " + subject.Name + " is currently in use";
                    return false;
                }
                subject.IsEnable = false;
                _dbEntities.SaveChanges();
                msg = "Subject " + subject.Name + " Disabled";
                return true;
            }

            subject.IsEnable = true;
            _dbEntities.SaveChanges();
            msg = "Subject " + subject.Name + " Enabled";
            return true;
        }
    }

}
