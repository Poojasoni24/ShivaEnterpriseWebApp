using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Accounts
{
    public class AccountTypeController : Controller
    {
        IAccountTypeServiceImpl accountTypeService = new AccountTypeServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllAccountType = await accountTypeService.GetAccountTypeList(authToken);
            return View("Index", getAllAccountType);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditAccountType(string accountTypeId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(accountTypeId))
            {
                var accountTypeDetail = await accountTypeService.GetAccountTypeById(accountTypeId, authToken);
                if (accountTypeDetail != null)
                {
                    return View("AddOrEditAccountType", accountTypeDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditAccountType(string accountTypeId, AccountType accountType)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(accountTypeId))
                {
                    accountType.AccountTypeId = new Guid(accountTypeId);
                    await accountTypeService.EditAccountTypeDetailsAsync(accountType, authToken);
                }
                else
                {
                    accountType.AccountTypeStatus = true;
                    await accountTypeService.AddAccountTypeDetailsAsync(accountType, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveAccountType(string accountTypeId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await accountTypeService.DeleteAccountType(accountTypeId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove AccountType." });
            }
        }

        public async Task<ActionResult> AccountTypeDetail(string accountTypeId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var accountTypeData = await accountTypeService.GetAccountTypeById(accountTypeId, authToken);
            return PartialView("_accountTypeDetail", new AccountType()
            {
                AccountTypeId = new Guid(accountTypeId),
                AccountTypeCode = accountTypeData.AccountTypeCode,
                AccountTypeName = accountTypeData.AccountTypeName,
                AccountTypeDescription = accountTypeData.AccountTypeDescription,
                AccountTypeStatus = accountTypeData.AccountTypeStatus
            });
        }
    }
}
