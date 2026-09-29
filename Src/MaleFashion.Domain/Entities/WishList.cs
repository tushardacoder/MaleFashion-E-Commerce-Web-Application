using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class WishList : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }


        // =====================================
        // User
        // =====================================

        public Guid UserId { get; set; }


        // =====================================
        // Wishlist Items
        // =====================================

        public ICollection<WishlistItem> Items { get; set; }
            = new List<WishlistItem>();
    }
}
