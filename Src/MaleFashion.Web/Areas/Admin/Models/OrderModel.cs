using MaleFashion.Domain.Utilities;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion.Web.Areas.Admin.Models
{
    public class OrderModel : DataTables
    {
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Wise Total is required.")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Product Wise Total must be greater than 0.")]
        public decimal ProductWiseTotal { get; set; }

        [Required(ErrorMessage = "Total is required.")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Total must be greater than 0.")]
        public decimal Total { get; set; }
    }
}
