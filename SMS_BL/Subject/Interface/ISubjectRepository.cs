using SMS_Models.Student;
using SMS_Models.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_BL.Subject.Interface
{
    public interface ISubjectRepository
    {
        /// <summary>
        /// get all subject
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEnumerable<SubjectBO> GetAllSubject(bool? isActive = null);

        /// <summary>
        /// get subject by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubjectBO GetSubjetByID(long id);

        /// <summary>
        /// interface of save subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SaveSubject(SubjectBO subject, out string msg);

        /// <summary>
        /// delete subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DeleteSubject(long id, out string msg);

        /// <summary>
        /// check the subject is in use or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsSubjectInUse(long id);

        /// <summary>
        /// check the subject name is available or not
        /// </summary>
        /// <param name="subName"></param>
        /// <returns></returns>
        bool CheckSubname(string subName);

        /// <summary>
        /// check the subject code is already exist
        /// </summary>
        /// <param name="subCode"></param>
        /// <returns></returns>
        bool CheckSubCode(string subCode);

        /// <summary>
        /// toggle the status of the subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ToggleSubject(long id, bool isEnable, out string msg);

        /// <summary>
        /// Search subject by search item and category
        /// </summary>
        /// <param name="term"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<SubjectBO> SearchSubject(string term, string category);
    }
}
