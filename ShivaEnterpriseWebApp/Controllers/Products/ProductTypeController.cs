using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Products
{
    public class ProductTypeController : Controller
    {
        IProductTypeServiceImpl productTypeService = new ProductTypeServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllProductType = await productTypeService.GetProductTypeList(authToken);
            return View("Index", getAllProductType);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditProductType(string productTypeId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(productTypeId))
            {
                var ProductTypeDetail = await productTypeService.GetProductTypeById(productTypeId, authToken);
                if (ProductTypeDetail != null)
                {
                    return View("AddOrEditProductType", ProductTypeDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditProductType(string productTypeId, ProductType productType)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(productTypeId))
                {
                    productType.ProductTypeId = new Guid(productTypeId);
                    await productTypeService.EditProductTypeDetailsAsync(productType, authToken);
                }
                else
                {
                    productType.ProductTypeStatus = true;
                    await productTypeService.AddProductTypeDetailsAsync(productType, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveProductCatgory(string productTypeId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await productTypeService.DeleteProductType(productTypeId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove productType." });
            }
        }

        public async Task<ActionResult> ProductTypeDetail(string productTypeId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var productTypeData = await productTypeService.GetProductTypeById(productTypeId, authToken);
            return PartialView("_productTypeDetail", new ProductType()
            {
                ProductTypeId = new Guid(productTypeId),
                ProductTypeCode = productTypeData.ProductTypeCode,
                ProductTypeName = productTypeData.ProductTypeName,
                ProductTypeDescription = productTypeData.ProductTypeDescription,
                ProductTypeStatus = productTypeData.ProductTypeStatus
            });
        }
    }
}
