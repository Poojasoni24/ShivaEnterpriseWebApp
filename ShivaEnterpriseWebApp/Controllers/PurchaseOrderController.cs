using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        IPurchaseOrderServiceImpl purchaseorderService = new PurchaseOrderServiceImpl();
        

        private readonly IHostingEnvironment _hostingEnv;

        public PurchaseOrderController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllPurchaseOrder = await purchaseorderService.GetPurchaseOrderList(authToken);
            foreach (var item in getAllPurchaseOrder)
            {
             //   item.Vendor = await vendorService.GetVendorById(item.VendorId, authToken);

            }

            return View("Index", getAllPurchaseOrder);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditPurchaseOrder(string purchaseorderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Vendor> vendorDataList = await vendorService.GetVendorList(authToken);
            SelectList groupselectList = new SelectList(vendorDataList, "VendorId", "VendorName");
            ViewBag.vendorSelectList = groupselectList;


            if (!string.IsNullOrEmpty(purchaseorderId))
            {
                var PurchaseOrderDetail = await purchaseorderService.GetPurchaseOrderById(purchaseorderId, authToken);
                if (PurchaseOrderDetail != null)
                {
                    return View("AddOrEditPurchaseOrder", PurchaseOrderDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditPurchaseOrder(string purchaseorderId, PurchaseOrder purchaseorder)
        {
            try
            {

                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(purchaseorderId))
                {
                    purchaseorder.PurchaseOrderId = new Guid(purchaseorderId);
                    purchaseorder.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseorder.ModifiedDateTime = DateTime.Now;
                    await purchaseorderService.EditPurchaseOrderDetailsAsync(purchaseorder, authToken);
                }
                else
                {
                    purchaseorder.IsActive = true;
                    purchaseorder.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    purchaseorder.CreatedDateTime = DateTime.Now;
                    // string image = uploadImage(product.ImageFile);
                    // product.ProductImage = "/img/product/" + image;
                    await purchaseorderService.AddPurchaseOrderDetailsAsync(purchaseorder, authToken);
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }
        // public string uploadImage(IFormFile image)
        //{
        //   string imageFileName = string.Empty;
        //   string basedirecotry = _hostingEnv.WebRootPath;

        //  string filePath = basedirecotry + "\\img\\product\\";

        // if (!System.IO.Directory.Exists(filePath))
        //     System.IO.Directory.CreateDirectory(filePath);

        //  if (image != null)
        // {
        //   imageFileName = Guid.NewGuid().ToString().ToLower() + new FileInfo(image.FileName).Extension;
        // string path = Path.Combine(filePath, imageFileName);
        // using (Stream fileStream = new FileStream(path, FileMode.Create))
        // {
        //     image.CopyTo(fileStream);
        // }
        // }
        // return imageFileName;
        //  }

        [HttpPost]
        public async Task<ActionResult> RemovePurchaseOrder(string purchaseorderId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await purchaseorderService.DeletePurchaseOrder(purchaseorderId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove PurchaseOrder." });
            }
        }

        public async Task<ActionResult> PurchaseOrderDetail(string purchaseorderId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var purchaseorderData = await purchaseorderService.GetPurchaseOrderById(purchaseorderId, authToken);
            return PartialView("_purchaseorderDetail", new PurchaseOrder()
            {
                PurchaseOrderId = new Guid(purchaseorderId),
                VendorID = purchaseorderData.VendorID,
                OrderDate = purchaseorderData.OrderDate,
                DeliveryDate = purchaseorderData.DeliveryDate,
                TotalAmount = purchaseorderData.TotalAmount,
                PurchaseOrderStatus = purchaseorderData.PurchaseOrderStatus,
                IsActive = purchaseorderData.IsActive,


            });
        }
    }
}


