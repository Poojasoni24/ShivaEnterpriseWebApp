using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
        ISalesOrderDetailServiceImpl salesorderDetailService = new SalesOrderDetailServiceImpl();
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

        public async Task<JsonResult> selectSBU(Guid selectedId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllCustomer = await customerService.GetCustomerById(selectedId, authToken);
            //SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerDiscount");
            //ViewBag.customerSelectList = customerselectList;
            return Json(getAllCustomer.CustomerDiscount);
        }
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
    
            List<Product> productDataList = await productService.GetProductList(authToken);
            SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productgroupselectList;

            List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            ViewBag.BrandSelectList = brandgroupselectList;

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

        [HttpPost]
        public async Task<ActionResult> AddOrEditSalesOrder(string salesorderId, [FromBody] SalesOrderViewModel SalesOrderViewModel)
        {
            try
            {
                List<SalesOrderDetail> soDetailList = new List<SalesOrderDetail>();
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                List<Customer> customerDataList = await customerService.GetCustomerList(authToken);
                SelectList customergroupselectList = new SelectList(customerDataList, "CustomerId", "CustomerName");
                ViewBag.customerSelectList = customergroupselectList;

                List<Product> productDataList = await productService.GetProductList(authToken);
                SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;

                List<Brand> brandDataList = await brandService.GetBrandList(authToken);
                SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
                ViewBag.BrandSelectList = brandgroupselectList;
                if (!string.IsNullOrEmpty(salesorderId))
                {
                    //purchaseorderVM..PurchaseOrderId = new Guid(purchaseorderId);
                    //purchaseorderVM.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    //purchaseorderVM.ModifiedDateTime = DateTime.Now;
                    //await purchaseorderService.EditPurchaseOrderDetailsAsync(purchaseorderVM, authToken);
                }
                else
                {

                    var netTotalAmount = SalesOrderViewModel.SODetail.Sum(x => x.NetTotal);
                    SalesOrderViewModel.SalesOrder.TotalAmount = netTotalAmount + (netTotalAmount * SalesOrderViewModel.SalesOrder.Tax_Percentage / 100);
                    SalesOrderViewModel.SalesOrder.SalesOrderStatus = "Approve";
                    SalesOrderViewModel.SalesOrder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    SalesOrderViewModel.SalesOrder.CreatedDateTime = DateTime.Now;
                    SalesOrderViewModel.SalesOrder.Customer = await customerService.GetCustomerById(SalesOrderViewModel.SalesOrder.CustomerId, authToken);

                    var issuccess = await salesorderService.AddSalesOrderDetailsAsync(SalesOrderViewModel.SalesOrder, authToken);
                    if (issuccess.success)
                    {
                        SalesOrderViewModel.SODetail.ForEach(
                            x =>
                            {
                               // x.SalesOrderId = JsonConvert.DeserializeObject<Guid>(issuccess.value);
                                x.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                x.CreatedDateTime = DateTime.Now;
                                x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                                x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                            });

                        await salesorderDetailService.AddSalesOrderDetailDetailsAsync(SalesOrderViewModel.SODetail, authToken);
                    }
                }
                

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View("Index");
            }

        }
    }
}
