using Microsoft.AspNetCore.Mvc;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
