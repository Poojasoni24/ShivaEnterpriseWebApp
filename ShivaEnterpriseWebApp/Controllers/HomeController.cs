using Microsoft.AspNetCore.Mvc;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
