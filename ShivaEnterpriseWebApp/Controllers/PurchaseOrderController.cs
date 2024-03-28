using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Data.Common;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Nodes;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class PurchaseOrderController : Controller
    {
        IVendorServiceImpl vendorService = new VendorServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
        IPurchaseOrderServiceImpl purchaseorderService = new PurchaseOrderServiceImpl();
        IPurchaseOrderDetailServiceImpl purchaseorderDetailService = new PurchaseOrderDetailServiceImpl();


        private readonly IHostingEnvironment _hostingEnv;

        public PurchaseOrderController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllPurchaseOrder = await purchaseorderService.GetPurchaseOrderList(authToken);
            if (getAllPurchaseOrder != null && getAllPurchaseOrder.Count > 0)
            {
                foreach (var item in getAllPurchaseOrder)
                {
                    item.Vendor = await vendorService.GetVendorById(item.VendorID, authToken);
                }
            }

            return View("Index", getAllPurchaseOrder);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditPurchaseOrder(Guid purchaseorderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Vendor> vendorDataList = await vendorService.GetVendorList(authToken);
            SelectList vendorgroupselectList = new SelectList(vendorDataList, "VendorId", "VendorName");
            ViewBag.vendorSelectList = vendorgroupselectList;

            List<Product> productDataList = await productService.GetProductList(authToken);
            SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productgroupselectList;

            List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            ViewBag.BrandSelectList = brandgroupselectList;

            if (purchaseorderId != Guid.Empty)
            {
                var PurchaseOrderDetail = await purchaseorderService.GetPurchaseOrderById(purchaseorderId, authToken);
                if (PurchaseOrderDetail != null)
                {
                    return View("AddOrEditPurchaseOrder", PurchaseOrderDetail);
                }
            }
            return View("AddOrEditPurchaseOrder");
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditPurchaseOrder(string purchaseorderId, PurchaseOrderViewModel purchaseorderVM, [FromBody] List<PurchaseOrderDetail> Podetails)
        {
            try
            {

                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                List<Vendor> vendorDataList = await vendorService.GetVendorList(authToken);
                SelectList vendorgroupselectList = new SelectList(vendorDataList, "VendorId", "VendorName");
                ViewBag.vendorSelectList = vendorgroupselectList;

                List<Product> productDataList = await productService.GetProductList(authToken);
                SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;

                List<Brand> brandDataList = await brandService.GetBrandList(authToken);
                SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
                ViewBag.BrandSelectList = brandgroupselectList;
                if (!string.IsNullOrEmpty(purchaseorderId))
                {
                    //purchaseorderVM..PurchaseOrderId = new Guid(purchaseorderId);
                    //purchaseorderVM.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    //purchaseorderVM.ModifiedDateTime = DateTime.Now;
                    //await purchaseorderService.EditPurchaseOrderDetailsAsync(purchaseorderVM, authToken);
                }
                else
                {
                    //var netPriceIncludingTax = purchaseorderVM.PODetail.NetTotal * (purchaseorderVM.PODetail.Tax_Percentage / 100);
                    //purchaseorderVM.PurchaseOrder.TotalAmount = purchaseorderVM.PODetail.NetTotal + netPriceIncludingTax;
                    purchaseorderVM.PurchaseOrder.PurchaseOrderStatus = "Approve";
                    purchaseorderVM.PurchaseOrder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseorderVM.PurchaseOrder.CreatedDateTime = DateTime.Now;
                    purchaseorderVM.PurchaseOrder.Vendor = await vendorService.GetVendorById(purchaseorderVM.PurchaseOrder.VendorID, authToken);

                    var issuccess = await purchaseorderService.AddPurchaseOrderDetailsAsync(purchaseorderVM.PurchaseOrder, authToken);
                    if (issuccess.success)
                    {
                        foreach (var item in Podetails)
                        {
                            item.PurchaseOrderId = JsonConvert.DeserializeObject<Guid>(issuccess.value);
                            item.Product = await productService.GetProductById(item.ProductId, authToken);
                            item.Brand = await brandService.GetBrandById(item.BrandId, authToken);
                            item.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                            item.CreatedDateTime = DateTime.Now;
                           // await purchaseorderDetailService.AddPurchaseOrderDetailDetailsAsync(purchaseorderVM.PODetail, authToken);
                        }
                    }
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemovePurchaseOrder(string purchaseorderId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await purchaseorderService.DeletePurchaseOrder(purchaseorderId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove PurchaseOrder." });
            }
        }

        [HttpGet]
        public async Task<ActionResult> PurchaseOrderDetail(Guid purchaseorderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var purchaseorderData = await purchaseorderService.GetPurchaseOrderById(purchaseorderId, authToken);
            var purchaseorderDetailData = purchaseorderDetailService.GetPurchaseOrderDetailList(authToken).Result.FirstOrDefault(x => x.PurchaseOrderId == purchaseorderId);
            return PartialView("_purchaseorderview", new PurchaseOrderViewModel()
            {
                PurchaseOrder = new PurchaseOrder()
                {
                    PurchaseOrderId = purchaseorderId,
                    Doc_No = purchaseorderData.Doc_No,
                    Vendor = await vendorService.GetVendorById(purchaseorderData.VendorID, authToken),
                    OrderDate = purchaseorderData.OrderDate,
                    DeliveryDate = purchaseorderData.DeliveryDate,
                    TotalAmount = purchaseorderData.TotalAmount,
                    PurchaseOrderStatus = purchaseorderData.PurchaseOrderStatus,
                },
                //PODetail = new PurchaseOrderDetail()
                //{
                //    Product = await productService.GetProductById(purchaseorderDetailData.ProductId, authToken),
                //    Brand = await brandService.GetBrandById(purchaseorderDetailData.BrandId, authToken),
                //    Quantity = purchaseorderDetailData.Quantity,
                //    UnitPrice = purchaseorderDetailData.UnitPrice,
                //    Discount = purchaseorderDetailData.Discount,
                //    Tax_Percentage = purchaseorderDetailData.Tax_Percentage,
                //    NetTotal = purchaseorderDetailData.NetTotal,
                //}

            });
        }

        [HttpPost]
        public JsonResult InsertPodetail([FromBody] List<PurchaseOrderDetail> Podetails)
        {
            return Json(Podetails);
        }

    }
}
    


