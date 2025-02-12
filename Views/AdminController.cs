using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Views
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
