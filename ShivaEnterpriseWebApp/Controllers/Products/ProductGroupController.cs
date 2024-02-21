using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Products
{
    public class ProductGroupController : Controller
    {
        IProductGroupServiceImpl productGroupService = new ProductGroupServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllProductGroup = await productGroupService.GetProductGroupList(authToken);
            return View("Index", getAllProductGroup);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditProductGroup(string productGroupId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(productGroupId))
            {
                var ProductGroupDetail = await productGroupService.GetProductGroupById(productGroupId, authToken);
                if (ProductGroupDetail != null)
                {
                    return View("AddOrEditProductGroup", ProductGroupDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditProductGroup(Guid productGroupId, ProductGroup productGroup)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (productGroupId != Guid.Empty)
                {
                    productGroup.ProductGroupId = productGroupId;
                    productGroup.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    productGroup.ModifiedDateTime = DateTime.Now;
                    await productGroupService.EditProductGroupDetailsAsync(productGroup, authToken);
                }
                else
                {
                    productGroup.IsActive = true;
                    productGroup.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    productGroup.CreatedDateTime = DateTime.Now;
                    await productGroupService.AddProductGroupDetailsAsync(productGroup, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveProductGroup(string productGroupId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await productGroupService.DeleteProductGroup(productGroupId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove productGroup." });
            }
        }

        public async Task<ActionResult> ProductGroupDetail(string productGroupId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var productGroupData = await productGroupService.GetProductGroupById(productGroupId, authToken);
            return PartialView("_productGroupDetail", new ProductGroup()
            {
                ProductGroupId = new Guid(productGroupId),
                ProductGroupCode = productGroupData.ProductGroupCode,
                ProductGroupName = productGroupData.ProductGroupName,
                ProductGroupDescription = productGroupData.ProductGroupDescription,
                IsActive = productGroupData.IsActive
            });
        }
    }
}
