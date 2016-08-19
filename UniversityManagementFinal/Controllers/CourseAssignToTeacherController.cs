using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using UniversityManagementFinal.Models;

namespace UniversityManagementFinal.Controllers
{
    public class CourseAssignToTeacherController : Controller
    {
        private UniversityManagementDBEntities db = new UniversityManagementDBEntities();

        // GET: CourseAssignToTeacher
        public ActionResult Index()
        {
            var courseAssignToTeachers = db.CourseAssignToTeachers.Include(c => c.Course).Include(c => c.Department).Include(c => c.Teacher);
            return View(courseAssignToTeachers.ToList());
        }

        // GET: CourseAssignToTeacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeacher/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode");
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName");
            return View();
        }

        // POST: CourseAssignToTeacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseAssignToTeacherId,DepartmentId,TeacherId,CourseId,CreditToBeTaken,OccupiedCredit,RemainingCredit,CourseName,CourseCredit")] CourseAssignToTeacher courseAssignToTeacher)
        {
            if (ModelState.IsValid)
            {
                if (!db.CourseAssignToTeachers.Any(catt => catt.CourseId == courseAssignToTeacher.CourseId))
                {
                    db.CourseAssignToTeachers.Add(courseAssignToTeacher);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.Write("Course Already Assigned");
                }

            }
            else
            {
                Response.Write("Model is not valid");
            }

            //ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseAssignToTeacher.CourseId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", courseAssignToTeacher.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseAssignToTeacher.CourseId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", courseAssignToTeacher.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // POST: CourseAssignToTeacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseAssignToTeacherId,DepartmentId,TeacherId,CourseId,CreditToBeTaken,OccupiedCredit,RemainingCredit,CourseName,CourseCredit")] CourseAssignToTeacher courseAssignToTeacher)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(courseAssignToTeacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseAssignToTeacher.CourseId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", courseAssignToTeacher.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseAssignToTeacher.TeacherId);
            return View(courseAssignToTeacher);
        }

        // GET: CourseAssignToTeacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            if (courseAssignToTeacher == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignToTeacher);
        }

        // POST: CourseAssignToTeacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseAssignToTeacher courseAssignToTeacher = db.CourseAssignToTeachers.Find(id);
            db.CourseAssignToTeachers.Remove(courseAssignToTeacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: SelectDepartment/AjaxCall
        public JsonResult SelectDepartment()
        {
            db.Configuration.ProxyCreationEnabled = false;
            //var departments = db.Departments.Where(department => department.DepartmentId == id);
            return Json(db.Departments.ToList(), JsonRequestBehavior.AllowGet);
        }

        // POST: SelectTeacher/AjaxCall
        public JsonResult SelectTeacher(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var teachers = db.Teachers.Where(teacher => teacher.DepartmentId == id);
            return Json(teachers);
        }

        // POST: SelectCourse/AjaxCall
        public JsonResult SelectCourse(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var courses = db.Courses.Where(course => course.DepartmentId == id);
            return Json(courses);
        }


        // POST: CreditToBeTaken/AjaxCall
        public JsonResult CreditToBeTaken(int teacherId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var creditsToBeTaken = db.Teachers.Where(teacher => teacher.TeacherId == teacherId).Select(teacher => teacher.CreditToBeTaken);
            return Json(creditsToBeTaken);
        }

        //public JsonResult OccupiedCredit(int teacherId)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var remainingCredits = db.Teachers.SqlQuery("Select OccupiedCredit From Teacher Where TeacherId='" + teacherId + "'");
        //    return Json(remainingCredits);
        //}
        public JsonResult RemainingCredit(int teacherId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var remainingCredits = db.Teachers.SqlQuery("Select RemainingCredit From Teacher Where TeacherId='" + teacherId + "'");
            return Json(remainingCredits);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
