/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using SMS_BL.Teacher;
using SMS_Models.Teacher;
using SMS_ViewModels.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherBL _teacherBL=new TeacherBL();
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Get All the teacher
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AllTeachers(int pageNumber, int pageSize, bool? isActive = null)
        {
            var teacherResult = new TeacherViewModel();

            teacherResult.TeacherList = _teacherBL.GetAllTeachers(isActive);

            List<TeacherBO> pageData;
            int totalPage;
            Pagination(pageNumber, pageSize, teacherResult, out pageData, out totalPage);

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
        /// Pagination for teachers
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="teacherResult"></param>
        /// <param name="pageData"></param>
        /// <param name="totalPage"></param>
        private static void Pagination(int pageNumber, int pageSize, TeacherViewModel teacherResult, out List<TeacherBO> pageData, out int totalPage)
        {
            int skip = (pageNumber - 1) * pageSize;


            pageData = teacherResult.TeacherList.OrderBy(s => s.TeacherID).Skip(skip).Take(pageSize).ToList();
            double totalRecords = teacherResult.TeacherList.Count();
            double pagesize = pageSize;

            double totalPages = Math.Ceiling(totalRecords / pageSize);

            totalPage = (int)Math.Round(totalPages);
        }

        /// <summary>
        /// Delete teacher if teacher is not allocated to subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTeacher(int id)
        {
            var msg = "";
            try
            {
                bool deleteTeacher = _teacherBL.DeleteTeacher(id, out msg);


                return Json(new { success = deleteTeacher, message= msg });
            }
            catch
            {
                return Json(new { success = false, message = msg });
            }
        }

        /// <summary>
        /// enable or disable the teacher 
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
                bool isToggle = _teacherBL.ToggleTeacher(id, enable, out msg);

                return Json(new { success = isToggle, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// get teacher for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddTeacher(long id = 0)
        {
            if (id == 0)
            {
                return PartialView("_AddTeacher", new TeacherBO());
            }
            var teacher = _teacherBL.GetTeacherByID(id);
            return PartialView("_AddTeacher", teacher);

        }


        /// <summary>
        /// save new tacher details or update
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacher(TeacherBO teacher)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSaveSuccess = _teacherBL.SaveTeacher(teacher, out msg);

                    return Json(new { success = isSaveSuccess, message = msg });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while adding the teacher: " + ex.Message });
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
        /// Teacher registraion number validation checking and return the json result
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public JsonResult IsTeacherRegNoAvailable(string regNo)
        {
            bool isAvailable = _teacherBL.CheckRegNo(regNo);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Teacher Display name validation checking and return the json result
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public JsonResult IsDisplayAvailable(string displayName)
        {
            bool isAvailable = _teacherBL.CheckTeacherDisplayName(displayName);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check Email avalilabilty
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult IsEmailAvailable(string email)
        {
            bool isAvailable = _teacherBL.CheckEmail(email);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search teacher by search term and search criteria
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ActionResult Search(string searchTerm, string searchCriteria)
        {
            var teacher = _teacherBL.SearchTeacher(searchTerm, searchCriteria).ToList();

            if (teacher.Count > 0)
            {
                return PartialView("_TeacherTable", teacher);
            }
            else
            {
                return PartialView("_TeacherTable", null);
            }
        }

        /// <summary>
        /// Get specific teacher details in partial view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetTeacherDetails(int id)
        {

            var teacherResult = new TeacherViewModel
            {
                Teacher = _teacherBL.GetTeacherByID(id)
            };



            if (teacherResult == null)
            {
                return HttpNotFound();
            }

            return PartialView("_TeacherDetails", teacherResult);
        }


    }
}