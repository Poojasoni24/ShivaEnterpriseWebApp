using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Security.Claims;
using ShivaEnterpriseWebApp.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class VendorController : Controller
    {
        IVendorServiceImpl vendorService = new VendorServiceImpl();
        IBankServiceImpl bankService = new BankServiceImpl();
        ITaxServiceImpl taxService = new TaxServiceImpl();
        
        private readonly IHostingEnvironment _hostingEnv;

        public VendorController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllVendor = await vendorService.GetVendorList(authToken);
            foreach (var item in getAllVendor)
            {
                item.Bank = await bankService.GetBankById(item.BankId, authToken);
                item.Tax = await taxService.GetTaxById(item.TaxId, authToken);
                
            }

            return View("Index", getAllVendor);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditVendor(string vendorId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Bank> bankDataList = await bankService.GetBankList(authToken);
            SelectList groupselectList = new SelectList(bankDataList, "BankId", "BankName");
            ViewBag.bankSelectList = groupselectList;

            List<Tax> taxDataList = await taxService.GetTaxList(authToken);
            SelectList typeselectList = new SelectList(taxDataList, "TaxId", "TaxName");
            ViewBag.taxList = typeselectList;

            
            if (!string.IsNullOrEmpty(vendorId))
            {
                var VendorDetail = await vendorService.GetVendorById(vendorId, authToken);
                if (VendorDetail != null)
                {
                    return View("AddOrEditVendor", VendorDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditVendor(string vendorId, Vendor vendor)
        {
            try
            {

                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(vendorId))
                {
                    vendor.VendorId = new Guid(vendorId);
                    vendor.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    vendor.ModifiedDateTime = DateTime.Now;
                    await vendorService.EditVendorDetailsAsync(vendor, authToken);
                }
                else
                {
                    vendor.IsActive = true;
                    vendor.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    vendor.CreatedDateTime = DateTime.Now;
                   // string image = uploadImage(product.ImageFile);
                   // product.ProductImage = "/img/product/" + image;
                    await vendorService.AddVendorDetailsAsync(vendor, authToken);
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
        public async Task<ActionResult> RemoveVendor(string vendorId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await vendorService.DeleteVendor(vendorId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove vendor." });
            }
        }

        public async Task<ActionResult> VendorDetail(string vendorId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var vendorData = await vendorService.GetVendorById(vendorId, authToken);
            return PartialView("_vendorDetail", new Vendor()
            {
                VendorId = new Guid(vendorId),
                VendorCode = vendorData.VendorCode,
                VendorName = vendorData.VendorName,
                VendorType = vendorData.VendorType,
                VendorAddress = vendorData.VendorAddress,
                Phoneno = vendorData.Phoneno,
                Email= vendorData.Email,
                BankId = vendorData.BankId,
                TaxId = vendorData.TaxId,
                ContractStartDate= vendorData.ContractStartDate,
                ContractEndDate= vendorData.ContractEndDate,
                Remark = vendorData.Remark,
                IsActive = vendorData.IsActive,
                
                
            });
        }
    }
}

