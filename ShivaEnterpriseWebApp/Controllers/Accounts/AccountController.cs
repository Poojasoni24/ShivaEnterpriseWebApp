using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class AccountController : Controller
    {
        IAccountServiceImpl accountService = new AccountServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllAccount = await accountService.GetAccountList(authToken);
            return View("Index", getAllAccount);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditAccount(string accountId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(accountId))
            {
                var accountDetail = await accountService.GetAccountById(accountId, authToken);
                if (accountDetail != null)
                {
                    return View("AddOrEditAccount", accountDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditAccount(string accountId, Account account)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(accountId))
                {
                    account.AccountId = new Guid(accountId);
                    await accountService.EditAccountDetailsAsync(account, authToken);
                }
                else
                {
                    account.AccountStatus = true;
                    await accountService.AddAccountDetailsAsync(account, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveAccount(string accountId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await accountService.DeleteAccount(accountId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Account." });
            }
        }

        public async Task<ActionResult> AccountDetail(string accountId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var accountData = await accountService.GetAccountById(accountId, authToken);
            return PartialView("_accountDetail", new Account()
            {
                AccountId = new Guid(accountId),
                AccountCode = accountData.AccountCode,
                AccountName = accountData.AccountName,
                AccountDescription = accountData.AccountDescription,
                AccountStatus = accountData.AccountStatus,
                AccountCategoryId = accountData.AccountCategoryId,
                AccountGroupId = accountData.AccountGroupId,
                AccountTypeId = accountData.AccountTypeId,
            });
        }

    }
}
