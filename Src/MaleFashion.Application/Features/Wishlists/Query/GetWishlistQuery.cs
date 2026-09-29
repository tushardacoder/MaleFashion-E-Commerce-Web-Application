using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Query
{
    public class GetWishlistQuery
       : IQuery<WishlistViewModel>
    {
        public Guid UserId { get; set; }


        public GetWishlistQuery(
            Guid userId)
        {
            UserId = userId;
        }
    }
}


