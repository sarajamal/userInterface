// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Test12.Models.Models;

namespace UserInterface.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Code { get; set; }

            [Required(ErrorMessage = "حقل البريد الإلكتروني مطلوب.")]
            [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
            [Display(Name = "الإيميل")]
            public string Email { get; set; }

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
            
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {

                Input = new InputModel
                {
                    Code = code,
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "البريد الالكتروني غير مسجل لدينا عذرا ");
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
