using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class BrandController : Controller
    {
        IBrandServiceImpl brandObj = new BrandServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var brandDetail = await brandObj.GetBrandList(authToken);
            return View("Index", brandDetail);
        }

        [HttpGet]
        public async Task<ActionResult> BrandDetail(Guid brandId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var brandDetail = await brandObj.GetBrandById(brandId, authToken);
            return PartialView("_BrandDetail", new Brand()
            {
                BrandId = brandDetail.BrandId,
                BrandCode = brandDetail.BrandCode,
                BrandName = brandDetail.BrandName,
                BrandDescription = brandDetail.BrandDescription,
                IsActive = brandDetail.IsActive,
                CreatedDateTime = brandDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditBrand(Guid brandId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (brandId!= Guid.Empty)
            {
                var brandDetail = await brandObj.GetBrandById(brandId, authToken);
                return View(brandDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditBrand(Brand brand)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (brand.BrandId != Guid.Empty)
            {
                brand.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                brand.ModifiedDateTime = DateTime.Now;
                await brandObj.EditBrandDetailsAsync(brand, authToken);
            }
            else
            {
                brand.IsActive = true;
                brand.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                brand.CreatedDateTime = DateTime.Now;
                await brandObj.AddBrandDetailsAsync(brand, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBrand(Guid brandId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await brandObj.DeleteBrand(brandId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Brand." });
            }
        }
    }
}

