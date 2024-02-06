using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class ModeOfPaymentController : Controller
    {
        IModeofPaymentServiceImpl modobj = new ModeofPaymentServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var modDetail = await modobj.GetModeofPaymentList(authToken);
            return View("Index", modDetail);
        }

        [HttpGet]
        public async Task<ActionResult> ModeofPaymentDetail(string modId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var modDetail = await modobj.GetModeofPaymentById(modId, authToken);
            return PartialView("_TaxDetail", new ModeofPayment()
            {
                MODId = modDetail.MODId,
                MODCode = modDetail.MODCode,
                MODName = modDetail.MODName,
                MODDescription = modDetail.MODDescription,
                IsActive = modDetail.IsActive,
                CreatedDateTime = modDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditModeofpayment(string modId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (!string.IsNullOrEmpty(modId))
            {
                var modDetail = await modobj.GetModeofPaymentById(modId, authToken);
                return View(modDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditTax(ModeofPayment mod)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (mod.MODId != Guid.Empty)
            {
                mod.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                mod.ModifiedDateTime = DateTime.Now;
                await modobj.EditModeofPaymentAsync(mod, authToken);
            }
            else
            {
                mod.IsActive = true;
                mod.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                mod.CreatedDateTime = DateTime.Now;
                await modobj.AddModeofPaymentAsync(mod, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveModeofPayment(string ModId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await modobj.DeleteModeofPayment(ModId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove mod." });
            }
        }
    }
}
