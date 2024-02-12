using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class TaxController : Controller
    {
        ITaxServiceImpl taxObj = new TaxServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var taxDetail = await taxObj.GetTaxList(authToken);
            return View("Index", taxDetail);
        }

        [HttpGet]
        public async Task<ActionResult> TaxDetail(string taxId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var TaxDetail = await taxObj.GetTaxById(taxId, authToken);
            return PartialView("_TaxDetail", new Tax()
            {
                TaxId = TaxDetail.TaxId,
                TaxCode = TaxDetail.TaxCode,
                TaxName = TaxDetail.TaxName,
                TaxDescription = TaxDetail.TaxDescription,
                IsActive = TaxDetail.IsActive,
                CreatedDateTime = TaxDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditTax(string taxId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (!string.IsNullOrEmpty(taxId))
            {
                var taxDetail = await taxObj.GetTaxById(taxId, authToken);
                return View(taxDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditTax(Tax tax)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (tax.TaxId != Guid.Empty)
            {
                tax.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                tax.ModifiedDateTime = DateTime.Now;
                await taxObj.EditTaxDetailsAsync(tax, authToken);
            }
            else
            {
                tax.IsActive = true;
                tax.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                tax.CreatedDateTime = DateTime.Now;
                await taxObj.AddTaxDetailsAsync(tax, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveTax(Guid taxId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await taxObj.DeleteTax(taxId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Tax." });
            }
        }
    }
}
