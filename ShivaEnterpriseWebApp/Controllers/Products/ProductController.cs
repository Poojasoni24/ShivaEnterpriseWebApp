using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Products
{
    public class ProductController : Controller
    {
        IProductServiceImpl productService = new ProductServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllProduct = await productService.GetProductList(authToken);
            return View("Index", getAllProduct);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditProduct(string productId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (!string.IsNullOrEmpty(productId))
            {
                var ProductDetail = await productService.GetProductById(productId, authToken);
                if (ProductDetail != null)
                {
                    return View("AddOrEditProduct", ProductDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditProduct(string productId, Product product)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(productId))
                {
                    product.ProductId = new Guid(productId);
                    await productService.EditProductDetailsAsync(product, authToken);
                }
                else
                {
                    product.ProductStatus = true;
                    await productService.AddProductDetailsAsync(product, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveProductCatgory(string productId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await productService.DeleteProduct(productId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove product." });
            }
        }

        public async Task<ActionResult> ProductDetail(string productId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var productData = await productService.GetProductById(productId, authToken);
            return PartialView("_productDetail", new Product()
            {
                ProductId = new Guid(productId),
                ProductCode = productData.ProductCode,
                ProductName = productData.ProductName,
                ProductDescription = productData.ProductDescription,
                ProductStatus = productData.ProductStatus,
                ProductImage = productData.ProductImage,
                ProductCategoryId = productData.ProductCategoryId,
                ProductGroupId = productData.ProductGroupId,
                ProductsTypeId = productData.ProductsTypeId,
            });
        }
    }
}
