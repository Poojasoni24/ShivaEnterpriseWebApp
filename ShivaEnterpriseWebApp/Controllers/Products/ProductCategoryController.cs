using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Products
{
    public class ProductCategoryController : Controller
    {
        IProductCategoryServiceImpl productCategoryService = new ProductCategoryServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllProductCategory = await productCategoryService.GetProductCategoryList(authToken);
            return View("Index", getAllProductCategory);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditProductCategory(string productCategoryId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(productCategoryId))
            {
                var ProductCategoryDetail = await productCategoryService.GetProductCategoryById(productCategoryId, authToken);
                if (ProductCategoryDetail != null)
                {
                    return View("AddOrEditProductCategory", ProductCategoryDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditProductCategory(string productId, ProductCategory productCategory)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(productId))
                {
                    productCategory.ProductCategoryId = new Guid(productId);
                    productCategory.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    productCategory.ModifiedDateTime = DateTime.Now;
                    await productCategoryService.EditProductCategoryDetailsAsync(productCategory, authToken);
                }
                else
                {
                    productCategory.IsActive = true;
                    productCategory.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    productCategory.CreatedDateTime = DateTime.Now;
                    await productCategoryService.AddProductCategoryDetailsAsync(productCategory, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveProductCatgory(string productCategoryId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await productCategoryService.DeleteProductCategory(productCategoryId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove productCategory." });
            }
        }

        public async Task<ActionResult> ProductCategoryDetail(string productCategoryId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var productCategoryData = await productCategoryService.GetProductCategoryById(productCategoryId, authToken);
            return PartialView("_productCategoryDetail", new ProductCategory()
            {
                ProductCategoryId = new Guid(productCategoryId),
                ProductCategoryCode = productCategoryData.ProductCategoryCode,
                ProductCategoryName = productCategoryData.ProductCategoryName,
                ProductCategoryDescription = productCategoryData.ProductCategoryDescription,
                IsActive = productCategoryData.IsActive
            });
        }
    }
}
