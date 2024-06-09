﻿using IMS.web.Data;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using IMS.infrastructure.Irepository;
using IMS.models.Entity;
using IMS.models.ViewModels;

namespace IMS.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterViewModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICrudServices<StoreInfo> _storeInfo;
        private readonly IRawSqlRepository _rawSqlRepository;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public AccountController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterViewModel> logger,
            IEmailSender emailSender,
            ICrudServices<StoreInfo> storeInfo,
            IRawSqlRepository rawSqlRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _storeInfo = storeInfo;
            _rawSqlRepository = rawSqlRepository;
            _emailStore = GetEmailStore();
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var users = await _context.ApplicationUsers.ToListAsync();
            IEnumerable<UserInfo> userInfos = new List<UserInfo>();

            var roleinfo = await _roleManager.FindByIdAsync(user.UserRoleId);


            if (roleinfo.Name == "SUPERADMIN")
            {
                var result = _rawSqlRepository.FromSql<UserInfo>(
                "usp_GetEmployee @storeId",
                new SqlParameter("@storeId", user.StoreId)
            ).ToList();
                userInfos = result;
            }
            else
            {
                var result = _rawSqlRepository.FromSql<UserInfo>(
                 "usp_GetEmployee @storeId",
                 new SqlParameter("@storeId", user.StoreId)
             ).ToList();
                userInfos = result;
            }




            return View(userInfos);
        }

        public async Task<IActionResult> AddUser()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.StoreId = user.StoreId;
            registerViewModel.IsActive = true;
            var roleinfo = await _roleManager.FindByIdAsync(user.UserRoleId);
            if (roleinfo.Name == "SUPERADMIN")
            {
                ViewBag.StoreInfo = await _storeInfo.GetAllAsync();
            }
            else
            {
                ViewBag.StoreInfo = await _storeInfo.GetAllAsync(p => p.Id == user.StoreId);
            }

            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userIds = _userManager.GetUserId(HttpContext.User);
                var users = await _userManager.FindByIdAsync(userIds);
                var user = CreateUser();

                user.FirstName = registerViewModel.FirstName;
                user.MiddleName = registerViewModel.MiddleName;
                user.LastName = registerViewModel.LastName;
                user.Address = registerViewModel.Address;
                user.PhoneNumber = registerViewModel.PhoneNumber;
                user.StoreId = registerViewModel.StoreId;
                user.CreatedBy = users.Id;
                user.CreatedDate = DateTime.Now;
                user.IsActive = true;
                var returnUrl = Url.Content("~/");
                var role = _roleManager.FindByNameAsync(registerViewModel.UserRoleId).Result;
                user.UserRoleId = role.Id;
                await _userStore.SetUserNameAsync(user, registerViewModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, registerViewModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {

                    if (role != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(registerViewModel.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}