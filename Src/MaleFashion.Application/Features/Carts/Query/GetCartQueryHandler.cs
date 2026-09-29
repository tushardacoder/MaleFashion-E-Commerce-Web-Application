using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Query
{
    public class GetCartQueryHandler
       : IQueryHandler<GetCartQuery, CartViewModel>
    {

        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetCartQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<CartViewModel> Handle(
            GetCartQuery query,
            CancellationToken cancellationToken)
        {
            var model = new CartViewModel();

            if (query.UserId == Guid.Empty)
                return model;


            // =====================================================
            // GET CART
            // =====================================================

            var cart =
                await _unitOfWork
                    .CartRepository
                    .GetCartByUserIdAsync(
                        query.UserId,
                        cancellationToken);


            if (cart == null)
                return model;


            // =====================================================
            // MAP CART ITEMS
            // =====================================================

            foreach (var item in cart.CartItems)
            {
                var variant = item.ProductVariant;

                if (variant == null)
                    continue;

                var product = variant.Product;

                if (product == null)
                    continue;


                var cartItem = new CartItemViewModel
                {
                    ProductVariantId =
                        item.ProductVariantId,

                    ProductName =
                        product.ProductName,

                    UnitPrice =
                        item.UnitPrice,

                    Quantity =
                        item.Quantity,

                    // Change these according to your
                    // actual ProductVariant properties
                    Size =
                        variant.Size,

                    Color =
                        variant.Color,

                    Sku =
                        variant.Sku,

                    Stock = variant.Inventory?.Quantity ?? 0,

                    ImageName =
                        variant.Images?
                .OrderBy(x => x.Id)
                .Select(x => x.ImageName)
              .FirstOrDefault() ?? string.Empty
                };

                

                model.Items.Add(cartItem);
            }


            // =====================================================
            // CALCULATE TOTAL
            // =====================================================

            model.Subtotal =
                model.Items.Sum(x => x.Total);

            model.Discount = 0;

            model.Total =
                model.Subtotal -
                model.Discount;


            return model;

        }
    }
}
