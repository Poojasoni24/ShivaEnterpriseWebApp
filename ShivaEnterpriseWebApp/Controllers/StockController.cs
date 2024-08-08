using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class StockController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IStockServiceImpl stockService = new StockServiceImpl();
        public StockController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            if (authToken == null)
                return BadRequest("Something went wrong");

            List<Stock> stockList = await stockService.GetStockList(authToken);

            return View("Index", stockList);
        }
    }
}
