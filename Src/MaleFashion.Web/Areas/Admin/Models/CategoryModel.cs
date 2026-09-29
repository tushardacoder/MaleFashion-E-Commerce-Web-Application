using MaleFashion.Domain.Utilities;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion.Web.Areas.Admin.Models
{
    public class CategoryModel

    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }


    }
}
