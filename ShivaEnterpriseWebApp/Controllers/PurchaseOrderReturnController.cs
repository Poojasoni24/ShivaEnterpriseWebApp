using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class PurchaseOrderReturnController : Controller
    {
        IPurchaseOrderReturnServiceImpl purchaseReturnService = new PurchaseOrderReturnServiceImpl();
        IPurchaseOrderServiceImpl purchaseOrderService = new PurchaseOrderServiceImpl();
        IPurchaseOrderDetailServiceImpl purchaseOrderDetailService = new PurchaseOrderDetailServiceImpl();
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
        IVendorServiceImpl vendorService = new VendorServiceImpl();
        ICityServiceImpl cityService = new CityServiceImpl();
        IStateServiceImpl stateService = new StateServiceImpl();
        ICountryServiceImpl countryService = new CountryServiceImpl();
        private readonly IHostingEnvironment _hostingEnv;

        public PurchaseOrderReturnController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllpurchaseReturns = await purchaseReturnService.GetpurchaseReturnList(authToken);
            if (getAllpurchaseReturns != null && getAllpurchaseReturns.Count > 0)
            {
                foreach (var item in getAllpurchaseReturns)
                {
                    item.PurchaseOrder = await purchaseOrderService.GetPurchaseOrderById(item.PurchaseOrderId, authToken);
                    item.PurchaseOrder.Vendor = await vendorService.GetVendorById(item.PurchaseOrder.VendorID, authToken);
                }
            }
            //ViewBag.purchaseOrders = ChangeIndex();
            return View("Index", getAllpurchaseReturns);
        }

        //public async Task<JsonResult> selectSBU(Guid selectedId)
        //{
        //    string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //    var getAllCustomer = await customerService.GetCustomerById(selectedId, authToken);
        //    //SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerDiscount");
        //    //ViewBag.customerSelectList = customerselectList;
        //    return Json(getAllCustomer.CustomerDiscount);
        //}
        //private static List<purchaseReturn> ChangeIndex()
        //{
        //    List<purchaseReturn> purchaseReturns = new List<purchaseReturn>();
        //    // Populate with sample data if needed
        //    return purchaseReturns;
        //}

        [HttpGet]
        public async Task<ActionResult> AddOrEditPurchaseOrderReturn(Guid purchaseReturnId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            List<PurchaseOrder> purchaseOrderDataList = await purchaseOrderService.GetPurchaseOrderList(authToken);
            SelectList purchaseOrderSelectList = new SelectList(purchaseOrderDataList, "PurchaseOrderId", "Doc_No");
            ViewBag.PurchaseOrderSelectList = purchaseOrderSelectList;

            List<Vendor> vendorDataList = await vendorService.GetVendorList(authToken);
            SelectList vendorgroupselectList = new SelectList(vendorDataList, "VendorId", "VendorName");
            ViewBag.vendorSelectList = vendorgroupselectList;

            List<Product> productDataList = await productService.GetProductList(authToken);
            SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productgroupselectList;

            List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            ViewBag.BrandSelectList = brandgroupselectList;
            var purchaseReturnDetail = new PurchaseOrderReturn();
            if (purchaseReturnId != Guid.Empty)
            {
                purchaseReturnDetail = await purchaseReturnService.GetpurchaseReturnById(purchaseReturnId, authToken);

                PurchaseOrder purchaseOrder = await purchaseOrderService.GetPurchaseOrderById(purchaseReturnDetail.PurchaseOrderId,authToken);
                purchaseOrderDataList = new List<PurchaseOrder> { purchaseOrder };
                purchaseOrderSelectList = new SelectList(purchaseOrderDataList, "PurchaseOrderId", "Doc_No");
                ViewBag.PurchaseOrderSelectList = purchaseOrder;

                var vendor = await vendorService.GetVendorById(purchaseOrder.VendorID, authToken);
                vendorDataList = new List<Vendor> { vendor };
                vendorgroupselectList = new SelectList(vendorDataList, "VendorId", "VendorName");
                ViewBag.vendorSelectList = vendorgroupselectList;

                productDataList = await productService.getProdutFromPurchaseOrderId(purchaseReturnDetail.PurchaseOrderId, authToken);
                productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;

                var purchaseOrderDetails = await purchaseOrderDetailService.GetPurchaseOrderDetailsbyPurcahseOrderid(purchaseReturnDetail.PurchaseOrderId, authToken);
                purchaseReturnDetail.ProductId = productDataList[0].ProductId;
                
                var brand = brandService.GetBrandById(purchaseOrderDetails.BrandId, authToken).Result;
                brandDataList = new List<Brand> { brand };

                brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
                ViewBag.BrandSelectList = brandgroupselectList;
                purchaseReturnDetail.Quantity = (int)purchaseOrderDetails.Quantity;

                if (purchaseReturnDetail != null)
                {
                    return View("AddOrEditPurchaseOrderReturn", purchaseReturnDetail);
                }
            }
            return View("AddOrEditPurchaseOrderReturn", purchaseReturnDetail);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditPurchaseOrderReturn(PurchaseOrderReturn purchaseOrderReturn)
        {
            try
            {
                List<PurchaseReturnDetail> soDetailList = new List<PurchaseReturnDetail>();
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                List<PurchaseOrder> purchaseOrderDataList = await purchaseOrderService.GetPurchaseOrderList(authToken);
                SelectList purchaseOrderSelectList = new SelectList(purchaseOrderDataList, "purchaseOrderID", "OrderDate");
                ViewBag.purchaseOrderSelectList = purchaseOrderSelectList;

                if (purchaseOrderReturn.PurchaseReturnId == null || purchaseOrderReturn.PurchaseReturnId == Guid.Empty)
                {
                    purchaseOrderReturn.PurchaseReturnId = Guid.NewGuid();

                    purchaseOrderReturn.PurchaseOrder = await purchaseOrderService.GetPurchaseOrderById(purchaseOrderReturn.PurchaseOrderId, authToken); ;
                    purchaseOrderReturn.PurchaseOrder.Vendor = await vendorService.GetVendorById(purchaseOrderReturn.PurchaseOrder.VendorID, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City = await cityService.GetCityById(purchaseOrderReturn.PurchaseOrder.Vendor.cityId, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.ModifiedDateTime = DateTime.Now;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State = await stateService.GetStateById(purchaseOrderReturn.PurchaseOrder.Vendor.City.State_Id, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.Country = await countryService.GetCountryById(purchaseOrderReturn.PurchaseOrder.Vendor.City.State.Country_Id, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.country = purchaseOrderReturn.PurchaseOrder.Vendor.City.Country;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.ModifiedDateTime = DateTime.Now;
                    await purchaseReturnService.AddpurchaseReturnDetailsAsync(purchaseOrderReturn, authToken);

                }
                else
                {
                    purchaseOrderReturn.PurchaseOrder = await purchaseOrderService.GetPurchaseOrderById(purchaseOrderReturn.PurchaseOrderId, authToken); ;
                    purchaseOrderReturn.PurchaseOrder.Vendor = await vendorService.GetVendorById(purchaseOrderReturn.PurchaseOrder.VendorID, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City = await cityService.GetCityById(purchaseOrderReturn.PurchaseOrder.Vendor.cityId, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.ModifiedDateTime = DateTime.Now;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State = await stateService.GetStateById(purchaseOrderReturn.PurchaseOrder.Vendor.City.State_Id, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.Country = await countryService.GetCountryById(purchaseOrderReturn.PurchaseOrder.Vendor.City.State.Country_Id, authToken);
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.country = purchaseOrderReturn.PurchaseOrder.Vendor.City.Country;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseOrderReturn.PurchaseOrder.Vendor.City.State.ModifiedDateTime = DateTime.Now;
                    var isSuccess = await purchaseReturnService.EditpurchaseReturnDetailsAsync(purchaseOrderReturn, authToken);
                    if (!isSuccess.success)
                    {
                        return View("AddOrEditPurchaseOrderReturn", purchaseOrderReturn);
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
        public async Task<IActionResult> DeletePurchaseReturn(Guid purchaseReturnId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var isSuccess = await purchaseReturnService.DeletepurchaseReturn(purchaseReturnId, authToken);
                if (isSuccess.successs)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest(isSuccess.message);
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductAndQuantity(Guid purchaseOrderId)
        {
            if (purchaseOrderId == Guid.Empty)
            {
                return Json(new { error = "SalesOrderId is invalid" });
            }

            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var purchaseOrderDetails = await purchaseOrderDetailService.GetPurchaseOrderDetailsbyPurcahseOrderid(purchaseOrderId, authToken);
            var products = await productService.getProdutFromPurchaseOrderId(purchaseOrderDetails.PurchaseOrderId, authToken);
            //var productbyid = productService.GetProductById(purchaseOrderDetails.ProductId, authToken).Result;
            //List<Product> product = new List<Product> { productbyid };
            var brand = brandService.GetBrandById(purchaseOrderDetails.BrandId, authToken).Result;
            List<Brand> brands = new List<Brand> { brand };
            var currentQuantity = purchaseOrderDetails.Quantity;
            return Json(new { products, brands, currentQuantity });
        }
    }
}
