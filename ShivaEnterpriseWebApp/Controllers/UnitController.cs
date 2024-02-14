using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class UnitController : Controller
    {
        IUnitServiceImpl unitObj = new UnitServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var unitDetail = await unitObj.GetUnitList(authToken);
            return View("Index", unitDetail);
        }

        [HttpGet]
        public async Task<ActionResult> UnitDetail(string unitId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var unitDetail = await unitObj.GetUnitById(unitId, authToken);
            return PartialView("_UnitDetail", new Unit()
            {
                UnitId = unitDetail.UnitId,
                UnitCode = unitDetail.UnitCode,
                UnitName = unitDetail.UnitName,
                UnitDescription = unitDetail.UnitDescription,
                IsActive = unitDetail.IsActive,
                CreatedDateTime = unitDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditUnit(string unitId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (!string.IsNullOrEmpty(unitId))
            {
                var unitDetail = await unitObj.GetUnitById(unitId, authToken);
                return View(unitDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditUnit(Unit unit)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (unit.UnitId != Guid.Empty)
            {
                unit.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                unit.ModifiedDateTime = DateTime.Now;
                await unitObj.EditUnitDetailsAsync(unit, authToken);
            }
            else
            {
                unit.IsActive = true;
                unit.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                unit.CreatedDateTime = DateTime.Now;
                await unitObj.AddUnitDetailsAsync(unit, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveUnit(string unitId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await unitObj.DeleteUnit(unitId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Bank." });
            }
        }
    }
}

