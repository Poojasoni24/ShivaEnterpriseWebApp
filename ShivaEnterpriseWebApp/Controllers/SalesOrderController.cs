using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Data.Common;
using System.Net;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class SalesOrderController : Controller
    {
        ISalesOrderServiceImpl salesorderService = new SalesOrderServiceImpl();
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        private readonly IHostingEnvironment _hostingEnv;

        public SalesOrderController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllSalesOrder = await salesorderService.GetSalesOrderList(authToken);
            if (getAllSalesOrder != null && getAllSalesOrder.Count > 0)
            {
                foreach (var item in getAllSalesOrder)
                {
                    item.Customer = await customerService.GetCustomerById(item.CustomerId, authToken);
                }
            }
            ViewBag.Customer = ChangeIndex();
            return View("Index", getAllSalesOrder);
        }
        //public async Task<JsonResult> selectSBU(string id)
        //{
        //    string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //    var getAllCustomer = await customerService.GetCustomerById(authToken);
        //    //SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerDiscount");
        //   ViewBag.customerSelectList = customerselectList;
        //    return Json(id);
        //}
       private static List<SalesOrder> ChangeIndex()
        {
            List<SalesOrder> Customer = new List<SalesOrder>();
            //branches.Add(new BranchModel { Branchcode = "1", BranchName = "Branch 1" });
            //branches.Add(new BranchModel { Branchcode = "2", BranchName = "Branch 2" });
            //branches.Add(new BranchModel { Branchcode = "3", BranchName = "Branch 3" });
            //branches.Add(new BranchModel { Branchcode = "4", BranchName = "Branch 4" });
            //branches.Add(new BranchModel { Branchcode = "5", BranchName = "Branch 5" });
            return Customer;
        }
        [HttpGet]
        public async Task<ActionResult> AddOrEditSalesOrder(Guid salesorderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Customer> customerDataList = await customerService.GetCustomerList(authToken);
            SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerName");
            ViewBag.customerSelectList = customerselectList;

            //List<Product> productDataList = await productService.GetProductList(authToken);
            //SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            //ViewBag.ProductSelectList = productgroupselectList;

            //List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            //SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            //ViewBag.BrandSelectList = brandgroupselectList;

           // if (!string.IsNullOrEmpty(salesorderId))
                if (salesorderId != Guid.Empty)
                {
                var SalesOrderDetail = await salesorderService.GetSalesOrderById(salesorderId, authToken);
                if (SalesOrderDetail != null)
                {
                    return View("AddOrEditSalesOrder", SalesOrderDetail);
                }
            }
            return View("AddOrEditSalesOrder");
        }

    }
}
