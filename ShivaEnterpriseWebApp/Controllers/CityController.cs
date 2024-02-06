using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using System.Linq;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CityController : Controller
    {
        ICityServiceImpl cityObj = new CityServiceImpl();
        IStateServiceImpl stateObj = new StateServiceImpl();
        ICountryServiceImpl countryObj = new CountryServiceImpl();

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
            var statedetail = await stateObj.GetStateById(cityDetail.State_Id, authToken);
            return PartialView("_CityDetail", new City()
            {
                City_Id = cityDetail.City_Id,
                City_Code = cityDetail.City_Code,
                City_Name = cityDetail.City_Name,
                State = statedetail,
                Country = await countryObj.GetCountryById(statedetail.Country_Id,authToken),
                IsActive = cityDetail.IsActive,
                CreatedDateTime = cityDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCity(string cityId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            List<State>  stateDataList = await stateObj.GetStateList(authToken);
            SelectList selectList = new SelectList(stateDataList, "State_Id", "State_Name");
            ViewBag.SelectList = selectList;
            if (!string.IsNullOrEmpty(cityId))
            {               
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
            List<State> stateDataList = await stateObj.GetStateList(authToken);
            SelectList selectList = new SelectList(stateDataList, "State_Id", "State_Name");
            ViewBag.SelectList = selectList;
            if (!string.IsNullOrEmpty(city.City_Id))
            {
                city.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                city.ModifiedDateTime = DateTime.Now;
                await cityObj.EditCityDetailsAsync(city,authToken);
            }
            else
            {
                city.IsActive = true;
                city.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                city.CreatedDateTime = DateTime.Now;
                await cityObj.AddCityDetailsAsync(city, authToken);
            }
            
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
