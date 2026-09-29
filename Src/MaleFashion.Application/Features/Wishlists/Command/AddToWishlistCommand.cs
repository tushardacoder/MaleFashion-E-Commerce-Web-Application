using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Command
{
    public class AddToWishlistCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }

        public Guid ProductVariantId { get; set; }


        
       
    }
}
