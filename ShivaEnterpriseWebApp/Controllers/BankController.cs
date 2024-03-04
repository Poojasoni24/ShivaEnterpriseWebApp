using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class BankController : Controller
    {
        IBankServiceImpl bankObj = new BankServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var bankDetail = await bankObj.GetBankList(authToken);
            return View("Index", bankDetail);
        }

        [HttpGet]
        public async Task<ActionResult> BankDetail(Guid bankId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var bankDetail = await bankObj.GetBankById(bankId, authToken);
            return PartialView("_BankDetail", new Bank()
            {
                BankId = bankDetail.BankId,
                BankCode = bankDetail.BankCode,
                BankName = bankDetail.BankName,
                BankDescription = bankDetail.BankDescription,
                IsActive = bankDetail.IsActive,
                CreatedDateTime = bankDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditBank(Guid bankId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (bankId != Guid.Empty)
                //if (!string.IsNullOrEmpty(bankId))
            {
                var bankDetail = await bankObj.GetBankById(bankId, authToken);
                return View(bankDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditBank(Bank bank)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (bank.BankId != Guid.Empty)
            {
                bank.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                bank.ModifiedDateTime = DateTime.Now;
                await bankObj.EditBankDetailsAsync(bank, authToken);
            }
            else
            {
                bank.IsActive = true;
                bank.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                bank.CreatedDateTime = DateTime.Now;
                await bankObj.AddBankDetailsAsync(bank, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBank(string bankId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await bankObj.DeleteBank(bankId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Bank." });
            }
        }
    }
}
