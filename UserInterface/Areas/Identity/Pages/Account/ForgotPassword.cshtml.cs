// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Test12.Models.Models;

namespace UserInterface.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "البريد الالكتروني غير مسجل ");
                }
                else
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "إعادة تعيين كلمة المرور",
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
                        <h2>إعادة تعيين كلمة المرور</h2>
                        <p>لقد طلبت إعادة تعيين كلمة المرور لحسابك. يرجى النقر على الرابط أدناه لتعيين كلمة مرور جديدة:</p>
                        <a class='email-button' href='{callbackUrl}'>إعادة تعيين كلمة المرور</a>
                    </div>
                </div>
            </body>
            </html>");

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

            }
            return Page();
        }
    }
}

