using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCDemoEntityFramework.Models;

namespace MVCDemoEntityFramework.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();

        // GET: EmployeeDetails
        public ActionResult Index()
        {
            var employeeDetails = db.EmployeeDetails.Include(e => e.EmpDepartment);
            return View(employeeDetails.ToList());
        }

        // GET: EmployeeDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }

        // GET: EmployeeDetails/Create
        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.EmpDepartments, "Id", "Name");
            return View();
        }

        // POST: EmployeeDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City,Gender,DateOfBirth,DeptId")] EmployeeDetail employeeDetail)
        {
            if (string.IsNullOrEmpty(employeeDetail.Name))
            {
                ModelState.AddModelError("Name", "Name filed must be required.");
            }

            if (ModelState.IsValid)
            {
                db.EmployeeDetails.Add(employeeDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeptId = new SelectList(db.EmpDepartments, "Id", "Name", employeeDetail.DeptId);
            return View(employeeDetail);
        }

        // GET: EmployeeDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptId = new SelectList(db.EmpDepartments, "Id", "Name", employeeDetail.DeptId);
            return View(employeeDetail);
        }

        // POST: EmployeeDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Name")] EmployeeDetail employeeDetail)
        {
            EmployeeDetail employeeDetailsFromDb = db.EmployeeDetails.Where(x => x.Id == employeeDetail.Id).FirstOrDefault();
            employeeDetailsFromDb.Id = employeeDetail.Id;
            employeeDetailsFromDb.Gender = employeeDetail.Gender;
            employeeDetailsFromDb.City = employeeDetail.City;
            employeeDetailsFromDb.DeptId = employeeDetail.DeptId;
            employeeDetail.Name = employeeDetailsFromDb.Name;

            if (ModelState.IsValid)
            {
                db.Entry(employeeDetailsFromDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.EmpDepartments, "Id", "Name", employeeDetail.DeptId);
            return View(employeeDetail);
        }

        // GET: EmployeeDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }

        // POST: EmployeeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            db.EmployeeDetails.Remove(employeeDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult DepartmentTotal()
        {
            List<DepartmentTotal> totalList = db.EmployeeDetails.Include("Department").GroupBy(x => x.EmpDepartment.Name).Select(x => new Models.DepartmentTotal() { Name = x.Key, Count = x.Count()}).OrderByDescending(y=>y.Count).ToList();

            return View(totalList);
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
