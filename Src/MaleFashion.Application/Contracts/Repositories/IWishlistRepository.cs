using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{

    public interface IWishlistRepository : IRepository<WishList, Guid>
    {

        Task<WishList?> GetByUserIdAsync(
         Guid userId,
         CancellationToken cancellationToken);

        Task<List<WishList>> GetByUserIdWithItemsAsync(
    Guid userId,
    CancellationToken cancellationToken);


        Task<WishlistItem?> GetItemByIdForUserAsync(
       Guid wishlistItemId,
       Guid userId,
       CancellationToken cancellationToken);
        Task RemoveItemAsync(
            WishlistItem item,
            CancellationToken cancellationToken);


        Task<int> GetWishlistItemCountAsync(Guid userId);


    }
}


