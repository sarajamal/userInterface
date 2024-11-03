using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Test12.Models.Models.trade_mark;

namespace Test12.Models.Models
{
    public class MainSections
    {
        [Key]
        public int MainSectionsID { get; set; }
        public double? MainSectionsOrder { get; set; }
        [MaxLength(255)]
        public string? SectionsName { get; set; }
        public string? SectionsImage { get; set; }
        [NotMapped]
        [ValidateNever]
        public bool IsChecked { get; set; } // New property

        public int? BrandFK { get; set; }
        [ForeignKey("BrandFK")]
        [ValidateNever]
        public Brands? Brand { get; set; }

    }
}
