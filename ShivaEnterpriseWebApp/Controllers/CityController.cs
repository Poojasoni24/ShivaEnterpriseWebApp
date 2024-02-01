using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CityController : Controller
    {
        ICityServiceImpl cityObj = new CityServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var cityDetails = await cityObj.GetCityList(authToken);
            return View("Index", cityDetails);
        }

        [HttpGet]
        public async Task<ActionResult> CityDetail(string cityId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var cityDetail = await cityObj.GetCityById(cityId, authToken);
            return PartialView("_CityDetail", new City()
            {
                City_Id = cityDetail.City_Id,
                City_Name = cityDetail.City_Name,
                State_Id = cityDetail.State_Id,
                CreatedDateAndTime = cityDetail.CreatedDateAndTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCity(string cityId)
        {
            if (!string.IsNullOrEmpty(cityId))
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var cityDetail = await cityObj.GetCityById(cityId, authToken);
                return View(cityDetail);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCity(City city)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            await cityObj.AddCityDetailsAsync(city, authToken);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveCity(string cityId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await cityObj.DeleteCity(cityId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Country." });
            }
        }
    }
}
