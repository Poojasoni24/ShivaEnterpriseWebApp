using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class AccountController : Controller
    {
        IAccountServiceImpl accountService = new AccountServiceImpl();
        IAccountCategoryServiceImpl accountcategoryService = new AccountCategoryServiceImpl();
        IAccountGroupServiceImpl accountgroupService = new AccountGroupServiceImpl();
        IAccountTypeServiceImpl accountTypeService = new AccountTypeServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllAccount = await accountService.GetAccountList(authToken);
            foreach (var item in getAllAccount)
            {
                item.AccountGroup = await accountgroupService.GetAccountGroupById(item.AccountGroupId, authToken);
                item.AccountType = await accountTypeService.GetAccountTypeById(item.AccountTypeId, authToken);
                item.AccountCategory = await accountcategoryService.GetAccountCategoryById(item.AccountCategoryId, authToken);
            }
           
            return View("Index", getAllAccount);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditAccount(Guid accountId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            //Add accountgroup dropdown
            List<AccountGroup> accountGroupDataList = await accountgroupService.GetAccountGroupList(authToken);
            SelectList groupselectList = new SelectList(accountGroupDataList, "AccountGroupId", "AccountGroupName");
            ViewBag.accountGroupSelectList = groupselectList;

            List<AccountType> accountTypeDataList = await accountTypeService.GetAccountTypeList(authToken);
            SelectList typeselectList = new SelectList(accountTypeDataList, "AccountTypeId", "AccountTypeName");
            ViewBag.accountTypeList = typeselectList;

            List<AccountCategory> accountcategoryDataList = await accountcategoryService.GetAccountCategoryList(authToken);
            SelectList categoryselectList = new SelectList(accountcategoryDataList, "AccountCategoryId", "AccountCategoryName");
            ViewBag.accountCategoryList = categoryselectList;
            if (accountId != Guid.Empty)
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
                    account.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    account.CreatedDateTime = DateTime.Now;
                    await accountService.EditAccountDetailsAsync(account, authToken);
                }
                else
                {
                    account.IsActive = true;
                    account.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    account.CreatedDateTime = DateTime.Now;
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

        public async Task<ActionResult> AccountDetail(Guid accountId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var accountData = await accountService.GetAccountById(accountId, authToken);
            return PartialView("_accountDetail", new Account()
            {
                AccountId = accountId,
                AccountCode = accountData.AccountCode,
                AccountName = accountData.AccountName,
                AccountDescription = accountData.AccountDescription,
                IsActive = accountData.IsActive,
                AccountCategoryId = accountData.AccountCategoryId,
                AccountGroupId = accountData.AccountGroupId,
                AccountTypeId = accountData.AccountTypeId,
            });
        }

    }
}
