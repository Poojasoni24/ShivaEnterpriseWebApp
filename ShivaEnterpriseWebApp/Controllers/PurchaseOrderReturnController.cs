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
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
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
                    //item.purchaseOrder = await purchaseOrderService.GetpurchaseOrderById(item.purchaseOrderID, authToken);
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

        //[HttpGet]
        //public async Task<ActionResult> AddOrEditpurchaseReturn(Guid purchaseReturnId)
        //{
        //    string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //    List<purchaseOrderReturn> purchaseOrderDataList = await purchaseOrRService.GetPurchaseOrderList(authToken);
        //    SelectList purchaseOrderSelectList = new SelectList(purchaseOrderDataList, "purchaseOrderID", "OrderDate");
        //    ViewBag.purchaseOrderSelectList = purchaseOrderSelectList;
        //    List<Product> productDataList = await productService.GetProductList(authToken);
        //    SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
        //    ViewBag.ProductSelectList = productgroupselectList;

        //    List<Brand> brandDataList = await brandService.GetBrandList(authToken);
        //    SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
        //    ViewBag.BrandSelectList = brandgroupselectList;
        //    if (purchaseReturnId != Guid.Empty)
        //    {
        //        var purchaseReturnDetail = await purchaseReturnService.GetpurchaseReturnById(purchaseReturnId, authToken);
        //        if (purchaseReturnDetail != null)
        //        {
        //            return View("AddOrEditpurchaseReturn", purchaseReturnDetail);
        //        }
        //    }
        //    return View("AddOrEditpurchaseReturn");
        //}

        //[HttpPost]
        //public async Task<ActionResult> AddOrEditpurchaseReturn(Guid purchaseReturnId, [FromBody] purchaseReturn purchaseReturn)
        //{
        //    try
        //    {
        //        List<purchaseReturnDetail> soDetailList = new List<purchaseReturnDetail>();
        //        string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //        List<purchaseOrder> purchaseOrderDataList = await purchaseOrderService.GetpurchaseOrderList(authToken);
        //        SelectList purchaseOrderSelectList = new SelectList(purchaseOrderDataList, "purchaseOrderID", "OrderDate");
        //        ViewBag.purchaseOrderSelectList = purchaseOrderSelectList;

        //        if (purchaseReturnId == null || purchaseReturnId == Guid.Empty)
        //        {
        //            await purchaseReturnService.EditpurchaseReturnDetailsAsync(purchaseReturn, authToken);
        //        }
        //        else
        //        {
        //            var isSuccess = await purchaseReturnService.AddpurchaseReturnDetailsAsync(purchaseReturn, authToken);
        //            if (!isSuccess.success)
        //            {
        //                return View("AddOrEditpurchaseReturn");
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Index");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeletepurchaseReturn(Guid purchaseReturnId)
        //{
        //    try
        //    {
        //        string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //        var isSuccess = await purchaseReturnService.DeletepurchaseReturn(purchaseReturnId, authToken);
        //        if (isSuccess.successs)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        else
        //        {
        //            return BadRequest(isSuccess.message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Index");
        //    }
        //}
    }
}
