using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // عرض المستخدمين
        // =========================
        public IActionResult Index()
        {
            var users = _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .ToList();

            return View(users);
        }

        // =========================
        // Create - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name"
            );

            ViewBag.Employees = new SelectList(
                _db.Employees.OrderBy(e => e.Name),
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
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<User>();

                user.Password = passwordHasher.HashPassword(
                    user,
                    user.Password
                );

                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _db.Employees.OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Edit - GET
        // =========================
        public IActionResult Edit(int id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _db.Employees.OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Edit - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            // كلمة المرور لا تتغير من صفحة التعديل
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                var existingUser = _db.Users.Find(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;

                // ربط حساب المستخدم بالموظف
                existingUser.EmployeeId = user.EmployeeId;

                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _db.Employees.OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Delete - GET
        // =========================
        public IActionResult Delete(int id)
        {
            var user = _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // =========================
        // Delete - POST
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _db.Users.Find(id);

            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}