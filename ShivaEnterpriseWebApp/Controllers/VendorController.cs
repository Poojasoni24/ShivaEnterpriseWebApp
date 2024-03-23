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
        ICityServiceImpl cityService = new CityServiceImpl();
        
        private readonly IHostingEnvironment _hostingEnv;

        public VendorController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllVendor = await vendorService.GetVendorList(authToken);
            foreach (var item in getAllVendor.Where(x => x.cityId != null))
            {
                item.City = await cityService.GetCityById(item.cityId, authToken);
            }

            return View("Index", getAllVendor);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditVendor(Guid vendorId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<City> cityDataList = await cityService.GetCityList(authToken);
            SelectList groupselectList = new SelectList(cityDataList, "cityId", "City_Name");
            ViewBag.citySelectList = groupselectList;

            if (vendorId != Guid.Empty)
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
                List<City> cityDataList = await cityService.GetCityList(authToken);
                SelectList groupselectList = new SelectList(cityDataList, "City_Id", "City_Name");
                ViewBag.citySelectList = groupselectList;
                vendor.City = await cityService.GetCityById(vendor.cityId, authToken);
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
                    await vendorService.AddVendorDetailsAsync(vendor, authToken);
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }
      
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

        public async Task<ActionResult> VendorDetail(Guid vendorId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var vendorData = await vendorService.GetVendorById(vendorId, authToken);
            return PartialView("_vendorDetail", new Vendor()
            {
                VendorId = vendorId,
                VendorCode = vendorData.VendorCode,
                VendorName = vendorData.VendorName,
                VendorType = vendorData.VendorType,
                VendorAddress = vendorData.VendorAddress,
                Phoneno = vendorData.Phoneno,
                Email= vendorData.Email,
                cityId = vendorData.cityId,
                City = await cityService.GetCityById(vendorData.cityId, authToken),
                ContractStartDate = vendorData.ContractStartDate,
                ContractEndDate= vendorData.ContractEndDate,
                Remark = vendorData.Remark,
                IsActive = vendorData.IsActive,               
                
            });
        }
    }
}

