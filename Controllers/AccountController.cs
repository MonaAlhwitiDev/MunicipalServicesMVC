using Microsoft.AspNetCore.Mvc;

namespace MunicipalServicesMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}