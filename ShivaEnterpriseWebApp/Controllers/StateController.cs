using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class StateController : Controller
    {
        IStateServiceImpl stateObj = new StateServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var stateDetails = await stateObj.GetStateList(authToken);
            return View("Index", stateDetails);
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
                State_Name = stateDetails.State_Name,
                Country_Id = stateDetails.Country_Id,
                CreatedDateAndTime = stateDetails.CreatedDateAndTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditState(string stateId)
        {
            if (!string.IsNullOrEmpty(stateId))
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
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
            await stateObj.AddStateDetailsAsync(state, authToken);
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
