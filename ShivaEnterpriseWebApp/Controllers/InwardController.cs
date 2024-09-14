using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class InwardController : Controller
    {
        IInwardServiceImpl _inwardService = new InwardServiceImpl();
        IProductServiceImpl _productService = new ProductServiceImpl();
        IVendorServiceImpl _vendorService = new VendorServiceImpl();
        IPurchaseOrderDetailServiceImpl _purchaseOrderDetailService = new PurchaseOrderDetailServiceImpl();
        IPurchaseOrderServiceImpl _purchaseOrderService = new PurchaseOrderServiceImpl();
        ICityServiceImpl CityService = new CityServiceImpl();
        IHostingEnvironment _hostingEnv;

        public InwardController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllInwards = await _inwardService.GetInwardList(authToken);
            if (getAllInwards != null && getAllInwards.Count > 0)
            {
                foreach (var item in getAllInwards)
                {
                    Product product  = await _productService.GetProductById(item.ProductId, authToken);
                    item.ProductName = product.ProductName;
                 
                    Vendor vendor = await _vendorService.GetVendorById(item.VendorId, authToken);
                    item.VendorName = vendor.VendorName;
                }
            }
            return View("Index", getAllInwards);
        }


        [HttpGet]
        public async Task<ActionResult> AddOrEditInward(Guid inwardId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                List<Product> productDataList = await _productService.GetProductList(authToken);
                SelectList productDropdownList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productDropdownList;

                List<Vendor> vendorDataList = await _vendorService.GetVendorList(authToken);
                SelectList supplierDropdownList = new SelectList(vendorDataList, "VendorId", "VendorName");
                ViewBag.SupplierSelectList = supplierDropdownList;

                List<PurchaseOrder> purchaseOrdersDataList = await _purchaseOrderService.GetPurchaseOrderList(authToken);
                SelectList purchaseOrderDropdownList = new SelectList(purchaseOrdersDataList, "PurchaseOrderId", "Doc_No");
                ViewBag.PurchaseOrderSelectList = purchaseOrderDropdownList;

                if (inwardId != Guid.Empty)
                {
                    var inwardDetail = await _inwardService.GetInwardById(inwardId, authToken);
                    productDataList = new List<Product>();
                    productDataList.Add(await _inwardService.GetProductByPurchseOrderId(inwardDetail.ProductId, authToken));
                    productDropdownList = new SelectList(productDataList, "ProductId", "ProductName");

                    ViewBag.ProductSelectList = productDropdownList;

                    if (inwardDetail != null)
                    {
                        return View("AddOrEditInward", inwardDetail);
                    }
                }
                return View("AddOrEditInward", new Inward());
            }
            catch (Exception ex)
            {
                return View("Index");
            }
  
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditInward(Inward inwardDetails)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                PurchaseOrder purchaseOrder = await _purchaseOrderService.GetPurchaseOrderById(inwardDetails.PurchaseOrderId, authToken);
                Vendor vendor = await _vendorService.GetVendorById(inwardDetails.VendorId, authToken);
                Product product = await _productService.GetProductById(inwardDetails.ProductId, authToken);
                inwardDetails.VendorName = vendor.VendorName;
                inwardDetails.ProductName = product.ProductName;
             
                if (inwardDetails.InwardId != Guid.Empty)
                {
                    //inwardDetails.Product = product;
                    //inwardDetails.Vendor = vendor;
                    //inwardDetails.PurchaseOrder = purchaseOrder;
                    //inwardDetails.PurchaseOrder.Vendor = inwardDetails.Vendor;

                    await _inwardService.EditInwardDetailsAsync(inwardDetails, authToken);
                }
                else
                { 
                    inwardDetails.InwardId = Guid.NewGuid();
                    inwardDetails.CreatedDate = DateTime.Now;
                    //inwardDetails.Product = product;
                    //inwardDetails.Vendor = vendor;
                    //inwardDetails.Vendor.City = await CityService.GetCityById(vendor.cityId, authToken);
                    //inwardDetails.PurchaseOrder = purchaseOrder;
                    //inwardDetails.PurchaseOrder.Vendor = inwardDetails.Vendor;


                    await _inwardService.AddInwardDetailsAsync(inwardDetails, authToken);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log exception
                return View("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> RemoveInward(Guid inwardId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                await _inwardService.DeleteInward(inwardId, authToken);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Outward details." });
            }
        }
    }
}
