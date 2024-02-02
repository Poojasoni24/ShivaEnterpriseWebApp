using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Accounts
{
    public class AccountCategoryController : Controller
    {
        IAccountCategoryServiceImpl accountCategoryService = new AccountCategoryServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllAccountCategory = await accountCategoryService.GetAccountCategoryList(authToken);
            return View("Index", getAllAccountCategory);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditAccountCategory(string accountCategoryId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(accountCategoryId))
            {
                var accountCategoryDetail = await accountCategoryService.GetAccountCategoryById(accountCategoryId, authToken);
                if (accountCategoryDetail != null)
                {
                    return View("AddOrEditAccountCategory", accountCategoryDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditAccountCategory(string accountId, AccountCategory accountCategory)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(accountId))
                {
                    accountCategory.AccountCategoryId = new Guid(accountId);
                    await accountCategoryService.EditAccountCategoryDetailsAsync(accountCategory, authToken);
                }
                else
                {
                    accountCategory.AccountCategoryStatus = true;
                    await accountCategoryService.AddAccountCategoryDetailsAsync(accountCategory, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveAccountCatgory(string accountCategoryId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await accountCategoryService.DeleteAccountCategory(accountCategoryId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove AccountCategory." });
            }
        }

        public async Task<ActionResult> AccountCategoryDetail(string accountCategoryId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var accountCategoryData = await accountCategoryService.GetAccountCategoryById(accountCategoryId, authToken);
            return PartialView("_accountCategoryDetail", new AccountCategory()
            {
                AccountCategoryId = new Guid(accountCategoryId),
                AccountCategoryCode = accountCategoryData.AccountCategoryCode,
                AccountCategoryName = accountCategoryData.AccountCategoryName,
                AccountCategoryDescription = accountCategoryData.AccountCategoryDescription,
                AccountCategoryStatus = accountCategoryData.AccountCategoryStatus               
            });
        }
    }
}
