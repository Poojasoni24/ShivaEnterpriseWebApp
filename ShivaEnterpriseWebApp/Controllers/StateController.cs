using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class StateController : Controller
    {
        IStateServiceImpl stateObj = new StateServiceImpl();
        ICountryServiceImpl countryObj = new CountryServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var getAllStateDetails = await stateObj.GetStateList(authToken);
            foreach (var item in getAllStateDetails)
            {
                item.country = await countryObj.GetCountryById(item.Country_Id, authToken);
            }
            return View("Index", getAllStateDetails);
        }

        [HttpGet]
        public async Task<ActionResult> stateDetail(string stateId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var stateDetails = await stateObj.GetStateById(stateId, authToken);
            return PartialView("_StateDetail", new State()
            {
                State_Id = stateDetails.State_Id,
                State_Code = stateDetails.State_Code,
                State_Name = stateDetails.State_Name,
                country = await countryObj.GetCountryById(stateDetails.Country_Id.ToString(),authToken),
                CreatedDateTime = stateDetails.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditState(string stateId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            List<Country> countryDataList = await countryObj.GetCountryList(authToken);
            SelectList selectList = new SelectList(countryDataList, "Country_Id", "Country_Name");
            ViewBag.SelectList = selectList;
            if (!string.IsNullOrEmpty(stateId))
            {               
                var stateDetails = await stateObj.GetStateById(stateId, authToken);
                return View(stateDetails);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditState(State state)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            List<Country> countryDataList = await countryObj.GetCountryList(authToken);
            SelectList selectList = new SelectList(countryDataList, "Country_Id", "Country_Name");
            ViewBag.SelectList = selectList;
            if (!string.IsNullOrEmpty(state.State_Id))
            {
                state.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                state.ModifiedDateTime = DateTime.Now;
                await stateObj.EditStateDetailsAsync(state, authToken);
            }
            else
            {
                state.IsActive = true;
                state.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                state.CreatedDateTime = DateTime.Now;
                await stateObj.AddStateDetailsAsync(state, authToken);
            }           
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveState(string stateId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await stateObj.DeleteState(stateId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove State." });
            }
        }
    }
}
