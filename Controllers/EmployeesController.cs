using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeesController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> employees =
                _db.Employees
                   .Include(e => e.Department)
                   .ToList();

            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name"
            );

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                employee.DepartmentId
            );

            return View(employee);
        }

        public IActionResult Edit(int id)
        {
            Employee? employee = _db.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                employee.DepartmentId
            );

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                employee.DepartmentId
            );

            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            Employee? employee = _db.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            _db.Employees.Remove(employee);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}