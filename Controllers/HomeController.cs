using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Models;
using MunicipalServicesMVC.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // عرض لوحة التحكم
        public IActionResult Index()
        {
            ViewBag.DepartmentsCount =
                _unitOfWork.Departments
                           .GetAll()
                           .Count();

            ViewBag.EmployeesCount =
                _unitOfWork.Employees
                           .GetAllWithDepartment()
                           .Count();

            ViewBag.ServicesCount =
                _unitOfWork.Services
                           .GetAllWithDepartment()
                           .Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}