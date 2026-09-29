using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaleFashion.Web.Areas.Customer.Models
{
    public class ContactUsModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Message { get; set; } = default!;

        public string RecaptchaToken { get; set; } = default!;
    }
}



