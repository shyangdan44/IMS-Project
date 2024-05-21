using IMS.infrastructure.Irepository;
using IMS.models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class CustomerInfoController : Controller
    {
        private readonly ICrudServices<CustomerInfo> _customerInfo;
        private readonly ICrudServices<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerInfoController(ICrudServices<CustomerInfo> customerInfo,
            ICrudServices<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager)
        {
            _customerInfo = customerInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            var customerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);


            return View(customerInfo);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int Id)
        {
            CustomerInfo customerInfo = new CustomerInfo();
            customerInfo.IsActive = true;
            if (Id > 0)
            {
                customerInfo = await _customerInfo.GetAsync(Id);

            }

            return View(customerInfo);

        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(CustomerInfo customerInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (customerInfo.Id == 0)
                    {
                        customerInfo.CreatedDate = DateTime.Now;
                        customerInfo.CreatedBy = userId;
                        customerInfo.StoreInfoId = user.StoreId;
                        await _customerInfo.InsertAsync(customerInfo);

                        TempData["success"] = "Data Added Successfully!";
                    }
                    else
                    {
                        var OrgCustomerInfo = await _customerInfo.GetAsync(customerInfo.Id);
                        OrgCustomerInfo.CustomerName = customerInfo.CustomerName;
                        OrgCustomerInfo.StoreInfoId = customerInfo.StoreInfoId;
                        OrgCustomerInfo.Email = customerInfo.Email;
                        OrgCustomerInfo.PhoneNumber = customerInfo.PhoneNumber;
                        OrgCustomerInfo.Address = customerInfo.Address;
                        OrgCustomerInfo.PanNo = customerInfo.PanNo;
                        OrgCustomerInfo.Address = customerInfo.Address;
                        OrgCustomerInfo.CreatedBy = customerInfo.CreatedBy;
                        OrgCustomerInfo.CreatedDate = DateTime.Now;
						OrgCustomerInfo.ModifiedDate = DateTime.Now;
						OrgCustomerInfo.ModifiedBy = userId;
						OrgCustomerInfo.IsActive = customerInfo.IsActive;
						await _customerInfo.UpdateAsync(OrgCustomerInfo);
                        TempData["success"] = "Data Updated Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }
            TempData["error"] = "Please input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }


    }
}