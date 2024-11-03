using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Test12.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.trade_mark;
using Microsoft.EntityFrameworkCore;

namespace UserInterface.Areas.Identity.Pages.Account
{
    public class UpdateUsersModel : PageModel
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


        public UpdateUsersModel(

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
            [ValidateNever]
            public string Id { get; set; }  // This will hold the Id from AspNetUsers

            [ValidateNever]
            [Display(Name = "�������")]
            public string Email { get; set; }
            [ValidateNever]
            [Display(Name = "��� ��������")]
            public string UserName { get; set; }

            [StringLength(100, ErrorMessage = "��� �� ���� ���� ������ ��� ����� {2} ����� ���� ���� �� {1} �����.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "���� ������ �������")]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).*$", ErrorMessage = "��� �� ����� ���� ������ ��� ��� ���� ���� ��� ����� ('A'-'Z') ���� ��� ����� ���� ���� ��� �����.")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "����� ���� ������")]
            [Compare("Password", ErrorMessage = "���� ������ ������ ���� ������ ��� ��������.")]
            public string ConfirmPassword { get; set; }

            [ValidateNever]
            public string Role { get; set; }

            [ValidateNever]
            public bool? isActive { get; set; } // ��� ���� ������
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> BrandList { get; set; }

            [ValidateNever]
            public string BrandName { get; set; }
            [DataType(DataType.Date)]
            [Display(Name = "����� ������ ��������")]
            public string? ExpirationDate { get; set; }  // ����� ������ ��������
        }
        // Handle form submission on POST

        public async Task<IActionResult> OnPostAsync(string username, string returnUrl = null)
        {
            var brands = _unitOfWork.TredMarketRepository.GetAll(); // GetAll ��� �� ���� IEnumerable �� IQueryable

            returnUrl ??= Url.Content("~/");

            // Load the existing user
            var user = await _userManager.FindByNameAsync(username);
            var clientId = user.Id;
            var getBrand = _unitOfWork.TredMarketRepository.Get(u => u.BrandName == user.BrandName);

            if (getBrand != null && getBrand.ClientID == null)
            {
                getBrand.ClientID = clientId;

                // ��� ���������
                _unitOfWork.TredMarketRepository.Update(getBrand);
                _unitOfWork.Save();
            }
            // Check if the email or username already exist for another user
            var emailExists = await _userManager.Users
            .Where(u => u.Email == Input.Email)
            .FirstOrDefaultAsync();
            var userNameExists = await _userManager.FindByNameAsync(Input.UserName);
            if ((emailExists != null && emailExists.Id != user.Id) || (userNameExists != null && userNameExists.Id != user.Id))
            {
                ModelState.AddModelError(string.Empty, "������� �� ��� �������� ������");

                // Repopulate the RoleList and BrandList before returning the page
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

            // Track if the email has changed
            bool emailChanged = user.Email != Input.Email;

            // Update user properties
            user.Email = Input.Email;
            user.UserName = Input.UserName;
            user.BrandName = Input.BrandName;

            // If the email has changed, send a confirmation email
            if (emailChanged)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "����� ����� �������",
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
                        <h2>����� ������ ����������</h2>
                        <p>������ ������ ���������� ���� ����� ��� ������ �����:</p>
                        <a class='email-button' href='{callbackUrl}'>����� ������ ����������</a>
                    </div>
                </div>
            </body>
            </html>");
            }

            // Only update the password if a new password is provided
            if (!string.IsNullOrWhiteSpace(Input.Password) && !string.IsNullOrWhiteSpace(Input.ConfirmPassword))
            {
                // Ensure passwords match
                if (Input.Password != Input.ConfirmPassword)
                {
                    // Repopulate the RoleList and BrandList before returning the page
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

                // Remove old password and add new one
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

                        // Repopulate the RoleList and BrandList before returning the page
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

            // Update roles if necessary
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.FirstOrDefault() != Input.Role)
            {
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }
                if (!string.IsNullOrEmpty(Input.Role))
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);
                    user.RoleName = Input.Role;
                }
            }

            // Update the user in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            TempData["success"] = "�� ������� �����";

            // Redirect to a confirmation page or back to the user management page
            return RedirectToPage(new { username = user.UserName, success = true });
        }

        // Load user data on GET
        public async Task<IActionResult> OnGetAsync(string username, string returnUrl = null)
        {
            var brands = _unitOfWork.TredMarketRepository.GetAll();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            // ������ ����� ExpirationDate �� string ��� DateTime
            DateTime? expirationDate = null;
            if (!string.IsNullOrEmpty(user.ExpirationDate))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(user.ExpirationDate, out parsedDate))
                {
                    expirationDate = parsedDate;
                }
            }

            // Populate the Input model with user data
            Input = new InputModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                BrandName = user.BrandName,
                isActive = user.isActive,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                ExpirationDate = user.ExpirationDate // �������� ������ �� string �� �������
            };

            // Populate RoleList with roles from the database
            Input.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,
                Selected = r.Name == Input.Role
            }).ToList();

            Input.BrandList = brands.Select(b => new SelectListItem
            {
                Text = b.BrandName,
                Value = b.BrandName,
                Selected = b.BrandName == user.BrandName
            }).ToList();

            return Page();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        //-----------------------����� ������---------------------------
        public async Task<IActionResult> OnPostActivateUserAsync()
        {
            // ��� �������� �������� ��� Id
            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null)
            {
                return NotFound(); // ��� �� ��� ������ ��� ��������
            }

            // ��� ���� �������� ��������
            var brands = _unitOfWork.TredMarketRepository.GetAll();

            // ��� ������� �������� ������ ���������
            var getBrand = _unitOfWork.TredMarketRepository.Get(u => u.BrandName == user.BrandName);

            // ����� ������
            user.isActive = true;
            user.ExpirationDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd"); // ����� ����� ������ ���� ���� 4 ����

            // ����� �������� �� ����� ��������
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return await RepopulateAndReturnPageGetDataError(brands); // ������� �� ����� �� �������
            }

            // ��� ���� ������� �������� ������ ��� ����� ��� ClientID� �� ������ ClientID
            if (getBrand != null && getBrand.ClientID == null)
            {
                getBrand.ClientID = user.Id; // ����� ClientID

                // ��� ��������� �� ������� ��������
                _unitOfWork.TredMarketRepository.Update(getBrand);
                _unitOfWork.Save();
            }

            // ����� ������ �� �������� �������
            return await RepopulateAndReturnPageGetDataSuccessful(brands);
        }


        private async Task<IActionResult> RepopulateAndReturnPageGetDataError(IEnumerable<Brands> brands)
        {
            // Repopulate the RoleList and BrandList before returning the page
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
            Input.isActive = true;

            // ��� ���� ������ء ���� �� ����� ����� ����� �������� ����� ������
            ModelState.Clear(); // ���� ���� ����� ����� �� ������
            ModelState.AddModelError(string.Empty, "��� ��� ����� ����� ������ ��������.");

            return Page();
        }
        private async Task<IActionResult> RepopulateAndReturnPageGetDataActive(IEnumerable<Brands> brands)
        {
            // Repopulate the RoleList and BrandList before returning the page
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
            Input.isActive = true;
            // ��� ���� ������ء ���� �� ����� ����� ����� �������� ����� ������
            ModelState.Clear(); // ���� ���� ����� ����� �� ������
            ModelState.AddModelError(string.Empty, "��� ������ ��� ! .");

            return Page();
        }
        private async Task<IActionResult> RepopulateAndReturnPageGetDataSuccessful(IEnumerable<Brands> brands)
        {
            var user = await _userManager.FindByIdAsync(Input.Id);

            // Repopulate the RoleList and BrandList before returning the page
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
            Input.isActive = true;
            // Set ExpirationDate to 3 minutes after the first login
            Input.ExpirationDate = user.ExpirationDate;

            var updateResult = await _userManager.UpdateAsync(user);
            // ��� ���� ������ء ���� �� ����� ����� ����� �������� ����� ������
            ModelState.Clear(); // ���� ���� ����� ����� �� ������

            TempData["success"] = "����� ������ �����";
            return Page();
        }

        //----------------------- ����� ����� ������---------------------------

        public async Task<IActionResult> OnPostDeactivateUserAsync()
        {
            var user = await _userManager.FindByIdAsync(Input.Id);
            var brands = _unitOfWork.TredMarketRepository.GetAll(); // GetAll ��� �� ���� IEnumerable �� IQueryable

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "�������� ��� �����.");
                return Page();
            }

            if (user.isActive == true)
            {
                user.isActive = false; // ����� ����� ������
                user.ExpirationDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

                // ����� ������ �� ����� ��������
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    // Repopulate the RoleList and BrandList before returning the page
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
                    Input.ExpirationDate = user.ExpirationDate;

                    ModelState.Clear(); // ���� ���� ����� ����� �� ������
                    ModelState.AddModelError(string.Empty, "��� ��� ����� ����� ������ ��������.");

                    return Page();
                }
                // Repopulate the RoleList and BrandList before returning the page
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
                _unitOfWork.Save();
                Input.isActive = false;
                Input.ExpirationDate = user.ExpirationDate;

                ModelState.Clear(); // ���� ���� ����� ����� �� ������
                TempData["success"] = "�� ����� ����� ������ �����.";
                return Page();
            }
            else
            {
                // Repopulate the RoleList and BrandList before returning the page
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
                ModelState.Clear(); // ���� ���� ����� ����� �� ������
                Input.ExpirationDate = user.ExpirationDate;
                Input.isActive = false;
                ModelState.AddModelError(string.Empty, "��� ������ ��� ��� ������.");

                return Page();
            }
        }

    }
}
