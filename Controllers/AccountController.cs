using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Controllers
{
    public class AccountController : Controller
    {
        // الاتصال بقاعدة البيانات
        private readonly AppDbContext _db;

        // Constructor
        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // فتح صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // التحقق من بيانات تسجيل الدخول
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
                return View();
            }

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
                return View();
            }

            // إنشاء بيانات المستخدم
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("RoleId", user.RoleId.ToString())
            };

            if (user.Role != null)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, user.Role.Name)
                );

                var permissions = _db.RolePermissions
                    .Include(rp => rp.Permission)
                    .Where(rp => rp.RoleId == user.RoleId)
                    .ToList();

                foreach (var rolePermission in permissions)
                {
                    if (rolePermission.Permission != null)
                    {
                        claims.Add(
                            new Claim(
                                "Permission",
                                rolePermission.Permission.Name
                            )
                        );
                    }
                }
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // تسجيل الخروج
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction(
                "Login",
                "Account"
            );
        }

        // صفحة عدم وجود صلاحية
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}