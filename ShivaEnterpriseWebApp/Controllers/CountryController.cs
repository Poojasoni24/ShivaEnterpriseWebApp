using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Countries
{
    public class CountryController : Controller
    {
        ICountryServiceImpl countryObj = new CountryServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var countryDetails = await countryObj.GetCountryList(authToken);
            return View("Index", countryDetails);
        }

        [HttpGet]
        public async Task<ActionResult> CountryDetail(string countryId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var countryDetails = await countryObj.GetCountryById(countryId, authToken);
            return PartialView("_CountryDetail", new Country()
            {
                Country_Id = countryDetails.Country_Id,
                Country_Code = countryDetails.Country_Code,
                Country_Name = countryDetails.Country_Name,
                CreatedDateAndTime = countryDetails.CreatedDateAndTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCountry(string countryId)
        {
            if (!string.IsNullOrEmpty(countryId))
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var countryDetails = await countryObj.GetCountryById(countryId, authToken);
                return View(countryDetails);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCountry(Country country)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            await countryObj.AddCountryDetailsAsync(country, authToken);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveCountry(string countryId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await countryObj.DeleteCountry(countryId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Country." });
            }
        }
    }
}
