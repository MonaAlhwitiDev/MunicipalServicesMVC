using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MunicipalServicesMVC.Models;
using MunicipalServicesMVC.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // Index
        // =========================
        public IActionResult Index()
        {
            IEnumerable<Employee> employees =
                _unitOfWork.Employees.GetAllWithDepartment();

            var employeeImages =
                _unitOfWork.Employees.GetLatestImages();

            ViewBag.EmployeeImages = employeeImages;

            return View(employees);
        }

        // =========================
        // Create - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
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
                _unitOfWork.Employees.Add(employee);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
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
                _unitOfWork.Employees.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
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
                _unitOfWork.Employees.Update(employee);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
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
                _unitOfWork.Employees
                           .GetByIdWithDepartment(id);

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
            Employee? existingEmployee =
                _unitOfWork.Employees.GetById(employee.Id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            _unitOfWork.Employees.Delete(existingEmployee);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // Manage Files - GET
        // =========================
        [HttpGet]
        public IActionResult ManageFiles(int id)
        {
            Employee? employee =
                _unitOfWork.Employees.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            var files =
                _unitOfWork.Employees
                           .GetFiles(id)
                           .ToList();

            ViewBag.EmployeeId = employee.Id;
            ViewBag.EmployeeName = employee.Name;
            ViewBag.EmployeeFiles = files;

            EmployeeFile employeeFile =
                new EmployeeFile
                {
                    EmployeeId = employee.Id
                };

            return View(employeeFile);
        }

        // =========================
        // Manage Files - POST
        // رفع ملفات متعددة
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageFiles(
            EmployeeFile employeeFile,
            List<IFormFile> fileEmployees)
        {
            Employee? employee =
                _unitOfWork.Employees
                           .GetById(employeeFile.EmployeeId);

            if (employee == null)
            {
                return NotFound();
            }

            if (fileEmployees == null ||
                fileEmployees.Count == 0)
            {
                TempData["FileError"] =
                    "الرجاء اختيار ملف واحد على الأقل.";

                return RedirectToAction(
                    nameof(ManageFiles),
                    new
                    {
                        id = employeeFile.EmployeeId
                    }
                );
            }

            foreach (var fileEmployee in fileEmployees)
            {
                if (fileEmployee.Length > 0)
                {
                    EmployeeFile newFile =
                        new EmployeeFile
                        {
                            EmployeeId =
                                employeeFile.EmployeeId,

                            Name =
                                "ملف الموظف"
                        };

                    newFile.FileURL =
                        UploadEmployeeFile(
                            fileEmployee,
                            newFile.Name
                        );

                    _unitOfWork.Employees
                               .AddFile(newFile);
                }
            }

            _unitOfWork.Save();

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
                _unitOfWork.Employees
                           .GetFileById(id);

            if (employeeFile == null)
            {
                return NotFound();
            }

            int employeeId =
                employeeFile.EmployeeId;

            // حذف الملف من wwwroot
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

            // حذف بيانات الملف من قاعدة البيانات
            _unitOfWork.Employees
                       .DeleteFile(employeeFile);

            _unitOfWork.Save();

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