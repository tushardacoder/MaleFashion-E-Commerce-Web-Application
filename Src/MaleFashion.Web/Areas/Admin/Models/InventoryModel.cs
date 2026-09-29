using MaleFashion.Domain.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion.Web.Areas.Admin.Models
{

    public class InventoryModel 
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Variant is required.")] public Guid ProductVariantId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")][Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")] public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty; 
        public string Size { get; set; } = string.Empty; 
        public DateTime UpdatedAt { get; set; }
        public IList<SelectListItem> ProductVariants { get; set; } = new List<SelectListItem>();
    }
}
