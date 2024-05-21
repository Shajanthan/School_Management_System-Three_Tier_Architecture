/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
/// 
using SMS_BL.Student;
using SMS_BL.Student.Interface;
using SMS_Data;
using SMS_Models.Student;
using SMS_ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController() {
            _studentRepository = new StudentRepository(new SMS_DBEntities());
        }

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// get all student details
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AllStudent(int pageNumber, int pageSize, bool? isActive = null)
        {
            var studentResults = new StudentViewModel();

            studentResults.StudentList = _studentRepository.GetAllStudents(isActive);

            List<StudentBO> pageData;
            int totalPage;
            Pagination(pageNumber, pageSize, studentResults, out pageData, out totalPage);

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
        /// pagination set
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="studentResults"></param>
        /// <param name="pageData"></param>
        /// <param name="totalPage"></param>
        private static void Pagination(int pageNumber, int pageSize, StudentViewModel studentResults, out List<StudentBO> pageData, out int totalPage)
        {
            int skip = (pageNumber - 1) * pageSize;


            pageData = studentResults.StudentList.OrderBy(s => s.StudentID).Skip(skip).Take(pageSize).ToList();
            double totalRecords = studentResults.StudentList.Count();
            double pagesize = pageSize;

            double totalPages = Math.Ceiling(totalRecords / pageSize);

            totalPage = (int)Math.Round(totalPages);
        }

        /// <summary>
        /// Delete Student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            var msg = "";
            try
            {
                bool deleteStudent = _studentRepository.DeleteStudent(id, out msg);


                return Json(new { success = deleteStudent, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = msg });
            }
        }

        /// <summary>
        /// change student status
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
                bool isToggle = _studentRepository.ToggleStudent(id, enable, out msg);

                return Json(new { success = isToggle, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// check add / edit tudent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddStudent(long id = 0)
        {
            if (id == 0)
            {
                return PartialView("_AddStudent", new StudentBO());
            }
            var student = _studentRepository.GetStudentByID(id);
            return PartialView("_AddStudent", student);

        }


        /// <summary>
        /// add or edit student details
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(StudentBO student)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSaveSuccess = _studentRepository.SaveStudent(student, out msg);

                    return Json(new { success = isSaveSuccess, message = msg });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error occurred while adding the student: " + ex.Message });
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
        /// Check Student Reg No is already exist
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public JsonResult IsStudentRegNoAvailable(string regNo)
        {
            bool isAvailable = _studentRepository.CheckRegNo(regNo);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check Student Display name is already exist
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public JsonResult IsDisplayAvailable(string displayName)
        {
            bool isAvailable = _studentRepository.CheckStudentDisplayName(displayName);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check Student email is already exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult IsEmailAvailable(string email)
        {
            bool isAvailable = _studentRepository.CheckEmail(email);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get student details by passing the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetStudentDetails(int id)
        {

            var studentResult = new StudentViewModel
            {
                Student = _studentRepository.GetStudentByID(id)
            };



            if (studentResult == null)
            {
                return HttpNotFound();
            }

            return PartialView("_StudentDetails", studentResult);
        }

        /// <summary>
        /// search student by givien criteria and term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ActionResult Search(string searchTerm, string searchCriteria)
        {
            var students = _studentRepository.SearchStudent(searchTerm, searchCriteria).ToList();

            if (students.Count > 0)
            {
                return PartialView("_StudentTable", students);
            }
            else
            {
                return PartialView("_StudentTable", null);
            }
        }

    }
   

}