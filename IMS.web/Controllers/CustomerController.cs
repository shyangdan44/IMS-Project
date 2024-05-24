﻿using IMS.infrastructure.Irepository;
using IMS.models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
	[Authorize(Roles = "ADMIN")]
	public class CustomerController : Controller
    {
        private readonly ICrudServices<CustomerInfo> _customerInfo;
        private readonly ICrudServices<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ICrudServices<CustomerInfo> customerInfo,
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
                        var OrgcustomerInfo = await _customerInfo.GetAsync(customerInfo.Id);
                        OrgcustomerInfo.CustomerName = customerInfo.CustomerName;                       
                        OrgcustomerInfo.Email = customerInfo.Email;
                        OrgcustomerInfo.PhoneNumber = customerInfo.PhoneNumber;
                        OrgcustomerInfo.PanNo = customerInfo.PanNo;
                        OrgcustomerInfo.Address = customerInfo.Address;
                        OrgcustomerInfo.CreatedBy = customerInfo.CreatedBy;
                        OrgcustomerInfo.CreatedDate = DateTime.Now;
						await _customerInfo.UpdateAsync(OrgcustomerInfo);
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