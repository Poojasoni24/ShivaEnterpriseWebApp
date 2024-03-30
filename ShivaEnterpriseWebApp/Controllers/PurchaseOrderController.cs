using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
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
                    PurchaseOrderDetail.Vendor = vendorDataList.Where(x => x.VendorId != null && x.VendorId == PurchaseOrderDetail.VendorID).FirstOrDefault();
                    var poDetailsByPOId = purchaseorderDetailService.GetPurchaseOrderDetailList(authToken).Result.Where(x=>x.PurchaseOrderId == purchaseorderId).ToList();
                    poDetailsByPOId.ForEach(x =>
                    {
                        x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                        x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                    });
                    var data = new PurchaseOrderViewModel()
                    { 
                        PurchaseOrder = PurchaseOrderDetail,
                        PODetail = poDetailsByPOId
                    };

                    return View("AddOrEditPurchaseOrder", data);
                }
            }
            return View("AddOrEditPurchaseOrder");
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditPurchaseOrder(string purchaseorderId, [FromBody] PurchaseOrderViewModel PurchaseOrderViewModel)
        {
            try
            {
                List<PurchaseOrderDetail> poDetailList = new List<PurchaseOrderDetail>();
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

                PurchaseOrderViewModel.PODetail = PurchaseOrderViewModel.PODetail.Where(item1 => !PurchaseOrderViewModel.UpdatedPODetail.Select(item2 => item2.PurchaseOrderDetailId).Contains(item1.PurchaseOrderDetailId)).ToList();
                if (!string.IsNullOrEmpty(purchaseorderId))
                {
                    PurchaseOrderViewModel.PurchaseOrder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    PurchaseOrderViewModel.PurchaseOrder.ModifiedDateTime = DateTime.Now;
                    var isSuccess = await purchaseorderService.EditPurchaseOrderDetailsAsync(PurchaseOrderViewModel.PurchaseOrder, authToken);
                    if (isSuccess.success)
                    {
                        PurchaseOrderViewModel.UpdatedPODetail.ForEach(
                            x =>
                            {
                                x.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                x.ModifiedDateTime = DateTime.Now;
                                x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                                x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                            });

                        await purchaseorderDetailService.EditPurchaseOrderDetailDetailsAsync(PurchaseOrderViewModel.UpdatedPODetail, authToken);
                    }
                }
                else
                {

                    var netTotalAmount = PurchaseOrderViewModel.PODetail.Sum(x => x.NetTotal);
                    PurchaseOrderViewModel.PurchaseOrder.TotalAmount = netTotalAmount + (netTotalAmount * PurchaseOrderViewModel.PurchaseOrder.Tax_Percentage / 100);
                    PurchaseOrderViewModel.PurchaseOrder.PurchaseOrderStatus = "Approve";
                    PurchaseOrderViewModel.PurchaseOrder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    PurchaseOrderViewModel.PurchaseOrder.CreatedDateTime = DateTime.Now;
                    PurchaseOrderViewModel.PurchaseOrder.Vendor = await vendorService.GetVendorById(PurchaseOrderViewModel.PurchaseOrder.VendorID, authToken);

                    var issuccess = await purchaseorderService.AddPurchaseOrderDetailsAsync(PurchaseOrderViewModel.PurchaseOrder, authToken);
                    if (issuccess.success)
                    {
                        PurchaseOrderViewModel.PODetail.ForEach(
                            x =>
                            {
                                x.PurchaseOrderId = JsonConvert.DeserializeObject<Guid>(issuccess.value);
                                x.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                x.CreatedDateTime = DateTime.Now;
                                x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                                x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                            });

                        await purchaseorderDetailService.AddPurchaseOrderDetailDetailsAsync(PurchaseOrderViewModel.PODetail, authToken);
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



