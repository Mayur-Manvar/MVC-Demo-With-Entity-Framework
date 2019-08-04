using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult GetEmployeeDetails()
        {
            EmployeeManagement employeeManagement = new EmployeeManagement();
            List<Employee> employeeList = employeeManagement.GetEmployeeDetails().ToList();

            return View(employeeList);
        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            return View();
        }

        //Bind model value using formCollection object.
        //[HttpPost]
        //[ActionName("Create")]
        //public ActionResult Create_Post(FormCollection formCollection)
        //{
        //    Employee employee = new Employee();
        //    employee.Name = formCollection["Name"];
        //    employee.Gender = formCollection["Gender"];
        //    employee.City = formCollection["City"];
        //    employee.DateOfBirth = Convert.ToDateTime(formCollection["DateOfBirth"]);
        //    EmployeeManagement employeeManagement = new EmployeeManagement();

        //    employeeManagement.AddNewEmployee(employee);

        //    return RedirectToAction("GetEmployeeDetails");
        //}

        // Bind value based on single parameter of model class.
        //[HttpPost]
        //[ActionName("Create")]
        //public ActionResult Create_Post(string name, string gender, string city, DateTime dateOfBirth)
        //{
        //    Employee employee = new Employee();
        //    employee.Name = name;
        //    employee.Gender = gender;
        //    employee.City = city;
        //    employee.DateOfBirth = Convert.ToDateTime(dateOfBirth);
        //    EmployeeManagement employeeManagement = new EmployeeManagement();

        //    employeeManagement.AddNewEmployee(employee);

        //    return RedirectToAction("GetEmployeeDetails");
        //}


        //Bind value using UpdateModel<>();
        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post()
        {
            Employee employee = new Employee();
            UpdateModel<Employee>(employee);

            if (ModelState.IsValid)
            {
                EmployeeManagement employeeManagement = new EmployeeManagement();

                employeeManagement.AddNewEmployee(employee);

                return RedirectToAction("GetEmployeeDetails");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            EmployeeManagement employeeManagement = new EmployeeManagement();
            Employee employee = employeeManagement.GetEmployeeDetails().Where(x => x.Id == id).FirstOrDefault();
            return View(employee);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_Post(int id)
        {
            EmployeeManagement employeeManagement = new EmployeeManagement();

            Employee employee = employeeManagement.GetEmployeeDetails().Where(x => x.Id == id).FirstOrDefault();

            UpdateModel<IEmployee>(employee);

            //UpdateModel(employee, null, null, new string[] { "Name" });

            if (ModelState.IsValid)
            {
                employeeManagement.SaveEmployee(employee);
                return RedirectToAction("GetEmployeeDetails");
            }
            else
            {
                return View(employee);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            EmployeeManagement employeeManagement = new EmployeeManagement();

            employeeManagement.DeleteEmployee(id);

            return RedirectToAction("GetEmployeeDetails");
        }
    }
}