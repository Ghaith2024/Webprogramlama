using Microsoft.AspNetCore.Mvc;

namespace BarberApp.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
