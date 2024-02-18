using Microsoft.AspNetCore.Mvc;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;
namespace ShivaEnterpriseWebApp.Controllers
{
    public class IssueController : Controller
    {
        IIssueServiceImpl issueObj = new IssueServiceImpl();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var issueDetail = await issueObj.GetIssueList(authToken);
            return View("Index", issueDetail);
        }

        [HttpGet]
        public async Task<ActionResult> IssueDetail(string issueId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            var issueDetail = await issueObj.GetIssueById(issueId, authToken);
            return PartialView("_IssueDetail", new Issue()
            {
                IssueId = issueDetail.IssueId,
                IssueCode = issueDetail.IssueCode,
                IssueName = issueDetail.IssueName,
                IssueDescription = issueDetail.IssueDescription,
                IsActive = issueDetail.IsActive,
                CreatedDateTime = issueDetail.CreatedDateTime,
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditIssue(string issueId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (!string.IsNullOrEmpty(issueId))
            {
                var issueDetail = await issueObj.GetIssueById(issueId, authToken);
                return View(issueDetail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditIssue(Issue issue)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            if (authToken == null)
                return BadRequest("Something went wrong");
            if (issue.IssueId != Guid.Empty)
            {
                issue.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                issue.ModifiedDateTime = DateTime.Now;
                await issueObj.EditIssueDetailsAsync(issue, authToken);
            }
            else
            {
                issue.IsActive = true;
                issue.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                issue.CreatedDateTime = DateTime.Now;
                await issueObj.AddIssueDetailsAsync(issue, authToken);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveIssue(string issueId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (authToken == null)
                    return BadRequest("Something went wrong");
                var response = await issueObj.DeleteIssue(issueId, authToken);
                return Json(new { success = response.successs, response.message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occured while remove Issue." });
            }
        }
    }
}
