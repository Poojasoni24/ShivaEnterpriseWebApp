using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace ShivaEnterpriseWebApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IInventoryServiceImpl inventoryService = new InventoryServiceImpl();
        public InventoryController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            if (authToken == null)
                return BadRequest("Something went wrong");

            List<Inventory> inventoryList = await inventoryService.GetInventoryList(authToken);

            return View("Index", inventoryList);
        }
    }
}
