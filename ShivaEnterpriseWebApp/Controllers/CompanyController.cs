using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
using System.Security.Policy;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CompanyController : Controller
    {
        ICompany companyobject = new CompanyServiceImpl();
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var companyallData = await companyobject.GetCompanyDetailsAsync(authToken);            
            return View("Index", companyallData);
        }

        public async Task<ActionResult> Details(Guid companyId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var companyData = await companyobject.GetCompanyDetailById(companyId, authToken);
            return PartialView("_companyDetails", new Company() {
                Company_Id = companyId,
                Company_Code = companyData.Company_Code,
                Company_Name = companyData.Company_Name,
                Company_Startyear = companyData.Company_Startyear,
                Company_Endyear = companyData.Company_Endyear,
                IsActive = companyData.IsActive,
            });
        }

        
        [HttpGet]
        public ActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCompany(Company companyDTO)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                companyDTO.IsActive = true;
                var companyData = await companyobject.AddCompanyDetailsAsync(companyDTO, authToken);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditCompany(Guid id)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var editData = await companyobject.GetCompanyDetailById(id,authToken);
            return View("EditCompany",editData);
        }

        [HttpPost]
        public async Task<ActionResult> EditCompany(Guid id, Company companyDTO)             
        {
            try
            {
                companyDTO.Company_Id = id;
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var result = await companyobject.EditCompanyDetailsAsync(companyDTO, authToken);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCompany(Guid companyId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await companyobject.DeleteCompany(companyId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove company." });
            }
        }
    }
}
