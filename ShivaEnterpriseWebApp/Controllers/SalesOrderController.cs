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
        ICityServiceImpl cityService = new CityServiceImpl();
        ISalesOrderDetailServiceImpl salesorderDetailService = new SalesOrderDetailServiceImpl();
        IStockServiceImpl stockservice = new StockServiceImpl();
        IInventoryServiceImpl inventoryservice = new InventoryServiceImpl();
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
                var SalesOrder = await salesorderService.GetSalesOrderById(salesorderId, authToken);
                var SalesOrderDetail = salesorderDetailService.GetSalesOrderDetailList(authToken).Result.Where(x => x.SalesOrderId == salesorderId).ToList();
                if (SalesOrder != null && SalesOrderDetail != null)
                {
                    SalesOrderDetail.ForEach(x =>
                    {
                        x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                        x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                    });

                    var salesOrderViewModel = new SalesOrderViewModel()
                    {
                        SalesOrder = SalesOrder,
                        SODetail = SalesOrderDetail,
                    };
                    return View("AddOrEditSalesOrder", salesOrderViewModel);
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

                // List<SalesOrderDetail> soDetailList = new List<SalesOrderDetail>();

                List<Customer> customerDataList = await customerService.GetCustomerList(authToken);
                SelectList customergroupselectList = new SelectList(customerDataList, "CustomerId", "CustomerName");
                ViewBag.customerSelectList = customergroupselectList;

                List<Product> productDataList = await productService.GetProductList(authToken);
                SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;

                List<Brand> brandDataList = await brandService.GetBrandList(authToken);
                SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
                ViewBag.BrandSelectList = brandgroupselectList;

                // Stock Implementation
                List<SalesOrderDetail> soBefore = new List<SalesOrderDetail>();
                if (SalesOrderViewModel.UpdatedSODetail.Count != 0)
                {
                    foreach (var SoDetail in SalesOrderViewModel.UpdatedSODetail)
                    {
                        SalesOrderDetail soDetail = await salesorderDetailService.GetSalesOrderDetailById(SoDetail.SalesOrderDetailId, authToken);
                        soBefore.Add(soDetail);
                    }
                }

                bool isQuantityLow = false;
                List<Stock> stock = new List<Stock>();
                List<Inventory> inventory = new List<Inventory>();

                if (SalesOrderViewModel.SalesOrder.SalesOrderId != Guid.Empty)
                {
                    for (int i = 0; i < SalesOrderViewModel.UpdatedSODetail.Count; i++)
                    {
                        int quantity;

                        if (soBefore.Count != 0)
                        {
                            quantity = (int)soBefore[i].Quantity - (int)SalesOrderViewModel.UpdatedSODetail[i].Quantity;
                        }
                        else
                        {
                            quantity = 0 - (int)SalesOrderViewModel.UpdatedSODetail[i].Quantity;
                        }

                        Stock stockDetail = await stockservice.GetStockByProductId(SalesOrderViewModel.UpdatedSODetail[i].ProductId, authToken);
                        if ((stockDetail.QuantityOnHand + quantity) < 0)
                        {
                            isQuantityLow = true;
                            break;
                        }

                        stock.Add(new Stock
                        {
                            ProductId = SalesOrderViewModel.SODetail[i].ProductId,
                            QuantityOnHand = quantity,
                            ReorderLevel = "Default",
                            StockCode = SalesOrderViewModel.SODetail[i].Product is null ? "" : SalesOrderViewModel.SODetail[i].Product.ProductName,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                        });

                        inventory.Add(new Inventory
                        {
                            InventoryCode = SalesOrderViewModel.SODetail[i].Product is null ? "" : SalesOrderViewModel.SODetail[i].Product.ProductName,
                            ProductId = SalesOrderViewModel.SODetail[i].ProductId,
                            OpeningQty = quantity,
                            ClosingQty = Math.Abs(quantity),
                            InQuantity = 0,
                            OutQuantity = Math.Abs(quantity),
                            InventoryCost = SalesOrderViewModel.UpdatedSODetail[i].UnitPrice,
                            TransactionDate = DateTime.Now,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                        });

                    }
                }
                else
                {
                    for (int i = 0; i < SalesOrderViewModel.SODetail.Count; i++)
                    {
                        int quantity = 0 - (int)SalesOrderViewModel.SODetail[i].Quantity;

                        Stock stockDetail = await stockservice.GetStockByProductId(SalesOrderViewModel.SODetail[i].ProductId, authToken);
                        if (stockDetail.QuantityOnHand < (int)SalesOrderViewModel.SODetail[i].Quantity)
                        {
                            isQuantityLow = true;
                            break;
                        }

                        stock.Add(new Stock
                        {
                            ProductId = SalesOrderViewModel.SODetail[i].ProductId,
                            QuantityOnHand = quantity,
                            ReorderLevel = "Default",
                            StockCode = SalesOrderViewModel.SODetail[i].Product is null ? "" : SalesOrderViewModel.SODetail[i].Product.ProductName,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                        });

                        inventory.Add(new Inventory
                        {
                            InventoryCode = SalesOrderViewModel.SODetail[i].Product is null ? "" : SalesOrderViewModel.SODetail[i].Product.ProductName,
                            ProductId = SalesOrderViewModel.SODetail[i].ProductId,
                            OpeningQty = quantity,
                            ClosingQty = Math.Abs(quantity),
                            InQuantity = 0,
                            OutQuantity = Math.Abs(quantity),
                            InventoryCost = SalesOrderViewModel.UpdatedSODetail[i].UnitPrice,
                            TransactionDate = DateTime.Now,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                        });
                    }
                }

                if (isQuantityLow)
                {
                    ViewBag.Message = "Product Quantity is less than required.";
                    return Content("Product Quantity is less than required.");

                }

                if (stock.Count > 0)
                {
                    await stockservice.AddEditStockDetailsAsync(stock, authToken);
                }
                if (inventory.Count > 0)
                {
                    await inventoryservice.AddEditInventoryDetailsAsync(inventory, authToken);
                }

                // Editing logic
                if (SalesOrderViewModel.SalesOrder.SalesOrderId != Guid.Empty)
                {
                    // Edit the sales order
                    var netTotalAmount = SalesOrderViewModel.SODetail.Sum(x => x.NetTotal);
                    SalesOrderViewModel.SalesOrder.TotalAmount = netTotalAmount + (netTotalAmount * SalesOrderViewModel.SalesOrder.Tax_Percentage / 100);
                    SalesOrderViewModel.SalesOrder.SaleOrderStatus = "Approve";
                    SalesOrderViewModel.SalesOrder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    SalesOrderViewModel.SalesOrder.ModifiedDateTime = DateTime.Now;
                    SalesOrderViewModel.SalesOrder.Customer = await customerService.GetCustomerById(SalesOrderViewModel.SalesOrder.CustomerId, authToken);
                    SalesOrderViewModel.SalesOrder.Customer.City = await cityService.GetCityById(SalesOrderViewModel.SalesOrder.Customer.cityId.Value, authToken);
                    await salesorderService.EditSalesOrderDetailsAsync(SalesOrderViewModel.SalesOrder, authToken);
                }
                else
                {
                    // Adding a new sales order
                    var netTotalAmount = SalesOrderViewModel.SODetail.Sum(x => x.NetTotal);
                    SalesOrderViewModel.SalesOrder.SalesOrderId = Guid.NewGuid();
                    SalesOrderViewModel.SalesOrder.TotalAmount = netTotalAmount + (netTotalAmount * SalesOrderViewModel.SalesOrder.Tax_Percentage / 100);
                    SalesOrderViewModel.SalesOrder.SaleOrderStatus = "Approve";
                    SalesOrderViewModel.SalesOrder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    SalesOrderViewModel.SalesOrder.CreatedDateTime = DateTime.Now;
                    SalesOrderViewModel.SalesOrder.Customer = await customerService.GetCustomerById(SalesOrderViewModel.SalesOrder.CustomerId, authToken);
                    SalesOrderViewModel.SalesOrder.Customer.City = await cityService.GetCityById(SalesOrderViewModel.SalesOrder.Customer.cityId.Value, authToken);
                    var issuccess = await salesorderService.AddSalesOrderDetailsAsync(SalesOrderViewModel.SalesOrder, authToken);
                    if (issuccess.success)
                    {
                        SalesOrderViewModel.SODetail.ForEach(
                            x =>
                            {
                                x.SalesOrderId = JsonConvert.DeserializeObject<Guid>(issuccess.value);
                                x.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                x.CreatedDateTime = DateTime.Now;
                                x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                                x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                                x.Tax_Percentage = "12";
                            });

                        await salesorderDetailService.AddSalesOrderDetailDetailsAsync(SalesOrderViewModel.SODetail, authToken);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveSalesOrder(string SalesOrderId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                // Stock Implementation
                List<SalesOrderDetail> soDetail = await salesorderDetailService.GetSalesOrderDetailList(authToken);
                List<SalesOrderDetail> soDetailToUpdate = soDetail.Where(po => po.SalesOrderId == Guid.Parse(SalesOrderId)).ToList();

                List<Stock> stock = new List<Stock>();
                List<Inventory> inventory = new List<Inventory>();

                for (int i = 0; i < soDetailToUpdate.Count; i++)
                {
                    stock.Add(new Stock
                    {
                        ProductId = soDetailToUpdate[i].ProductId,
                        QuantityOnHand = (int)soDetailToUpdate[i].Quantity,
                        ReorderLevel = "Default",
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                    });

                    inventory.Add(new Inventory
                    {
                        ProductId = soDetailToUpdate[i].ProductId,
                        OpeningQty = (int)soDetailToUpdate[i].Quantity,
                        ClosingQty = 0 - (int)soDetailToUpdate[i].Quantity,
                        InQuantity = 0,
                        OutQuantity = 0 - (int)soDetailToUpdate[i].Quantity,
                        InventoryCost = soDetailToUpdate[i].UnitPrice,
                        TransactionDate = DateTime.Now,
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    });
                }

                await stockservice.AddEditStockDetailsAsync(stock, authToken);
                // Stock Implementation

                var response = await salesorderService.DeleteSalesOrder(SalesOrderId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove PurchaseOrder." });
            }
        }

    }
}
