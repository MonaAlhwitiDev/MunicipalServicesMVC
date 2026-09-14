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
        // الاتصال بقاعدة البيانات
        private readonly AppDbContext _db;

        // Constructor
        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // عرض جميع المستخدمين مع الأدوار
        public IActionResult Index()
        {
            var users = _db.Users
                .Include(u => u.Role)
                .ToList();

            return View(users);
        }

        // فتح صفحة إضافة مستخدم جديد
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name"
            );

            return View();
        }

        // حفظ المستخدم الجديد
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

            return View(user);
        }

        // فتح صفحة تعديل المستخدم
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

            return View(user);
        }

        // حفظ تعديلات المستخدم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

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

                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(
                _db.Roles,
                "Id",
                "Name",
                user.RoleId
            );

            return View(user);
        }

        // فتح صفحة تأكيد الحذف
        public IActionResult Delete(int id)
        {
            var user = _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // حذف المستخدم
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