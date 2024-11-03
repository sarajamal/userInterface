// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data;
using Utility;
using Test12.Models.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Test12.DataAccess.Repository.IRepository;

namespace UserInterface.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        //1- Add Roles To Table 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger,
            //2- Add Roles To Table 
            RoleManager<IdentityRole> roleManager,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "حقل اسم المستخدم مطلوب .")]
            [Display(Name = "اسم المستخدم")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "حقل كلمة المرور مطلوب.")]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            public string Role { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "خطأ في اسم المستخدم أو كلمة المرور.");
                    return Page();
                }
                if (result.Succeeded)
                {
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { returnUrl, RememberMe = Input.RememberMe });
                    }

                    _logger.LogInformation("User logged in.");

                    // Get the user
                    var user = await _userManager.FindByNameAsync(Input.UserName);

                    // جلب المعلومات الكاملة عن العلامة التجارية بناءً على BrandName
                    var brand =  _unitOfWork.TredMarketRepository.Get(b => b.BrandName == user.BrandName);
                
                    var userId = user.Id;
                    if (user != null)
                    {
                        // Get the roles for the user
                        var roles = await _userManager.GetRolesAsync(user);

                        string redirectUrl = null;
                        // Check if ExpirationDate is null
                        if (roles.Contains(SD.Role_Client)){
                            if (string.IsNullOrEmpty(user.ExpirationDate))
                            {
                                // Set ExpirationDate to 3 minutes after the first login
                                user.ExpirationDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd");

                                // Update the user's ExpirationDate in the database
                                var updateResult = await _userManager.UpdateAsync(user);
                                if (!updateResult.Succeeded)
                                {
                                    ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث بيانات المستخدم.");
                                    user.isActive = true;
                                    return Page();
                                }
                            }
                            else
                            {
                                // Check if ExpirationDate has passed
                                if (DateTime.TryParse(user.ExpirationDate, out DateTime expirationDate))
                                {
                                    if (DateTime.UtcNow > expirationDate)
                                    {
                                        // If expiration date has passed, deny login
                                        ModelState.AddModelError(string.Empty, "لقد انتهت صلاحية حسابك. يرجى التواصل مع الدعم.");
                                        user.isActive = false;
                                        return Page();
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "حدث خطأ أثناء التحقق من صلاحية الحساب.");
                                    return Page();
                                }
                            }

                        }

                        if (roles.Contains(SD.Role_Admin) || roles.Contains(SD.Role_Manager))
                        {
                            redirectUrl = Url.Action("IndexManager", "Index", new { area = "Admin" });
                        }
                        else if (roles.Contains(SD.Role_User))
                        {
                            redirectUrl = Url.Action("RedirectToIndexBrand", "Index", new { area = "Admin" ,  Id = userId });
                        }
                        else
                        {
                            var brandId = brand.BrandID;
                            redirectUrl = Url.Action("RedirectToUserInformation", "Home", new { area = "Customer", id = brandId });
                        }

                        if (!string.IsNullOrEmpty(redirectUrl))
                        {
                            _logger.LogInformation($"Redirecting to: {redirectUrl}");
                            return LocalRedirect(redirectUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Redirect URL is null or empty.");
                            // Handle the case where redirect URL is null or empty
                            return LocalRedirect(returnUrl);
                        }
                    }
                    // Default redirect if no specific role is found
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "خطأ أثناء تسجيل الدخول");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

