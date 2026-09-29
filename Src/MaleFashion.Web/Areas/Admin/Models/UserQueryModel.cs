using MaleFashion.Domain.Utilities;

namespace MaleFashion.Web.Areas.Admin.Models
{
    public class UserQueryModel : DataTables
    {
        public string? SearchText { get; set; }

    }
}
