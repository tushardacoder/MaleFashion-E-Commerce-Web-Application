using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{

    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<CartItem?> GetCartItemAsync(
            Guid cartId,
            Guid productVariantId,
            CancellationToken cancellationToken);

        Task AddCartAsync(
            Cart cart,
            CancellationToken cancellationToken);

        Task AddCartItemAsync(
            CartItem cartItem,
            CancellationToken cancellationToken);

        Task UpdateCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);
        Task DeleteCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);



        Task<Cart?> GetByUserIdWithItemsAsync(
        Guid userId,
        CancellationToken cancellationToken);

        Task ClearAsync(
            Guid cartId,
            CancellationToken cancellationToken);

    }
}
