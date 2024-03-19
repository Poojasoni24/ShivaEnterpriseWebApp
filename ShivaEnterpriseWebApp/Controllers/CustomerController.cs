using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerServiceImpl customerService = new CustomerServiceImpl();
        ICityServiceImpl cityService = new CityServiceImpl();
        
        public async Task<IActionResult> Index()
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            var getAllCustomer = await customerService.GetCustomerList(authToken);
            foreach (var item in getAllCustomer)
            {
                item.City = await cityService.GetCityById(item.CityId, authToken);
               
            }

            return View("Index", getAllCustomer);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrEditCustomer(Guid customerId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            //Add accountgroup dropdown
            List<City> cityDataList = await cityService.GetCityList(authToken);
            SelectList groupselectList = new SelectList(cityDataList, "CityId", "CityName");
            ViewBag.citySelectList = groupselectList;

            
            if (customerId != Guid.Empty)
            {
                var customerDetail = await customerService.GetCustomerById(customerId, authToken);
                if (customerDetail != null)
                {
                    return View("AddOrEditCustomer", customerDetail);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditCustomer(string customerId, Customer customer)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                if (!string.IsNullOrEmpty(customerId))
                {
                    customer.CustomerId = new Guid(customerId);
                    customer.ModifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    customer.ModifiedDateTime = DateTime.Now;
                    await customerService.EditCustomerDetailsAsync(customer, authToken);
                }
                else
                {
                    customer.IsActive = true;
                    customer.CreatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    customer.CreatedDateTime = DateTime.Now;
                    await customerService.AddCustomerDetailsAsync(customer, authToken);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View("Index");
            }

        }

        [HttpPost]
        public async Task<ActionResult> RemoveCustomer(Guid customerId)
        {
            try
            {
                string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
                var response = await customerService.DeleteCustomer(customerId, authToken);

                return Json(new { success = response.successs, message = response.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured while remove Account." });
            }
        }

        public async Task<ActionResult> CustomerDetail(Guid customerId)
        {
            string? authToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;

            var customerData = await customerService.GetCustomerById(customerId, authToken);
            return PartialView("_customerDetail", new Customer()
            {
                CustomerId = customerId,
                CustomerCode = customerData.CustomerCode,
                CustomerName = customerData.CustomerName,
                CustomerType = customerData.CustomerType,
                CustomerAddress= customerData.CustomerAddress,
                Phoneno = customerData.Phoneno,
                Email = customerData.Email,
                ContractStartDate = customerData.ContractStartDate,
                ContractEndDate = customerData.ContractEndDate,
                Remark = customerData.Remark,
                IsActive = customerData.IsActive,
                CustomerDiscount = customerData.CustomerDiscount,
               
                
                City = await cityService.GetCityById(customerData.CityId, authToken),
                // AccountCategoryId = accountData.AccountCategoryId,
                //AccountGroup = await accountgroupService.GetAccountGroupById(accountData.AccountGroupId, authToken),
                //  AccountGroupId = accountData.AccountGroupId,
               // AccountType = await accountTypeService.GetAccountTypeById(accountData.AccountTypeId, authToken),
                // AccountTypeId = accountData.AccountTypeId,
            });
        }

    }
}
