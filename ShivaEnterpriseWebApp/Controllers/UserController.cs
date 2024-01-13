using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class UserController : Controller
    {
        IUserServicecImpl userObject = new UserServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllUser = await userObject.GetUserList(authToken);
            return View("Index", getAllUser);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var userData = await userObject.GetUserById(id.ToString(), authToken);
                var castFromUser = new UserRegistration()
                {
                    UserName = userData.UserName,
                    First_Name = userData.First_Name,
                    Middle_Name = userData.Middle_Name,
                    Last_Name = userData.Last_Name,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    IsActive = userData.IsActive,
                    CreatedDateAndTime = userData.CreatedDateAndTime
                };
                return View("AddOrEditUser", castFromUser);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditUser(UserRegistration user, string id)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (string.IsNullOrEmpty(id))
                {
                    user.IsActive = true;
                    var userData = await userObject.AddUserDetailsAsync(user, authToken);
                }
                else
                {
                    user.Id = id;
                    var result = await userObject.EditUserDetailsAsync(user, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveUser(string userId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await userObject.DeleteUser(userId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove User." });
            }
        }

        public async Task<ActionResult> UserDetails(string userId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var userData = await userObject.GetUserById(userId, authToken);
            return PartialView("_UserDetail", new UserRegistration()
            {
                Id = userId,
                UserName = userData.UserName,
                First_Name = userData.First_Name,
                Middle_Name = userData.Middle_Name,
                Last_Name = userData.Last_Name,
                Email = userData.Email,
                PhoneNumber = userData.PhoneNumber,
                IsActive = userData.IsActive,
            });
        }
    }
}
