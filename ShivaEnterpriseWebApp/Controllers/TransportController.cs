using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class TransportController : Controller
    {
        ITransportServiceImpl transportObj = new TransportServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var transportDetail = await transportObj.GetTransportList(authToken);
            return View("Index", transportDetail);
        }

        [HttpGet]
        public async Task<ActionResult> TransportDetail(string transportId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var TransportDetail = await transportObj.GetTransportById(transportId, authToken);
            return PartialView("_TransportDetail", new Transport()
            {
                TransportId = TransportDetail.TransportId,
                TransportCode = TransportDetail.TransportCode,
                TransportName = TransportDetail.TransportName,
                TransportDescription = TransportDetail.TransportDescription,
                IsActive = TransportDetail.IsActive,
                CreatedDateTime = TransportDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditTransport(string transportId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (!string.IsNullOrEmpty(transportId))
            {
                var transportDetail = await transportObj.GetTransportById(transportId, authToken);
                return View(transportDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditTransport(Transport transport)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (transport.TransportId != Guid.Empty)
            {
                transport.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                transport.ModifiedDateTime = DateTime.Now;
                await transportObj.EditTransportDetailsAsync(transport, authToken);
            }
            else
            {
                transport.IsActive = true;
                transport.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                transport.CreatedDateTime = DateTime.Now;
                await transportObj.AddTransportDetailsAsync(transport, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveTransport(string transportId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await transportObj.DeleteTransport(transportId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Transport." });
            }
        }
    }
}
