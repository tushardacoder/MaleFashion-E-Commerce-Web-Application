using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Query
{
    public class GetWishlistQueryHandler
       : IQueryHandler<
           GetWishlistQuery,
           WishlistViewModel>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;


        public GetWishlistQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<WishlistViewModel> Handle(
            GetWishlistQuery query,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // ==========================================
            // VALIDATE USER
            // ==========================================

            if (query.UserId == Guid.Empty)
            {
                return new WishlistViewModel();
            }


            // ==========================================
            // GET ALL USER WISHLISTS
            // ==========================================

            var wishlists =
                await _unitOfWork
                    .WishlistRepository
                    .GetByUserIdWithItemsAsync(
                        query.UserId,
                        cancellationToken);


            // ==========================================
            // NO WISHLISTS
            // ==========================================

            if (wishlists == null ||
                wishlists.Count == 0)
            {
                return new WishlistViewModel();
            }


            // ==========================================
            // GET ALL ITEMS FROM ALL WISHLISTS
            // ==========================================

            var items =
                wishlists
                    .SelectMany(x => x.Items)
                    .Where(
                        x =>
                            x.ProductVariant != null &&
                            x.ProductVariant.Product != null)
                    .Select(
                        item =>
                        {
                            var variant =
                                item.ProductVariant;

                            var product =
                                variant.Product;


                            // ==================================
                            // IMAGE
                            // ==================================

                            var image =
                                variant.Images?
                                    .OrderBy(
                                        x => x.DisplayOrder)
                                    .Select(
                                        x => x.ImageName)
                                    .FirstOrDefault();


                            // ==================================
                            // STOCK
                            // ==================================

                            var isInStock =
                                variant.Inventory != null &&
                                variant.Inventory.Quantity > 0;


                            // ==================================
                            // VIEW MODEL
                            // ==================================

                            return new WishlistItemViewModel
                            {
                                Id = item.Id,

                                ProductId =
                                    product.Id,

                                ProductVariantId =
                                    variant.Id,

                                ProductName =
                                    product.ProductName,

                                Branding =
                                    product.Branding,

                                Color =
                                    variant.Color,

                                Size =
                                    variant.Size,

                                Sku =
                                    variant.Sku,

                                Price =
                                    product.ProductPrize,

                                Image =
                                    image,

                                AddedAt =
                                    item.AddedAt,

                                IsInStock =
                                    isInStock
                            };
                        })
                    .ToList();


            // ==========================================
            // RETURN
            // ==========================================

            return new WishlistViewModel
            {
                Items = items
            };

        }
    }
}


