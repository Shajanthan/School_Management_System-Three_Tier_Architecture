/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using SMS_BL.Subject;
using SMS_ViewModels.Subject;
using System.Linq;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using SMS_Models.Subject;
using SMS_BL.Subject.Interface;
using SMS_Data;

namespace SMS.Controllers
{
    public class SubjectController : Controller
    {

        //private readonly SubjectBL _subjectBL = new SubjectBL();

        private readonly ISubjectRepository _subjectRepository;

        public SubjectController()
        {
            _subjectRepository = new SubjectRepository(new SMS_DBEntities());
        }

        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Subject
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AllSubjects(int pageNumber, int pageSize, bool? isActive = null)
        {
            var subjectResult = new SubjectViewModel();

            //subjectResult.SubjectList = _subjectBL.GetAllSubject(isActive);

            subjectResult.SubjectList = _subjectRepository.GetAllSubject(isActive);

            List<SubjectBO> pageData;
            int totalPage;
            Pagination(pageNumber, pageSize, subjectResult, out pageData, out totalPage);

            if (pageData.Count > 0)
            {
                return Json(new { success = true, data = pageData, totalPages = totalPage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "No Data Found", totalPages = totalPage }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// For pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectResult"></param>
        /// <param name="pageData"></param>
        /// <param name="totalPage"></param>
        private static void Pagination(int pageNumber, int pageSize, SubjectViewModel subjectResult, out List<SubjectBO> pageData, out int totalPage)
        {
            int skip = (pageNumber - 1) * pageSize;


            pageData = subjectResult.SubjectList.OrderBy(s => s.SubjectID).Skip(skip).Take(pageSize).ToList();
            double totalRecords = subjectResult.SubjectList.Count();
            double pagesize = pageSize;

            double totalPages = Math.Ceiling(totalRecords / pageSize);

            totalPage = (int)Math.Round(totalPages);
        }

        /// <summary>
        /// Delete Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSubject(int id)
        {
            var msg = "";
            try
            {
                //bool deleteSubject = _subjectBL.DeleteSubject(id,out msg);

                bool deleteSubject = _subjectRepository.DeleteSubject(id, out msg);

                return Json(new { success = deleteSubject, msg});
            }
            catch
            {
                return Json(new { success = false, msg });
            }
        }

        /// <summary>
        /// Get Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddSubject(long id=0)
        {
            if (id == 0) {
                return PartialView("_AddSubject",new SubjectBO());
            }
            //var subject=_subjectBL.GetSubjetByID(id);
            var subject = _subjectRepository.GetSubjetByID(id);
            return PartialView("_AddSubject", subject);

        }


        /// <summary>
        /// Add Subjects
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(SubjectBO subject)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //bool isSaveSuccess = _subjectBL.SaveSubject(subject, out msg);
                    bool isSaveSuccess = _subjectRepository.SaveSubject(subject, out msg);

                    return Json(new { success = isSaveSuccess, message = msg });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while adding the subject: " + ex.Message });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return Json(new { success = false, message = "Please fill all details.", errors = errors });
            }
        }

        /// <summary>
        /// Subject_code Validation json result
        /// </summary>
        /// <param name="subCode"></param>
        /// <returns></returns>
        public JsonResult IsSubCodeAvailable(string subCode)
        {
            //bool isAvailable = _subjectBL.CheckSubCode(subCode);
            bool isAvailable = _subjectRepository.CheckSubCode(subCode);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Subject_Name Validation jason result
        /// </summary>
        /// <param name="subName"></param>
        /// <returns></returns>
        public JsonResult IsSubNameAvailable(string subName)
        {
            //bool isAvailable = _subjectBL.CheckSubname(subName);
            bool isAvailable = _subjectRepository.CheckSubname(subName);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Toggle Subject Avaliable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ToggleEnable(int id, bool enable)
        {
            var msg = "";
            try
            {
                //bool isToggle = _subjectBL.ToggleSubject(id, enable, out msg);
                bool isToggle = _subjectRepository.ToggleSubject(id, enable, out msg);

                return Json(new { success = isToggle, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// Search Subject
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ActionResult Search(string searchTerm, string searchCriteria)
        {
            //var subjects = _subjectBL.SearchSubject(searchTerm, searchCriteria).ToList();

            var subjects = _subjectRepository.SearchSubject(searchTerm, searchCriteria).ToList();

            if (subjects.Count > 0)
            {
                return PartialView("_SubjectTable", subjects);
            }
            else
            {
                return PartialView("_SubjectTable", null);
            }
        }






    }
}