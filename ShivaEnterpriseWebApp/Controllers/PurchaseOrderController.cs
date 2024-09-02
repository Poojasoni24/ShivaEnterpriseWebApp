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
        IStockServiceImpl stockservice = new StockServiceImpl();
        IInventoryServiceImpl inventoryservice = new InventoryServiceImpl();


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
                    var poDetailsByPOId = purchaseorderDetailService.GetPurchaseOrderDetailList(authToken).Result.Where(x => x.PurchaseOrderId == purchaseorderId).ToList();
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
                var purchaseOrderId = Guid.Empty;
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

                PurchaseOrderViewModel.PODetail = PurchaseOrderViewModel.PODetail.Where(item1 => !PurchaseOrderViewModel.UpdatedPODetail.Any(item2 => item2.PurchaseOrderDetailId == item1.PurchaseOrderDetailId)).ToList();
                PurchaseOrderViewModel.PurchaseOrder.PurchaseOrderStatus = "Approve";
                PurchaseOrderViewModel.PurchaseOrder.Vendor = await vendorService.GetVendorById(PurchaseOrderViewModel.PurchaseOrder.VendorID, authToken);

                // Stock Implementation
                List<PurchaseOrderDetail> poBefore = new List<PurchaseOrderDetail>();

                if (PurchaseOrderViewModel.UpdatedPODetail.Count != 0)
                {
                    foreach (var PODetail in PurchaseOrderViewModel.UpdatedPODetail)
                    {
                        poBefore.Add(await purchaseorderDetailService.GetPurchaseOrderDetailById(PODetail.PurchaseOrderDetailId.ToString(), authToken));
                    }
                }
                // Stock Implementation

                if (PurchaseOrderViewModel.PurchaseOrder.PurchaseOrderId != Guid.Empty)
                {
                    PurchaseOrderViewModel.PurchaseOrder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    PurchaseOrderViewModel.PurchaseOrder.ModifiedDateTime = DateTime.Now;
                    var isSuccess = await purchaseorderService.EditPurchaseOrderDetailsAsync(PurchaseOrderViewModel.PurchaseOrder, authToken);
                    if (isSuccess.success)
                    {
                        purchaseOrderId = PurchaseOrderViewModel.PurchaseOrder.PurchaseOrderId;
                        var pOObject = await purchaseorderService.GetPurchaseOrderById(purchaseOrderId, authToken);
                        if (PurchaseOrderViewModel.UpdatedPODetail.Any())
                        {
                            PurchaseOrderViewModel.UpdatedPODetail.ForEach(
                                x =>
                                {
                                    x.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                                    x.ModifiedDateTime = DateTime.Now;
                                    x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                                    x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                                    x.PurchaseOrder = pOObject;
                                });
                            await purchaseorderDetailService.EditPurchaseOrderDetailDetailsAsync(PurchaseOrderViewModel.UpdatedPODetail, authToken);
                        }

                    }
                }
                else
                {

                    var netTotalAmount = PurchaseOrderViewModel.PODetail.Sum(x => x.NetTotal);
                    PurchaseOrderViewModel.PurchaseOrder.TotalAmount = netTotalAmount + (netTotalAmount * PurchaseOrderViewModel.PurchaseOrder.Tax_Percentage / 100);

                    PurchaseOrderViewModel.PurchaseOrder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    PurchaseOrderViewModel.PurchaseOrder.CreatedDateTime = DateTime.Now;

                    var issuccess = await purchaseorderService.AddPurchaseOrderDetailsAsync(PurchaseOrderViewModel.PurchaseOrder, authToken);
                    if (issuccess.success)
                    {
                        purchaseOrderId = JsonConvert.DeserializeObject<Guid>(issuccess.value);
                        await purchaseorderDetailService.AddPurchaseOrderDetailDetailsAsync(PurchaseOrderViewModel.PODetail, authToken);
                    }
                }

                if (PurchaseOrderViewModel.PODetail.Any(x => x.PurchaseOrderDetailId == Guid.Empty))
                {
                    var addNewPO = PurchaseOrderViewModel.PODetail.Where(x => x.PurchaseOrderDetailId == Guid.Empty).ToList();
                    addNewPO.ForEach(
                        x =>
                        {
                            x.PurchaseOrderId = purchaseOrderId;
                            x.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                            x.CreatedDateTime = DateTime.Now;
                            x.Product = productService.GetProductById(x.ProductId, authToken).Result;
                            x.Brand = brandService.GetBrandById(x.BrandId, authToken).Result;
                        });

                    await purchaseorderDetailService.AddPurchaseOrderDetailDetailsAsync(addNewPO, authToken);
                }

                // Stock Implementation
                List<Stock> stock = new List<Stock>();
                List<Inventory> inventory = new List<Inventory>();
                // Edit Stock
                if (PurchaseOrderViewModel.PurchaseOrder.PurchaseOrderId != Guid.Empty)
                {
                    int quantity;

                    for (int i = 0; i < PurchaseOrderViewModel.UpdatedPODetail.Count; i++)
                    {
                        if (poBefore.Count != 0)
                        {
                            quantity = (int)PurchaseOrderViewModel.UpdatedPODetail[i].Quantity - (int)poBefore[i].Quantity;
                        }
                        else
                        {
                            quantity = (int)PurchaseOrderViewModel.UpdatedPODetail[i].Quantity;
                        }

                        stock.Add(new Stock
                        {
                            ProductId = PurchaseOrderViewModel.UpdatedPODetail[i].ProductId,
                            QuantityOnHand = quantity,
                            ReorderLevel = "Default",
                            StockCode = PurchaseOrderViewModel.UpdatedPODetail[i].Product is null ? "" : PurchaseOrderViewModel.UpdatedPODetail[i].Product.ProductName,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                        });

                        inventory.Add(new Inventory
                        {
                            InventoryCode = PurchaseOrderViewModel.UpdatedPODetail[i].Product is null ? "" : PurchaseOrderViewModel.UpdatedPODetail[i].Product.ProductName,
                            ProductId = PurchaseOrderViewModel.UpdatedPODetail[i].ProductId,
                            OpeningQty = quantity,
                            ClosingQty = 0,
                            InQuantity = quantity,
                            OutQuantity = 0,
                            InventoryCost = PurchaseOrderViewModel.UpdatedPODetail[i].UnitPrice,
                            TransactionDate = DateTime.Now,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                        });
                    }
                }
                // Add stock
                else
                {
                    for (int i = 0; i < PurchaseOrderViewModel.PODetail.Count; i++)
                    {
                        int quantity = (int)PurchaseOrderViewModel.PODetail[i].Quantity;

                        stock.Add(new Stock
                        {
                            ProductId = PurchaseOrderViewModel.PODetail[i].ProductId,
                            QuantityOnHand = quantity,
                            ReorderLevel = "Default",
                            StockCode = PurchaseOrderViewModel.PODetail[i].Product is null ? "" : PurchaseOrderViewModel.PODetail[i].Product.ProductName,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                        });

                        inventory.Add(new Inventory
                        {
                            InventoryCode = PurchaseOrderViewModel.UpdatedPODetail[i].Product is null ? "" : PurchaseOrderViewModel.UpdatedPODetail[i].Product.ProductName,
                            ProductId = PurchaseOrderViewModel.UpdatedPODetail[i].ProductId,
                            OpeningQty = quantity,
                            ClosingQty = 0,
                            InQuantity = quantity,
                            OutQuantity = 0,
                            InventoryCost = PurchaseOrderViewModel.UpdatedPODetail[i].UnitPrice,
                            TransactionDate = DateTime.Now,
                            ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                        });

                    }
                }

                if(stock.Count != 0)
                {
                    await stockservice.AddEditStockDetailsAsync(stock, authToken);
                }
                if (inventory.Count != 0)
                {
                    await inventoryservice.AddEditInventoryDetailsAsync(inventory, authToken);
                }
                // Stock Implementation

                return View("Index");
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

                // Stock Implementation
                List<PurchaseOrderDetail> poDetail = await purchaseorderDetailService.GetPurchaseOrderDetailList(authToken);
                List<PurchaseOrderDetail> poDetailToUpdate = poDetail.Where(po => po.PurchaseOrderId == Guid.Parse(purchaseorderId)).ToList();

                List<Stock> stock = new List<Stock>();
                for (int i = 0; i < poDetailToUpdate.Count; i++)
                {
                    stock.Add(new Stock
                    {
                        ProductId = poDetailToUpdate[i].ProductId,
                        QuantityOnHand = 0 - (int)poDetailToUpdate[i].Quantity,
                        ReorderLevel = "Default",
                        ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value

                    });
                }

                await stockservice.AddEditStockDetailsAsync(stock, authToken);
                // Stock Implementation

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


    }
}



