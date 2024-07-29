using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class SalesReturnController : Controller
    {
        ISalesReturnServiceImpl salesReturnService = new SalesReturnServiceImpl();
        ISalesOrderServiceImpl salesOrderService = new SalesOrderServiceImpl();
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
        ISalesReturnDetailServiceImpl SalesReturnDetailService = new SalesReturnDetailServiceImpl();
        private readonly IHostingEnvironment _hostingEnv;

        public SalesReturnController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllSalesReturns = await salesReturnService.GetSalesReturnList(authToken);
            if (getAllSalesReturns != null && getAllSalesReturns.Count > 0)
            {
                foreach (var item in getAllSalesReturns)
                {
                    item.SalesOrder = await salesOrderService.GetSalesOrderById(item.SalesOrderID, authToken);
                }
            }
            ViewBag.SalesOrders = ChangeIndex();
            return View("Index", getAllSalesReturns);
        }

        public async Task<JsonResult> selectSBU(Guid selectedId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllCustomer = await customerService.GetCustomerById(selectedId, authToken);
            //SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerDiscount");
            //ViewBag.customerSelectList = customerselectList;
            return Json(getAllCustomer.CustomerDiscount);
        }
        private static List<SalesReturn> ChangeIndex()
        {
            List<SalesReturn> salesReturns = new List<SalesReturn>();
            // Populate with sample data if needed
            return salesReturns;
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditSalesReturn(Guid salesReturnId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<SalesOrder> salesOrderDataList = await salesOrderService.GetSalesOrderList(authToken);
            SelectList salesOrderSelectList = new SelectList(salesOrderDataList, "SalesOrderID", "OrderDate");
            ViewBag.salesOrderSelectList = salesOrderSelectList;
            List<Product> productDataList = await productService.GetProductList(authToken);
            SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productgroupselectList;

            List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            ViewBag.BrandSelectList = brandgroupselectList;
            if (salesReturnId != Guid.Empty)
            {
                var salesReturnDetail = await salesReturnService.GetSalesReturnById(salesReturnId, authToken);
                if (salesReturnDetail != null)
                {
                    return View("AddOrEditSalesReturn", salesReturnDetail);
                }
            }
            return View("AddOrEditSalesReturn");
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditSalesReturn(Guid salesReturnId, [FromBody] SalesReturn salesReturn)
        {
            try
            {
                List<SalesReturnDetail> soDetailList = new List<SalesReturnDetail>();
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                List<SalesOrder> salesOrderDataList = await salesOrderService.GetSalesOrderList(authToken);
                SelectList salesOrderSelectList = new SelectList(salesOrderDataList, "SalesOrderID", "OrderDate");
                ViewBag.salesOrderSelectList = salesOrderSelectList;

                if (salesReturnId == null || salesReturnId == Guid.Empty)
                {
                    await salesReturnService.EditSalesReturnDetailsAsync(salesReturn, authToken);
                }
                else
                {
                    var isSuccess = await salesReturnService.AddSalesReturnDetailsAsync(salesReturn, authToken);
                    if (!isSuccess.success)
                    {
                        return View("AddOrEditSalesReturn");
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
        public async Task<IActionResult> DeleteSalesReturn(Guid salesReturnId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var isSuccess = await salesReturnService.DeleteSalesReturn(salesReturnId, authToken);
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
    }
}
