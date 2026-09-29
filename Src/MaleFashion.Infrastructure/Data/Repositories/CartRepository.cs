using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Cart?> GetCartByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            //return await _dbContext.Carts
            //    .Include(x => x.CartItems)
            //    .FirstOrDefaultAsync(
            //        x => x.UserId == userId,
            //        cancellationToken);

            //return await _dbContext.Carts.Include(x => x.CartItems)
            //    .ThenInclude(x => x.ProductVariant)
            //    .ThenInclude(x => x.Product)
            //    .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            return await _dbContext.Carts
       .Include(x => x.CartItems)
           .ThenInclude(x => x.ProductVariant)
               .ThenInclude(x => x.Product)

       .Include(x => x.CartItems)
           .ThenInclude(x => x.ProductVariant)
               .ThenInclude(x => x.Images)

       .FirstOrDefaultAsync(
           x => x.UserId == userId,
           cancellationToken);
        }


        public async Task<CartItem?> GetCartItemAsync(
            Guid cartId,
            Guid productVariantId,
            CancellationToken cancellationToken)
        {
            //return await _dbContext.CartItems
            //    .FirstOrDefaultAsync(
            //        x =>
            //            x.CartId == cartId &&
            //            x.ProductVariantId == productVariantId,
            //        cancellationToken);

            return await _dbContext.CartItems.Include(x => x.ProductVariant)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductVariantId == productVariantId, cancellationToken);
        }


        public async Task AddCartAsync(
            Cart cart,
            CancellationToken cancellationToken)
        {
            await _dbContext.Carts.AddAsync(
                cart,
                cancellationToken);
        }


        public async Task AddCartItemAsync(
            CartItem cartItem,
            CancellationToken cancellationToken)
        {
            await _dbContext.CartItems.AddAsync(
                cartItem,
                cancellationToken);
        }





        // ========================================================= // UPDATE CART ITEM // =========================================================
        public Task UpdateCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            _dbContext.CartItems.Update(cartItem);
            return Task.CompletedTask;
        }


        // ========================================================= // DELETE CART ITEM // =========================================================

        public Task DeleteCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            _dbContext.CartItems.Remove(cartItem); 
            return Task.CompletedTask; 
        }



        //for order
        public async Task<Cart?> GetByUserIdWithItemsAsync(
      Guid userId,
      CancellationToken cancellationToken)
        {
            return await _dbContext.Carts

                .Include(x => x.CartItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)

                .Include(x => x.CartItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Inventory)

                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);
        }

        public async Task ClearAsync(
            Guid cartId,
            CancellationToken cancellationToken)
        {
            var items = await _dbContext.CartItems
                .Where(x => x.CartId == cartId)
                .ToListAsync(cancellationToken);

            if (items.Any())
            {
                _dbContext.CartItems.RemoveRange(items);
            }
        }

    }
}
