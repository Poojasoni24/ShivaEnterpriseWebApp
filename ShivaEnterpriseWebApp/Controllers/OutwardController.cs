using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class OutwardController : Controller
    {
        IOutwardServiceImpl outwardService = new OutwardServiceImpl();
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        ISalesOrderServiceImpl saleorderService = new SalesOrderServiceImpl();

        public async Task<IActionResult> Index()
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var getAllOutwards = await outwardService.GetOutwardList(authToken);
                if (getAllOutwards != null && getAllOutwards.Count > 0)
                {
                    foreach (var item in getAllOutwards)
                    {
                        Customer custName = await customerService.GetCustomerById(item.CustomerId, authToken);
                        item.CustomerName = custName.CustomerName;
                        Product product = await productService.GetProductById(item.ProductId, authToken);
                        item.ProductName = product.ProductName;
                        //item.SalesOrder = await saleorderService.GetSalesOrderById(item.SalesOrderId, authToken);
                    }
                }
                ViewBag.Customer = new List<Outward>();
                return View("Index", getAllOutwards);
            }
            catch(Exception ex)
            {
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditOutward(Guid outwardId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                //List<Customer> customerDataList = await customerService.GetCustomerList(authToken);
                List<Customer> customerDataList = new List<Customer>();
                SelectList customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerName");
                ViewBag.customerSelectList = customerselectList;

                //List<Product> productDataList = await productService.GetProductList(authToken);
                List<Product> productDataList = new List<Product>();
                SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;

                List<SalesOrder> saleOrderDataList = await saleorderService.GetSalesOrderList(authToken);
                var saleOrders = GetSaleOrders(saleOrderDataList);
                ViewBag.SaleOrderSelectList = new SelectList(saleOrders, "SalesOrderId", "Doc_No");

                if (outwardId != Guid.Empty)
                {
                    var OutwardDetail = await outwardService.GetOutwardById(outwardId, authToken);
                    customerDataList = await outwardService.getCustomerFromSaleOrderId(OutwardDetail.SalesOrderId ,authToken);
                    customerselectList = new SelectList(customerDataList, "CustomerId", "CustomerName");
                    ViewBag.customerSelectList = customerselectList;

                    productDataList = await productService.getProdutFromSaleOrderId(OutwardDetail.SalesOrderId ,authToken);
                    productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                    ViewBag.ProductSelectList = productgroupselectList;

                    if (OutwardDetail != null)
                    {
                        return View("AddOrEditOutward", OutwardDetail);
                    }
                }
                return View("AddOrEditOutward", new Outward());
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        private List<SalesOrder> GetSaleOrders(List<SalesOrder> so)
        {
            // Replace with your actual logic to fetch sale orders
            List<SalesOrder> so1 = new List<SalesOrder>();

            foreach (SalesOrder so2 in so)
            {
                so1.Add(new SalesOrder { SalesOrderId = so2.SalesOrderId, Doc_No = so2.Doc_No });
            }
            return so1;
        }


        [HttpPost]
        public async Task<ActionResult> AddOrEditOutward(Outward outward)
        {
            try
            {
                bool isValid = true;

                if(outward.CarrierId == 0 || outward.CostPerUnit == 0 || outward.TotalCost == 0 || outward.QuantityShipped == 0)
                {
                    isValid = false;
                    //ModelState.AddModelError("BatchNumber", "Batch Number should not be empty");
                }

                if (isValid)
                {
                    string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                    Customer cust = await customerService.GetCustomerById(outward.CustomerId, authToken);
                    Product prod = await productService.GetProductById(outward.ProductId, authToken);
                    SalesOrder order = await saleorderService.GetSalesOrderById(outward.SalesOrderId, authToken);

                    outward.CustomerName = cust.CustomerName;
                    outward.ProductName = prod.ProductName;
                    outward.Doc_No = order.Doc_No;

                    outward.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (outward.OutwardId != Guid.Empty)
                    {
                        await outwardService.EditOutwardDetailsAsync(outward, authToken);
                    }
                    else
                    {
                        await outwardService.AddOutwardDetailsAsync(outward, authToken);
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<ActionResult> RemoveOutward(string outwardId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await outwardService.DeleteOutward(outwardId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Outward details." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsAndCustomers(Guid saleOrderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            // Replace with your actual logic to fetch products and customers based on sale order ID
            var products = await productService.getProdutFromSaleOrderId(saleOrderId, authToken);
            var customers = await outwardService.getCustomerFromSaleOrderId(saleOrderId, authToken);

            return Json(new { products, customers });
        }

    }
}
