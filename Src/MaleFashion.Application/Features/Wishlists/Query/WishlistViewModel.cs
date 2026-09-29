using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Query
{
    public class WishlistViewModel
    {
        public IList<WishlistItemViewModel> Items { get; set; }
          = new List<WishlistItemViewModel>();


        public int TotalItems =>
            Items?.Count ?? 0;
    }
}

