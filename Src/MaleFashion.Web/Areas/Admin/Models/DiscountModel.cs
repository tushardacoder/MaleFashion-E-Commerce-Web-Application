using System.ComponentModel.DataAnnotations;

namespace MaleFashion.Web.Areas.Admin.Models
{
    public class DiscountModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Discount Name is required.")]
        public string DiscountName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Discount Code is required.")]
        public string Code { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "100000",
            ErrorMessage = "Discount Percentage must be greater than 0.")]
        public decimal DiscountPercentage { get; set; }


        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartAt { get; set; }


        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndAt { get; set; }


        public bool IsActive { get; set; }
    }
}
