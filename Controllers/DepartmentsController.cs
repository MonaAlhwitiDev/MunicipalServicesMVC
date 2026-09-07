using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _db;

        public DepartmentsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Department> departments =
                _db.Departments
                   .Include(d => d.Employees)
                   .Include(d => d.Services)
                   .ToList();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Add(department);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(department);
        }

        public IActionResult Edit(int id)
        {
            Department? department = _db.Departments.Find(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Update(department);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(department);
        }

        public IActionResult Delete(int id)
        {
            Department? department = _db.Departments.Find(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            _db.Departments.Remove(department);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

