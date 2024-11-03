// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Utility;

namespace UserInterface.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        //1- Add Roles To Table 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(

            UserManager<ApplicationUser> userManager,
            //2- Add Roles To Table 
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "حقل البريد الإلكتروني مطلوب.")]
            [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
            [Display(Name = "الإيميل")]
            public string Email { get; set; }
            [Required(ErrorMessage = "حقل اسم المستخدم مطلوب .")]
            [Display(Name = "اسم المستخدم")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "حقل كلمة المرور مطلوب.")]
            [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور على الأقل {2} حرفًا وألا تزيد عن {1} حرفًا.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).*$", ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف واحد كبير على الأقل ('A'-'Z') وحرف غير أبجدي رقمي واحد على الأقل.")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقين.")]
            public string ConfirmPassword { get; set; }

            //1- select Roles In Register Page 
            [ValidateNever]
            public string Role { get; set; }
            [ValidateNever]
            public string RoleName { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> BrandList { get; set; }
            [ValidateNever]
            public string BrandName { get; set; }
            public string? ExpirationDate { get; set; }
            public string? Note { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            var brands = _unitOfWork.TredMarketRepository.GetAll(); // GetAll يجب أن يعيد IEnumerable أو IQueryable

            // Add roles to roles table if they do not exist
            if (!_roleManager.RoleExistsAsync(SD.Role_Client).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Client)).GetAwaiter().GetResult();
            }

            // Populate RoleList for the registration form
            Input = new InputModel
            {

                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }).ToList(),
                // Populate BrandList for the registration form
                BrandList = brands.Select(b => new SelectListItem
                {
                    Text = b.BrandName,
                    Value = b.BrandName
                }).ToList()
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var brands = _unitOfWork.TredMarketRepository.GetAll(); // GetAll يجب أن يعيد IEnumerable أو IQueryable

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Check if email or username already exist
                var emailExists = await _userManager.FindByEmailAsync(Input.Email) != null;
                var userNameExists = await _userManager.FindByNameAsync(Input.UserName) != null;

                if (emailExists || userNameExists)
                {
                    // Add model error and return page
                    ModelState.AddModelError(string.Empty, "الإيميل أو اسم المستخدم مستخدم");
                    // Repopulate the RoleList before returning the page
                    Input.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();
                    // Populate BrandList for the registration form
                    Input.BrandList = brands.Select(b => new SelectListItem
                    {
                        Text = b.BrandName,
                        Value = b.BrandName
                    }).ToList();

                    return Page();
                }

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.UserName = Input.UserName;
                user.RoleName = Input.RoleName;
                user.BrandName = Input.BrandName;
                user.Note = Input.Note;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // If User Selects Role
                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        var roleName = Input.Role;
                        user.RoleName = roleName;
                        await _userManager.AddToRoleAsync(user, user.RoleName);
                    }
                    else
                    {
                        var roleName = SD.Role_Client;
                        user.RoleName = roleName;
                        await _userManager.AddToRoleAsync(user, user.RoleName);
                    }

                    if (user.RoleName == SD.Role_Client)
                    {
                        // البحث عن مستخدم بناءً على BrandName
                        var getIdbyBrandName = await _userManager.Users
                                                .Where(u => u.BrandName == user.BrandName)
                                                .FirstOrDefaultAsync();
                        var Brand = getIdbyBrandName.BrandName;
                        if (getIdbyBrandName != null)
                        {
                            // الحصول على معرف المستخدم (ClientID)
                            var clientId = getIdbyBrandName.Id;
                            var getBrand = _unitOfWork.TredMarketRepository.Get(u => u.BrandName == Brand);

                            if (getBrand != null)
                            {
                                getBrand.ClientID = clientId;

                                // حفظ التغييرات
                                _unitOfWork.TredMarketRepository.Update(getBrand);
                                _unitOfWork.Save();
                            }
                        }
                        // Set ExpirationDate to 1 day after registration only for Client role
                        user.ExpirationDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd");
                        user.isActive = true;
                    }
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm Email",
                        $@"
                <html>
                <head>
                    <style>
                        .email-container {{ font-family: Arial, sans-serif; max-width: 600px; margin: auto; }}
                        .email-content {{ text-align: center; }}
                        .email-button {{ background-color: #004aad; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                        .ii a[href] {{ color:#fff; }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-content'>
                            <h2>تأكيد البريد الإلكتروني</h2>
                            <p>لتأكيد البريد الإلكتروني يرجى النقر على الرابط أدناه:</p>
                            <a class='email-button' href='{callbackUrl}'>تأكيد البريد الإلكتروني</a>
                        </div>
                    </div>
                </body>
                </html>");
                    // Update the password if a new one is provided
                    if (!string.IsNullOrWhiteSpace(Input.Password))
                    {
                        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                        if (removePasswordResult.Succeeded)
                        {
                            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.Password);
                            if (!addPasswordResult.Succeeded)
                            {
                                foreach (var error in addPasswordResult.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                // Repopulate the RoleList before returning the page
                                Input.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                                {
                                    Value = r.Name,
                                    Text = r.Name
                                }).ToList();
                                Input.BrandList = brands.Select(b => new SelectListItem
                                {
                                    Text = b.BrandName,
                                    Value = b.BrandName
                                }).ToList();
                                return Page();
                            }
                        }
                    }
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // Redirect to the "IndexManager" page under the "Admin" area
                        return RedirectToAction("IndexManager", "Index", new { area = "Admin" });
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // Repopulate the RoleList before returning the page
            Input.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            Input.BrandList = brands.Select(b => new SelectListItem
            {
                Text = b.BrandName,
                Value = b.BrandName
            }).ToList();
            TempData["success"] = "تم الإضافة  بنجاح";
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
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
