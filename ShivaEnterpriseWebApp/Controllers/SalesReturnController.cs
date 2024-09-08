using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Collections;
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
        ISalesOrderDetailServiceImpl salesOrderDetailService = new SalesOrderDetailServiceImpl();
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        IProductServiceImpl productService = new ProductServiceImpl();
        IBrandServiceImpl brandService = new BrandServiceImpl();
        ICityServiceImpl cityService = new CityServiceImpl();
        IStateServiceImpl stateService = new StateServiceImpl();
        ICountryServiceImpl countryService = new CountryServiceImpl();
        ISalesReturnDetailServiceImpl SalesReturnDetailService = new SalesReturnDetailServiceImpl();
        IStockServiceImpl stockservice = new StockServiceImpl();
        IInventoryServiceImpl inventoryservice = new InventoryServiceImpl();

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

            List<SalesOrder> saleOrderDataList = await salesOrderService.GetSalesOrderList(authToken);
            var saleOrders = GetSaleOrders(saleOrderDataList);
            ViewBag.SaleOrderSelectList = new SelectList(saleOrders, "SalesOrderId", "Doc_No");

            List<Product> productDataList = await productService.GetProductList(authToken);
            SelectList productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productgroupselectList;

            List<Brand> brandDataList = await brandService.GetBrandList(authToken);
            SelectList brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
            ViewBag.BrandSelectList = brandgroupselectList;
            if (salesReturnId != Guid.Empty)
            {
                var salesReturnDetail = await salesReturnService.GetSalesReturnById(salesReturnId, authToken);
                
                productDataList = await productService.getProdutFromSaleOrderId(salesReturnDetail.SalesOrderID, authToken);
                productgroupselectList = new SelectList(productDataList, "ProductId", "ProductName");
                ViewBag.ProductSelectList = productgroupselectList;
                var salesOrderDetail = await salesOrderDetailService.getsalesorderdetailbySalesOrderid(salesReturnDetail.SalesOrderID, authToken);
                salesReturnDetail.ProductId = productDataList[0].ProductId;
                var brand = brandService.GetBrandById(salesOrderDetail.BrandId, authToken).Result;
                brandDataList = new List<Brand> { brand };
                brandgroupselectList = new SelectList(brandDataList, "BrandId", "BrandName");
                ViewBag.BrandSelectList = brandgroupselectList;
                salesReturnDetail.Quantity = (int)salesOrderDetail.Quantity;
                if (salesReturnDetail != null)
                {
                    return View("AddOrEditSalesReturn", salesReturnDetail);
                }
            }
            return View("AddOrEditSalesReturn", new SalesReturn());
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

        //[HttpPost]
        //public async Task<ActionResult> AddOrEditSalesReturn(SalesReturn salesReturn)
        //{
        //    try
        //    {
        //        List<SalesReturnDetail> soDetailList = new List<SalesReturnDetail>();
        //        string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
        //        List<SalesOrder> salesOrderDataList = await salesOrderService.GetSalesOrderList(authToken);
        //        SelectList salesOrderSelectList = new SelectList(salesOrderDataList, "SalesOrderID", "OrderDate");
        //        ViewBag.salesOrderSelectList = salesOrderSelectList;

        //        if (salesReturn.SalesReturnID == null || salesReturn.SalesReturnID == Guid.Empty)
        //        {
        //            await salesReturnService.EditSalesReturnDetailsAsync(salesReturn, authToken);
        //        }
        //        else
        //        {
        //            var isSuccess = await salesReturnService.AddSalesReturnDetailsAsync(salesReturn, authToken);
        //            if (!isSuccess.success)
        //            {
        //                return View("AddOrEditSalesReturn");
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Index");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> AddOrEditSalesReturn(SalesReturn salesReturn)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                if (salesReturn.SalesReturnID == null || salesReturn.SalesReturnID == Guid.Empty)
                {
                    salesReturn.SalesReturnID =  Guid.NewGuid();
                    salesReturn.SalesOrder = await salesOrderService.GetSalesOrderById(salesReturn.SalesOrderID, authToken);
                    salesReturn.SalesOrder.SaleOrderStatus = "Approve";
                    salesReturn.SalesOrder.Customer =  await customerService.GetCustomerById(salesReturn.SalesOrder.CustomerId, authToken);
                    salesReturn.SalesOrder.Customer.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City = await cityService.GetCityById(salesReturn.SalesOrder.Customer.cityId.Value, authToken);
                    salesReturn.SalesOrder.Customer.City.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City.ModifiedDateTime = DateTime.Now;
                    salesReturn.SalesOrder.Customer.City.State = await stateService.GetStateById(salesReturn.SalesOrder.Customer.City.State_Id, authToken);
                    salesReturn.SalesOrder.Customer.City.Country = await countryService.GetCountryById(salesReturn.SalesOrder.Customer.City.State.Country_Id, authToken);
                    salesReturn.SalesOrder.Customer.City.State.country = salesReturn.SalesOrder.Customer.City.Country;
                    salesReturn.SalesOrder.Customer.City.State.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City.State.ModifiedDateTime = DateTime.Now;
                    //salesReturn.SalesOrder.Customer.City.StateList = await stateService.GetStateList(authToken);
                    //salesReturn.SalesOrder.Customer.CityList = await cityService.GetCityList(authToken);

                    var isSuccess = await salesReturnService.AddSalesReturnDetailsAsync(salesReturn, authToken);
                    if (!isSuccess.success)
                    {
                        return View("AddOrEditSalesReturn", salesReturn);
                    }

                    //----Add Stock-----
                    List<Stock> lst = new List<Stock>();
                    lst.Add(new Stock()
                    {
                        ProductId = salesReturn.ProductId,
                        QuantityOnHand = salesReturn.ReturnedQuantity,
                        ReorderLevel = "Default",
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    });

                    await stockservice.AddEditStockDetailsAsync(lst, authToken);
                    //----Add Stock-----

                    //--Inventory Add--
                    List<Inventory> inventory = new List<Inventory>();
                    inventory.Add(new Inventory
                    {
                        ProductId = salesReturn.ProductId,
                        OpeningQty = salesReturn.ReturnedQuantity,
                        ClosingQty = 0 - salesReturn.ReturnedQuantity,
                        InQuantity = 0,
                        OutQuantity = 0 - salesReturn.ReturnedQuantity,
                        TransactionDate = DateTime.Now,
                        InventoryCost = (decimal)salesReturn.RestockingFee,
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    });

                    if (inventory.Count > 0)
                    {
                        await inventoryservice.AddEditInventoryDetailsAsync(inventory, authToken);
                    }
                    //--Inventory Add--

                }
                else
                {
                    //---Stock Edit---
                    List<SalesReturn> saleReturnLst = await salesReturnService.GetSalesReturnList(authToken);
                    SalesReturn saleReturnDeetail = saleReturnLst.Where(sr => sr.SalesReturnID == salesReturn.SalesReturnID).FirstOrDefault();

                    List<Stock> lst = new List<Stock>();
                    lst.Add(new Stock()
                    {
                        ProductId = saleReturnDeetail.ProductId,
                        QuantityOnHand = saleReturnDeetail.ReturnedQuantity - salesReturn.ReturnedQuantity,
                        ReorderLevel = "Default",
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    });

                    await stockservice.AddEditStockDetailsAsync(lst, authToken);
                    //---Stock Edit---

                    //--Inventory Edit--
                    List<Inventory> inventory = new List<Inventory>();
                    inventory.Add(new Inventory
                    {
                        ProductId = saleReturnDeetail.ProductId,
                        OpeningQty = saleReturnDeetail.ReturnedQuantity - salesReturn.ReturnedQuantity,
                        ClosingQty = 0 - (saleReturnDeetail.ReturnedQuantity - salesReturn.ReturnedQuantity),
                        InQuantity = 0,
                        OutQuantity = 0 - (saleReturnDeetail.ReturnedQuantity - salesReturn.ReturnedQuantity),
                        TransactionDate = DateTime.Now,
                        InventoryCost = (decimal)salesReturn.RestockingFee,
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    });

                    if (inventory.Count > 0)
                    {
                        await inventoryservice.AddEditInventoryDetailsAsync(inventory, authToken);
                    }
                    //--Inventory Edit--

                    salesReturn.SalesOrder = await salesOrderService.GetSalesOrderById(salesReturn.SalesOrderID, authToken);
                    salesReturn.SalesOrder.SaleOrderStatus = "Approve";
                    salesReturn.SalesOrder.Customer = await customerService.GetCustomerById(salesReturn.SalesOrder.CustomerId, authToken);
                    salesReturn.SalesOrder.Customer.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City = await cityService.GetCityById(salesReturn.SalesOrder.Customer.cityId.Value, authToken);
                    salesReturn.SalesOrder.Customer.City.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City.ModifiedDateTime = DateTime.Now;
                    salesReturn.SalesOrder.Customer.City.State = await stateService.GetStateById(salesReturn.SalesOrder.Customer.City.State_Id, authToken);
                    salesReturn.SalesOrder.Customer.City.Country = await countryService.GetCountryById(salesReturn.SalesOrder.Customer.City.State.Country_Id, authToken);
                    salesReturn.SalesOrder.Customer.City.State.country = salesReturn.SalesOrder.Customer.City.Country;
                    salesReturn.SalesOrder.Customer.City.State.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    salesReturn.SalesOrder.Customer.City.State.ModifiedDateTime = DateTime.Now;
                    await salesReturnService.EditSalesReturnDetailsAsync(salesReturn, authToken);
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

                //---Stock Delete---
                List<SalesReturn> saleReturnLst = await salesReturnService.GetSalesReturnList(authToken);
                SalesReturn saleReturn = saleReturnLst.Where(sr => sr.SalesReturnID == salesReturnId).FirstOrDefault();

                List<Stock> lst = new List<Stock>();
                lst.Add(new Stock()
                {
                    ProductId = saleReturn.ProductId,
                    QuantityOnHand = 0 - saleReturn.ReturnedQuantity,
                    ReorderLevel = "Default",
                    ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                });

                await stockservice.AddEditStockDetailsAsync(lst, authToken);
                //---Stock Delete---

                //--Inventory Delete--
                List<Inventory> inventory = new List<Inventory>();
                inventory.Add(new Inventory
                {
                    ProductId = saleReturn.ProductId,
                    OpeningQty = 0 - saleReturn.ReturnedQuantity,
                    ClosingQty = saleReturn.ReturnedQuantity,
                    InQuantity = 0,
                    OutQuantity = saleReturn.ReturnedQuantity,
                    TransactionDate = DateTime.Now,
                    ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                });

                if (inventory.Count > 0)
                {
                    await inventoryservice.AddEditInventoryDetailsAsync(inventory, authToken);
                }
                //--Inventory Delete--

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

        [HttpGet]
        public async Task<JsonResult> GetProductAndQuantity(Guid salesOrderId)
        {
            if (salesOrderId == Guid.Empty)
            {
                return Json(new { error = "SalesOrderId is invalid" });
            }

            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var salesOrderDetail = await salesOrderDetailService.getsalesorderdetailbySalesOrderid(salesOrderId, authToken);
            var products = await productService.getProdutFromSaleOrderId(salesOrderDetail.SalesOrderId, authToken);
            //var product = productService.GetProductById(salesOrderDetail.ProductId, authToken).Result;
            var brand = brandService.GetBrandById(salesOrderDetail.BrandId, authToken).Result;
            List<Brand> brands = new List<Brand> { brand };
            var currentQuantity = salesOrderDetail.Quantity;

            return Json(new { products, brands, currentQuantity });
        }


    }
}
