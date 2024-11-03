using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test12.Models.Models.trade_mark
{
    public class Brands
    {
        [Key]
        public int BrandID { get; set; } = 0;

        [MaxLength(255)]
        public string? BrandName { get; set; }
        [ValidateNever]
        public string? BrandCoverImage { get; set; }
        public DateTime? Date1 { get; set; }

        [ValidateNever]
        public string? BrandLogoImage { get; set; }

        [ValidateNever]
        [MaxLength(255)]
        public string? CreatedBY { get; set; }
        [ValidateNever]
        [MaxLength(255)]
        public string? UserId { get; set; }
        [ValidateNever]
        public string? BrandFooterImage { get; set; }
        [ValidateNever]
        public string? ClientID { get; set; }
        [ForeignKey("ClientID")] // تحديد اسم خاصية التنقل "User"
        [ValidateNever]
        public ApplicationUser? User { get; set; } // خاصية التنقل
      

    }
}
