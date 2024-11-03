using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Test12.Models.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [ValidateNever]
        public string? BrandName { get; set; }
        public string? ExpirationDate { get; set; }
        public string? Note { get; set; }
        public string? RoleName { get; set; }
        public bool? isActive { get; set; }

    }
}
