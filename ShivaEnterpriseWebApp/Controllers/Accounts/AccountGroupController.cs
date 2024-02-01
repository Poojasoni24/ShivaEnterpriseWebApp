using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Accounts
{
    public class AccountGroupController : Controller
    {
        IAccountGroupServiceImpl accountGroupService = new AccountGroupServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllAccountGroup = await accountGroupService.GetAccountGroupList(authToken);
            return View("Index", getAllAccountGroup);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditAccountGroup(string accountGroupId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(accountGroupId))
            {
                var accountGroupDetail = await accountGroupService.GetAccountGroupById(accountGroupId, authToken);
                if (accountGroupDetail != null)
                {
                    return View("AddOrEditAccountGroup", accountGroupDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditAccountGroup(string accountGroupId, AccountGroup accountGroup)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(accountGroupId))
                {
                    accountGroup.AccountGroupId = new Guid(accountGroupId);
                    await accountGroupService.EditAccountGroupDetailsAsync(accountGroup, authToken);
                }
                else
                {
                    accountGroup.AccountGroupStatus = true;
                    await accountGroupService.AddAccountGroupDetailsAsync(accountGroup, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveAccountGroup(string accountGroupId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await accountGroupService.DeleteAccountGroup(accountGroupId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove AccountGroup." });
            }
        }

        public async Task<ActionResult> AccountGroupDetail(string accountGroupId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var accountGroupData = await accountGroupService.GetAccountGroupById(accountGroupId, authToken);
            return PartialView("_accountGroupDetail", new AccountGroup()
            {
                AccountGroupId = new Guid(accountGroupId),
                AccountGroupCode = accountGroupData.AccountGroupCode,
                AccountGroupName = accountGroupData.AccountGroupName,
                AccountGroupDescription = accountGroupData.AccountGroupDescription,
                AccountGroupStatus = accountGroupData.AccountGroupStatus
            });
        }
    }
}
