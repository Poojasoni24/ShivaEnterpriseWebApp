﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers.Products
{
    public class ProductController : Controller
    {
        IProductServiceImpl productService = new ProductServiceImpl();
        IProductCategoryServiceImpl productcategoryService = new ProductCategoryServiceImpl();
        IProductGroupServiceImpl productgroupService = new ProductGroupServiceImpl();
        IProductTypeServiceImpl productTypeService = new ProductTypeServiceImpl();
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
            List<ProductGroup> productGroupDataList = await productgroupService.GetProductGroupList(authToken);
            SelectList groupselectList = new SelectList(productGroupDataList, "ProductGroupId", "ProductGroupName");
            ViewBag.productGroupSelectList = groupselectList;

            List<ProductType> productTypeDataList = await productTypeService.GetProductTypeList(authToken);
            SelectList typeselectList = new SelectList(productTypeDataList, "ProductTypeId", "ProductTypeName");
            ViewBag.productTypeList = typeselectList;

            List<ProductCategory> productcategoryDataList = await productcategoryService.GetProductCategoryList(authToken);
            SelectList categoryselectList = new SelectList(productcategoryDataList, "ProductCategoryId", "ProductCategoryName");
            ViewBag.productCategoryList = categoryselectList;
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
                    product.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    product.ModifiedDateTime = DateTime.Now;
                    await productService.EditProductDetailsAsync(product, authToken);
                }
                else
                {
                    product.IsActive = true;
                    product.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    product.CreatedDateTime = DateTime.Now;
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
                IsActive = productData.IsActive,
                ProductImage = productData.ProductImage,
                ProductCategoryId = productData.ProductCategoryId,
                ProductGroupId = productData.ProductGroupId,
                ProductsTypeId = productData.ProductsTypeId,
            });
        }
    }
}
