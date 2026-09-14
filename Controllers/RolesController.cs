using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class RolesController : Controller
    {
        // الاتصال بقاعدة البيانات
        private readonly AppDbContext _db;

        // Constructor
        public RolesController(AppDbContext db)
        {
            _db = db;
        }

        // عرض جميع الأدوار
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }

        // فتح صفحة إضافة دور جديد
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الدور الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _db.Roles.Add(role);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // فتح صفحة تعديل الدور
        public IActionResult Edit(int id)
        {
            var role = _db.Roles.Find(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // حفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Roles.Update(role);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // فتح صفحة تأكيد الحذف
        public IActionResult Delete(int id)
        {
            var role = _db.Roles.Find(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // حذف الدور
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _db.Roles.Find(id);

            if (role != null)
            {
                _db.Roles.Remove(role);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // فتح صفحة ربط الدور بالصلاحيات
        public IActionResult Permissions(int id)
        {
            var role = _db.Roles.Find(id);

            if (role == null)
            {
                return NotFound();
            }

            var selectedPermissionIds = _db.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionId)
                .ToList();

            var viewModel = new RolePermissionsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = _db.Permissions.ToList(),
                SelectedPermissionIds = selectedPermissionIds
            };

            return View(viewModel);
        }

        // حفظ صلاحيات الدور
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Permissions(RolePermissionsViewModel model)
        {
            var oldPermissions = _db.RolePermissions
                .Where(rp => rp.RoleId == model.RoleId)
                .ToList();

            _db.RolePermissions.RemoveRange(oldPermissions);

            if (model.SelectedPermissionIds != null)
            {
                foreach (var permissionId in model.SelectedPermissionIds)
                {
                    _db.RolePermissions.Add(new RolePermission
                    {
                        RoleId = model.RoleId,
                        PermissionId = permissionId
                    });
                }
            }

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}