using SMS_BL.Allocation;
using SMS_BL.Student;
using SMS_BL.Subject;
using SMS_BL.Teacher;
using SMS_Data;
using SMS_Models.Allocation;
using SMS_Models.Student;
using SMS_ViewModels.Allocation;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class AllocationController : Controller
    {

        private readonly AllocationBL _allocationBL = new AllocationBL();

        private readonly SubjectBL _subjectBL = new SubjectBL();

        private readonly TeacherBL _teacherBL = new TeacherBL();

        private readonly StudentBL _studentBL = new StudentBL();

        //Load ViewBag Values
        public AllocationController() {

            ViewBag.Subjects = _subjectBL.GetAllSubject().Where(s => s.IsEnable == true)
               .Select(s => new { SubjectID = s.SubjectID, Name = s.SubjectCode + " - " + s.Name })
               .ToList();

            ViewBag.Teachers = _teacherBL.GetAllTeachers().Where(s => s.IsEnable == true)
                .Select(s => new { TeacherID = s.TeacherID, DisplayName = s.TeacherRegNo + " - " + s.DisplayName })
               .ToList();

            ViewBag.Students = _studentBL.GetAllStudents().Where(s => s.IsEnable == true)
               .Select(s => new { StudentID = s.StudentID, DisplayName = s.StudentRegNo + " - " + s.DisplayName })
              .ToList();

        }

        // GET: Allocation
        public ActionResult Index()
        {
            return View();
        }


        //----------------------------------------------------------Subject Allocations---------------------------------------------------------------------

        /// <summary>
        /// All Subject Allocations
        /// </summary>
        /// <returns></returns>
        public ActionResult AllSubjectAllocation() 
        {
            var allocatedSubject = new AllocationViewModel();

            allocatedSubject.SubjectAllocationList = _allocationBL.GetAllSubjectAllocation();

            return PartialView("_AllocatedSubjects", allocatedSubject.SubjectAllocationList);
        }


        /// <summary>
        /// Delete subject Allocation
        /// </summary>
        /// <param name="subjectAllocationID"></param>
        /// <returns></returns>
        public ActionResult DeleteSubjectAllocation(long id)
        {
            var msg = "";
            try
            {
                bool deleteSubjectAllocation = _allocationBL.DeleteSubjectAllocation(id, out msg);


                return Json(new { success = deleteSubjectAllocation, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = msg });
            }
        }

        /// <summary>
        /// Check Edit or create
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddTeacherSubjectAllocation(long id = 0)
        {
            if (id == 0)
            {
                return PartialView("_AddTeacherSubjectAllocation", new SubjectAllocationBO());
            }
            var subjectAllocation = _allocationBL.GetSubjectAllocationByID(id);
            return PartialView("_AddTeacherSubjectAllocation", subjectAllocation);

        }


        /// <summary>
        /// Add subject allocation
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacherSubjectAllocation(SubjectAllocationBO subjectAllocation)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSaveSuccess = _allocationBL.SaveSubjectAllocation(subjectAllocation, out msg);

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
        /// Student Alocation search
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ActionResult SubjectAllocationSearch(string searchTerm, string searchCriteria)
        {
            var subjects = _allocationBL.SearchSubjectAllocation(searchTerm, searchCriteria).ToList();

            if (subjects.Count > 0)
            {
                //return Json(students, JsonRequestBehavior.AllowGet);
                return PartialView("_SubjectAllocationSearchTable", subjects);
            }
            else
            {
                //return Json(students, JsonRequestBehavior.AllowGet);
                return PartialView("_SubjectAllocationSearchTable", null);
            }
        }

        //----------------------------------------------------------STUDENTS Allocations---------------------------------------------------------------------

        /// <summary>
        /// get the all student alloaction 
        /// </summary>
        /// <returns></returns>
        public ActionResult AllStudentAllocation(bool? isActive = null)
        {
            var allStudentAllocations = new AllocationViewModel();


            allStudentAllocations.StudentAllocationList = _allocationBL.GetAllStudentAllocation(isActive);

            return PartialView("_AllocatedStudents", allStudentAllocations.StudentAllocationList);

            //var allStudentAllocations = _allocationBL.GetAllStudentAllocation();

            //if (allStudentAllocations != null)
            //{

            //    return Json(new { success = true, data = allStudentAllocations }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { success = false, message = "No Data Found" }, JsonRequestBehavior.AllowGet);
            //}
        }

        /// <summary>
        /// Delete the specific allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteStudentAllocation(long id)
        {
            var msg = "";
            try
            {
                bool deleteStudentAllocation = _allocationBL.DeleteStudentAllocation(id, out msg);


                return Json(new { success = deleteStudentAllocation, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = msg });
            }
        }

        /// <summary>
        /// Delete all the allocation for the student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAllStudentAllocation(long id)
        {
            var msg = "";
            try
            {
                bool deleteStudentAllocation = _allocationBL.DeleteAllStudentAllocation(id, out msg);


                return Json(new { success = deleteStudentAllocation, message = msg });
            }
            catch
            {
                return Json(new { success = false, message = msg });
            }
        }

        /// <summary>
        /// check it is new entry or existing entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddStudentAllocation(long id = 0)
        {
            if (id == 0)
            {
                return PartialView("_AddStudentAllocation", new StudentAllocationBO());
            }
            var subjectAllocation = _allocationBL.GetStudenttAllocationByID(id);
            return PartialView("_AddStudentAllocation", subjectAllocation);

        }

        /// <summary>
        /// Add or edit Student Allocation
        /// </summary>
        /// <param name="studentAllocation"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudentAllocation(StudentAllocationBO studentAllocation)
        {

            var msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSaveSuccess = _allocationBL.SaveStudentAllocation(studentAllocation, out msg);

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
        /// Get allocated Subject
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllocatedSubject()
        {
            var data = _allocationBL.GetAllocatedSubjects().ToList();

            if (data.Count > 0)
            {
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "No Data Found" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Get subject teacher bu passing the id
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public JsonResult GetTeachersBySubject(long subjectId)
        {
            var teachers = _allocationBL.GetTeachersBySubject(subjectId);
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get allocation id by passing the subjectid and teacher id
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllocationID(long subjectId,long teacherId)
        {
            var allocationID = _allocationBL.GetSubjectAllocationID(subjectId,teacherId);
            return Json(allocationID, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Student Alocation search
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ActionResult StudentAllocationSearch(string searchTerm, string searchCriteria)
        {
            var students = _allocationBL.SearchStudentAllocation(searchTerm, searchCriteria).ToList();

            if (students.Count > 0)
            {
                //return Json(students, JsonRequestBehavior.AllowGet);
                return PartialView("_StudentAllocationSearchTable",  students);
            }
            else
            {
                //return Json(students, JsonRequestBehavior.AllowGet);
                return PartialView("_StudentAllocationSearchTable", null);
            }
        }

    }
}