using MaleFashion.Domain.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion.Web.Areas.Admin.Models
{
    public class ProductModel 
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = default!;

        [Required]
        public string Branding { get; set; } = default!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ProductPrize { get; set; }

        public string TagsText { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = default!;

        public string CustomerPreview { get; set; } = default!;

        public string AdditionalInfo { get; set; } = default!;

        public bool IsActive { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        public List<ProductVariantModel> Variants { get; set; }
            = new();


    }

    public class ProductVariantModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Sku { get; set; } = default!;

        [Required]
        public string Size { get; set; } = default!;

        [Required]
        public string Color { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public List<ProductImageModel> Images { get; set; }
            = new();
    }

    public class ProductImageModel
    {
        public Guid? Id { get; set; }

        public string? ImageName { get; set; }

        public IFormFile? Image { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
    }
}