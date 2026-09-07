using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;

        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Service> services =
                _db.Services
                   .Include(s => s.Department)
                   .ToList();

            return View(services);
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

        public IActionResult Edit(int id)
        {
            Service service = _db.Services.Find(id);

            if (service == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        public IActionResult Delete(int id)
        {
            Service service = _db.Services
                .Include(s => s.Department)
                .FirstOrDefault(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _db.Services.Add(service);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        [HttpPost]
        public IActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                _db.Services.Update(service);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        [HttpPost]
        public IActionResult Delete(Service service)
        {
            _db.Services.Remove(service);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}