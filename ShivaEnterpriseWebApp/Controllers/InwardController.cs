using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class InwardController : Controller
    {
        IInwardServiceImpl _inwardService = new InwardServiceImpl();
        IProductServiceImpl _productService = new ProductServiceImpl();
        IVendorServiceImpl _vendorService = new VendorServiceImpl();
        IPurchaseOrderDetailServiceImpl _purchaseOrderDetailService = new PurchaseOrderDetailServiceImpl();
        IPurchaseOrderServiceImpl _purchaseOrderService = new PurchaseOrderServiceImpl();
        IHostingEnvironment _hostingEnv;

        public InwardController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllInwards = await _inwardService.GetInwardList(authToken);
            if (getAllInwards != null && getAllInwards.Count > 0)
            {
                foreach (var item in getAllInwards)
                {
                    item.Product = await _productService.GetProductById(item.ProductId, authToken);
                    item.PurchaseOrder = await _purchaseOrderService.GetPurchaseOrderById(item.PurchaseOrderId, authToken);
                    item.Vendor = await _vendorService.GetVendorById(item.VendorId, authToken);
                }
            }
            return View("Index", getAllInwards);
        }


        [HttpGet]
        public async Task<ActionResult> AddOrEditInward(Guid inwardId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Product> productDataList = await _productService.GetProductList(authToken);
            SelectList productDropdownList = new SelectList(productDataList, "ProductId", "ProductName");
            ViewBag.ProductSelectList = productDropdownList;
            List<Vendor> vendorDataList = await _vendorService.GetVendorList(authToken);
            SelectList supplierDropdownList = new SelectList(vendorDataList, "VendorId", "VendorName");
            ViewBag.SupplierSelectList = supplierDropdownList;
            List<PurchaseOrder> purchaseOrdersDataList = await _purchaseOrderService.GetPurchaseOrderList(authToken);
            SelectList purchaseOrderDropdownList = new SelectList(purchaseOrdersDataList, "PurchaseOrderId", "OrderNumber"); // Assuming OrderNumber is a property
            ViewBag.PurchaseOrderSelectList = purchaseOrderDropdownList;

            if (inwardId != Guid.Empty)
            {
                var inwardDetail = await _inwardService.GetInwardById(inwardId, authToken);
                if (inwardDetail != null)
                {
                    var viewModel = new InwardViewModel
                    {
                        InwardId = inwardDetail.InwardId,
                        PurchaseOrderId = inwardDetail.PurchaseOrderId,
                        VendorId = inwardDetail.VendorId,
                        VendorName = inwardDetail.VendorName,
                        ReceiptDate = inwardDetail.ReceiptDate,
                        ReceivedBy = inwardDetail.ReceivedBy,
                        ProductId = inwardDetail.ProductId,
                        ProductName = inwardDetail.ProductName,
                        QuantityReceived = inwardDetail.QuantityReceived,
                        UnitOfMeasure = inwardDetail.UnitOfMeasure,
                        BatchNumber = inwardDetail.BatchNumber,
                        QualityCheckStatus = inwardDetail.QualityCheckStatus,
                        QualityCheckRemarks = inwardDetail.QualityCheckRemarks,
                        InvoiceNumber = inwardDetail.InvoiceNumber,
                        InvoiceDate = inwardDetail.InvoiceDate,
                        CostPerUnit = inwardDetail.CostPerUnit,
                        TotalCost = inwardDetail.TotalCost,
                        Remarks = inwardDetail.Remarks,
                        CreatedBy = inwardDetail.CreatedBy,
                        CreatedDate = inwardDetail.CreatedDate,
                        ModifiedBy = inwardDetail.ModifiedBy,
                        ModifiedDate = inwardDetail.ModifiedDate,
                        Products = productDataList,
                        PurchaseOrders = purchaseOrdersDataList,
                        Vendors = vendorDataList
                    };
                    return View("AddOrEditInward", viewModel);
                }
            }

            var newInwardViewModel = new InwardViewModel
            {
                Products = productDataList,
                Vendors = vendorDataList
            };
            return View("AddOrEditInward", newInwardViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditInward(InwardViewModel inwardViewModel)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

                if (inwardViewModel.InwardId != Guid.Empty)
                {
                    var existingInward = await _inwardService.GetInwardById(inwardViewModel.InwardId, authToken);
                    existingInward.PurchaseOrderId = inwardViewModel.PurchaseOrderId;
                    existingInward.VendorId = inwardViewModel.VendorId;
                    existingInward.ReceiptDate = inwardViewModel.ReceiptDate;
                    existingInward.ReceivedBy = inwardViewModel.ReceivedBy;
                    existingInward.ProductId = inwardViewModel.ProductId;
                    existingInward.QuantityReceived = inwardViewModel.QuantityReceived;
                    existingInward.UnitOfMeasure = inwardViewModel.UnitOfMeasure;
                    existingInward.BatchNumber = inwardViewModel.BatchNumber;
                    existingInward.QualityCheckStatus = inwardViewModel.QualityCheckStatus;
                    existingInward.QualityCheckRemarks = inwardViewModel.QualityCheckRemarks;
                    existingInward.InvoiceNumber = inwardViewModel.InvoiceNumber;
                    existingInward.InvoiceDate = inwardViewModel.InvoiceDate;
                    existingInward.CostPerUnit = inwardViewModel.CostPerUnit;
                    existingInward.TotalCost = inwardViewModel.TotalCost;
                    existingInward.Remarks = inwardViewModel.Remarks;
                    existingInward.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    existingInward.ModifiedDate = DateTime.Now;
                    await _inwardService.EditInwardDetailsAsync(existingInward, authToken);
                }
                else
                {
                    var newInward = new Inward
                    {
                        InwardId = Guid.NewGuid(),
                        PurchaseOrderId = inwardViewModel.PurchaseOrderId,
                        VendorId = inwardViewModel.VendorId,
                        ReceiptDate = inwardViewModel.ReceiptDate,
                        ReceivedBy = inwardViewModel.ReceivedBy,
                        ProductId = inwardViewModel.ProductId,
                        QuantityReceived = inwardViewModel.QuantityReceived,
                        UnitOfMeasure = inwardViewModel.UnitOfMeasure,
                        BatchNumber = inwardViewModel.BatchNumber,
                        QualityCheckStatus = inwardViewModel.QualityCheckStatus,
                        QualityCheckRemarks = inwardViewModel.QualityCheckRemarks,
                        InvoiceNumber = inwardViewModel.InvoiceNumber,
                        InvoiceDate = inwardViewModel.InvoiceDate,
                        CostPerUnit = inwardViewModel.CostPerUnit,
                        TotalCost = inwardViewModel.TotalCost,
                        Remarks = inwardViewModel.Remarks,
                        CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                        CreatedDate = DateTime.Now
                    };
                    await _inwardService.AddInwardDetailsAsync(newInward, authToken);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log exception
                return View("Index");
            }
        }

        public async Task<IActionResult> Delete(Guid inwardId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            await _inwardService.DeleteInward(inwardId, authToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
