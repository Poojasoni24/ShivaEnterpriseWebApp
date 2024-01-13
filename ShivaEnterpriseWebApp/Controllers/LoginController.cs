using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Helper;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Diagnostics;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        readonly ILoginServiceImpl loginServiceImpl = new LoginServiceImpl();

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string ReturnUrl)
        {
            string authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            if (authToken != null)
            {
                long expDateTime = long.Parse(TokenReader.GetExpDateTimeFromAuthToken(authToken));
                DateTimeOffset expDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expDateTime);

                if (expDateTimeOffset >= DateTimeOffset.UtcNow)
                {
                    if (ReturnUrl == null)
                    {
                        return RedirectToAction("Index", "Company");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }

            }

            LoginModel loginModel = new LoginModel();

            loginModel.ReturnUrlValue = ReturnUrl;

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            { //checking model state

                try
                {
                    var authDao = await loginServiceImpl.PerformLogin(loginModel);

                    if (authDao.Token != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Hash, authDao.Token)
                        };

                        foreach (var role in authDao.Roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var userDetails = await loginServiceImpl.GetUserdetail(loginModel.Username);
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        if (userDetails != null && userDetails.UserName != null)
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userDetails.UserName));
                        }


                        long expDateTime = long.Parse(TokenReader.GetExpDateTimeFromAuthToken(authDao.Token));
                        DateTimeOffset expDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expDateTime);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = expDateTimeOffset,
                        };

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                        if (loginModel.ReturnUrlValue == null)
                        {
                            return RedirectToAction("Index", "Company");
                        }
                        else
                        {
                            return Redirect(loginModel.ReturnUrlValue);
                        }
                    }
                    else
                    {
                        @ViewBag.ErrorMessage = "Invalid User Name or Password";
                        return View(loginModel);
                    }
                }
                catch (Exception e)
                {
                    @ViewBag.ErrorMessage = e.Message;
                    return View(loginModel);
                }
            }
            return View(loginModel);
        }
    }
}
