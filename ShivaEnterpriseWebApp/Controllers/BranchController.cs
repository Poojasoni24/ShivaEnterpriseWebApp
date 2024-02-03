using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class BranchController : Controller
    {
        IBranchServiceImpl branchObject = new BranchServiceImpl();
        ICompany Company = new CompanyServiceImpl();
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var branchallData = await branchObject.GetBranchList(authToken);
            foreach (var item in branchallData)
            {
                item.Company = await Company.GetCompanyDetailById(item.Company_Id, authToken);
            }
            return View("Index", branchallData);
        }

        [HttpGet]
        public async Task<ActionResult> AddorEditBranch(Guid id)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            List<Company> companyDataList = await Company.GetCompanyDetailsAsync(authToken);
            SelectList selectList = new SelectList(companyDataList, "Company_Id", "Company_Name");
            ViewBag.SelectList = selectList;
            if (id != Guid.Empty)
            {
                var editData = await branchObject.GetBranchById(id, authToken);
                return View("AddorEditBranch", editData);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddorEditBranch(Branch branch, string id)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                List<Company> companyDataList = await Company.GetCompanyDetailsAsync(authToken);
                SelectList selectList = new SelectList(companyDataList, "Company_Id", "Company_Name");
                ViewBag.SelectList = selectList;
                if (string.IsNullOrEmpty(id))
                {
                    branch.IsActive = true;
                    var companyData = await branchObject.AddBranchDetailsAsync(branch, authToken);
                }
                else
                {
                    branch.Branch_Id = new Guid(id);
                    var result = await branchObject.EditBranchDetailsAsync(branch, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveBranch(string branchId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await branchObject.DeleteBranch(branchId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Branch." });
            }
        }

        public async Task<ActionResult> BranchDetails(Guid branchId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var branchData = await branchObject.GetBranchById(branchId, authToken);
            return PartialView("_BranchDetail", new Branch()
            {
              Branch_Id = branchId,
              Branch_Code = branchData.Branch_Code,
              Branch_Name = branchData.Branch_Name,
              Company = await Company.GetCompanyDetailById(branchData.Company_Id,authToken),
              IsActive = branchData.IsActive,
            });
        }

    }
}
