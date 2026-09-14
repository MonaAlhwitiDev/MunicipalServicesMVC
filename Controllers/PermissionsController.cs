using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class PermissionsController : Controller
    {
        // الاتصال بقاعدة البيانات
        private readonly AppDbContext _db;

        // Constructor
        public PermissionsController(AppDbContext db)
        {
            _db = db;
        }

        // عرض جميع الصلاحيات
        public IActionResult Index()
        {
            var permissions = _db.Permissions.ToList();

            return View(permissions);
        }

        // فتح صفحة إضافة صلاحية جديدة
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الصلاحية الجديدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _db.Permissions.Add(permission);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        // فتح صفحة تعديل الصلاحية
        public IActionResult Edit(int id)
        {
            var permission = _db.Permissions.Find(id);

            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // حفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Permission permission)
        {
            if (id != permission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Permissions.Update(permission);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        // فتح صفحة تأكيد الحذف
        public IActionResult Delete(int id)
        {
            var permission = _db.Permissions.Find(id);

            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // حذف الصلاحية
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var permission = _db.Permissions.Find(id);

            if (permission != null)
            {
                _db.Permissions.Remove(permission);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}