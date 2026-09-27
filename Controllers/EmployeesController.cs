using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeesController(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // Index
        // =========================
        public IActionResult Index()
        {
            IEnumerable<Employee> employees =
                _db.Employees
                   .Include(e => e.Department)
                   .ToList();

            // جلب آخر صورة مرفوعة لكل موظف
            var employeeImages = _db.EmployeeFiles
                .GroupBy(f => f.EmployeeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(f => f.Id)
                          .First()
                          .FileURL
                );

            ViewBag.EmployeeImages = employeeImages;

            return View(employees);
        }

        // =========================
        // Create - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                _db.Departments,
                "Id",
                "Name"
            );

            return View();
        }

        // =========================
        // Create - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // =========================
        // Edit - GET
        // =========================
        public IActionResult Edit(int id)
        {
            Employee? employee =
                _db.Employees.Find(id);

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

        // =========================
        // Edit - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // =========================
        // Delete - GET
        // =========================
        public IActionResult Delete(int id)
        {
            Employee? employee =
                _db.Employees
                   .Include(e => e.Department)
                   .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // =========================
        // Delete - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee employee)
        {
            _db.Employees.Remove(employee);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // Manage Files - GET
        // =========================
        [HttpGet]
        public IActionResult ManageFiles(int id)
        {
            Employee? employee =
                _db.Employees
                   .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var files =
                _db.EmployeeFiles
                   .Where(f => f.EmployeeId == id)
                   .ToList();

            ViewBag.EmployeeId = employee.Id;
            ViewBag.EmployeeName = employee.Name;
            ViewBag.EmployeeFiles = files;

            EmployeeFile employeeFile = new EmployeeFile
            {
                EmployeeId = employee.Id
            };

            return View(employeeFile);
        }

        // =========================
        // Manage Files - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageFiles(
            EmployeeFile employeeFile,
            IFormFile fileEmployee)
        {
            Employee? employee =
                _db.Employees
                   .FirstOrDefault(
                       e => e.Id == employeeFile.EmployeeId
                   );

            if (employee == null)
            {
                return NotFound();
            }

            if (fileEmployee == null ||
                fileEmployee.Length == 0)
            {
                TempData["FileError"] =
                    "الرجاء اختيار صورة.";

                return RedirectToAction(
                    nameof(ManageFiles),
                    new
                    {
                        id = employeeFile.EmployeeId
                    }
                );
            }

            // اسم تلقائي للصورة
            employeeFile.Name = "صورة الموظف";

            // رفع الصورة وحفظ الرابط
            employeeFile.FileURL =
                UploadEmployeeFile(
                    fileEmployee,
                    employeeFile.Name
                );

            // نخلي SQL Server يولد Id تلقائياً
            employeeFile.Id = 0;

            // حفظ بيانات الصورة في قاعدة البيانات
            _db.EmployeeFiles.Add(employeeFile);
            _db.SaveChanges();

            return RedirectToAction(
                nameof(ManageFiles),
                new
                {
                    id = employeeFile.EmployeeId
                }
            );
        }

        // =========================
        // Delete Employee File
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(int id)
        {
            EmployeeFile? employeeFile =
                _db.EmployeeFiles
                   .FirstOrDefault(f => f.Id == id);

            if (employeeFile == null)
            {
                return NotFound();
            }

            int employeeId =
                employeeFile.EmployeeId;

            // حذف الصورة من wwwroot
            if (!string.IsNullOrEmpty(
                    employeeFile.FileURL))
            {
                string filePath =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        employeeFile.FileURL.TrimStart('/')
                    );

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // حذف بيانات الصورة من قاعدة البيانات
            _db.EmployeeFiles.Remove(employeeFile);
            _db.SaveChanges();

            return RedirectToAction(
                nameof(ManageFiles),
                new
                {
                    id = employeeId
                }
            );
        }

        // =========================
        // Upload Employee File
        // =========================
        private string UploadEmployeeFile(
            IFormFile file,
            string name)
        {
            string extension =
                Path.GetExtension(file.FileName);

            string fileName =
                name +
                "_" +
                Guid.NewGuid().ToString() +
                extension;

            string folderPath =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Files",
                    "Employees"
                );

            Directory.CreateDirectory(folderPath);

            string filePath =
                Path.Combine(
                    folderPath,
                    fileName
                );

            using (
                FileStream stream =
                    new FileStream(
                        filePath,
                        FileMode.Create
                    )
            )
            {
                file.CopyTo(stream);
            }

            return "/Files/Employees/" + fileName;
        }
    }
}