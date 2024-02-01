using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class RoleController : Controller
    {
        IRoleServiceImpl roleService =  new RoleServiceImpl();
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllRole = await roleService.GetRoleList(authToken);
            return View("Index", getAllRole);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditRole(string roleId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(roleId))
            {
                var roledetail = await roleService.GetRoleById(roleId,authToken);
                if (roledetail != null)
                {
                    return View("AddOrEditRole", roledetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditRole(string roleId , Role role)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(roleId))
                {
                    role.Id = roleId;
                    await roleService.EditRoleAsync(role, authToken);
                }
                else
                {
                    role.IsActive = true;
                    role.CreatedDateTime = DateTime.Now;
                    role.NormalizedName = role.Name;
                    await roleService.AddRoleAsync(role, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> RemoveRole(string roleId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await roleService.DeleteRole(roleId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Role." });
            }
        }

        public async Task<ActionResult> RoleDetails(string roleId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var roleData = await roleService.GetRoleById(roleId, authToken);
            return PartialView("_roleDetail", new Role()
            {
                Id = roleId,
                Name = roleData.Name,
                IsActive = roleData.IsActive               
            });
        }

    }
}
