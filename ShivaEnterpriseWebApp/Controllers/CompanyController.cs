using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Policy;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CompanyController : Controller
    {
        ICompany companyobject = new CompanyServiceImpl();
        public async Task<ActionResult> Index()
        {
            var companyallData = await companyobject.GetCompanyDetailsAsync();            
            return View("Index", companyallData);
        }

        public async Task<ActionResult> Details(Guid companyId)
        {
            var companyData = await companyobject.GetCompanyDetailById(companyId);
            return PartialView("_companyDetails", new CompanyDTO() {
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
        public async Task<ActionResult> AddCompany(CompanyDTO companyDTO)
        {
            try
            {
                companyDTO.IsActive = true;
                var companyData = await companyobject.AddCompanyDetailsAsync(companyDTO);
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
            var editData = await companyobject.GetCompanyDetailById(id);
            return View("EditCompany",editData);
        }

        [HttpPost]
        public async Task<ActionResult> EditCompany(Guid id, CompanyDTO companyDTO)             
        {
            try
            {
                var result = await companyobject.EditCompanyDetailsAsync(id, companyDTO);
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
                var response = await companyobject.DeleteCompany(companyId);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove organization." });
            }
        }
    }
}
