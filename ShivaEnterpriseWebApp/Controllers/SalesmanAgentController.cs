using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class SalesmanAgentController : Controller
    {
        ISalesmanAgentServiceImpl salesmanAgentService = new SalesmanAgentServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var salesmanAgentData = await salesmanAgentService.GetSalesmanAgentList(authToken);
            return View("Index", salesmanAgentData);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditSalesmanAgent(string salesmanAgentId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(salesmanAgentId))
            {
                var salesmanAgentDetail = await salesmanAgentService.GetSalesmanAgentById(salesmanAgentId, authToken);
                if (salesmanAgentDetail != null)
                {
                    return View("AddOrEditSalesmanAgent", salesmanAgentDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditSalesmanAgent(string salesmanAgentId, SalesmanAgent salesmanAgent)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(salesmanAgentId))
                {
                    salesmanAgent.SalesmanAgentID = new Guid(salesmanAgentId);
                    await salesmanAgentService.EditSalesmanAgentDetailsAsync(salesmanAgent, authToken);
                }
                else
                {
                    salesmanAgent.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesmanAgent.CreatedDateTime = DateTime.Now;
                    salesmanAgent.IsActive = true;
                    await salesmanAgentService.AddSalesmanAgentDetailsAsync(salesmanAgent, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemovesalesmanAgent(string salemanAgentId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await salesmanAgentService.DeleteSalesmanAgent(salemanAgentId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove salesmanAgent." });
            }
        }

        public async Task<ActionResult> SalesmanAgentDetail(string salemanAgentId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var salesmanAgentData = await salesmanAgentService.GetSalesmanAgentById(salemanAgentId, authToken);
            return PartialView("_salesmanAgentDetail", new SalesmanAgent()
            {
                SalesmanAgentID = new Guid(salemanAgentId),
                Salesmancode = salesmanAgentData.Salesmancode,
                SalesmanName = salesmanAgentData.SalesmanName,
                Salesmanemail = salesmanAgentData.Salesmanemail,
                Salesmanphone = salesmanAgentData.Salesmanphone,
                IsActive = salesmanAgentData.IsActive
            });
        }
    }
}
