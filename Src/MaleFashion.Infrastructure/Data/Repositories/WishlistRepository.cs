using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{
    public class WishlistRepository : Repository<WishList,Guid>,IWishlistRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WishlistRepository(
            ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WishList?> GetByUserIdAsync(
          Guid userId,
          CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<WishList>()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);
        }


        // ==========================================
        // GET WISHLIST WITH FULL DETAILS
        // ==========================================
        public async Task<List<WishList>> GetByUserIdWithItemsAsync(
    Guid userId,
    CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<WishList>()
                .Where(x => x.UserId == userId)
                .Include(x => x.Items)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.Items)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Images)
                .Include(x => x.Items)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Inventory)
                .ToListAsync(cancellationToken);
        }

        // ==========================================
        // ADD WISHLIST
        // ==========================================




        // ==========================================
        // REMOVE WISHLIST ITEM
        // ==========================================

        public Task RemoveItemAsync(
            WishlistItem item,
            CancellationToken cancellationToken)
        {
            _dbContext
                .Set<WishlistItem>()
                .Remove(item);

            return Task.CompletedTask;
        }


        public async Task<WishlistItem?> GetItemByIdForUserAsync(
    Guid wishlistItemId,
    Guid userId,
    CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<WishlistItem>()
                .Include(x => x.Wishlist)
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == wishlistItemId &&
                        x.Wishlist.UserId == userId,
                    cancellationToken);
        }


        public async Task<int> GetWishlistItemCountAsync(Guid userId)
        {


            //var count = await _dbContext.WishlistItems
            //    .CountAsync(x => x.WishlistId == userId);

            //return count;

            var wishlists = await _dbContext.WishLists
        .Where(x => x.UserId == userId)
        .Select(x => x.Id)
        .ToListAsync();

            var items = await _dbContext.WishlistItems
                .Select(x => new
                {
                    x.Id,
                    x.WishlistId,
                    x.ProductVariantId
                })
                .ToListAsync();

            //Console.WriteLine($"CURRENT USER: {userId}");
            //Console.WriteLine($"WISHLIST COUNT: {wishlists.Count}");

            //foreach (var id in wishlists)
            //{
            //    Console.WriteLine($"WISHLIST ID: {id}");
            //}

            //Console.WriteLine($"TOTAL ITEMS: {items.Count}");

            //foreach (var item in items)
            //{
            //    Console.WriteLine(
            //        $"ITEM: {item.Id} | " +
            //        $"WishlistId: {item.WishlistId} | " +
            //        $"VariantId: {item.ProductVariantId}");
            //}

            return items.Count(x => wishlists.Contains(x.WishlistId));
        }

    }
}










