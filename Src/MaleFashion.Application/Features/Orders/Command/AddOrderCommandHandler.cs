using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Command
{
    public class AddOrderCommandHandler
    : ICommandHandler<AddOrderCommand, Guid>
    {
        

        private readonly IApplicationUnitOfWork
            _unitOfWork;

        private readonly ITransactionManager
            _transactionManager;


        public AddOrderCommandHandler(
          
            IApplicationUnitOfWork unitOfWork,
            ITransactionManager transactionManager)
        {
            

            _unitOfWork =
                unitOfWork;

            _transactionManager =
                transactionManager;
        }


        public async Task<Guid> Handle(
            AddOrderCommand command,
            CancellationToken cancellationToken)
        {
            // ======================================================
            // 1. GET USER ID
            // ======================================================

            var userId =
                command.UserId;

            if (!userId.HasValue)
            {
                return Guid.Empty;
            }


            // ======================================================
            // 2. LOAD CART
            // ======================================================

            var cart =
                await _unitOfWork.CartRepository
                    .GetByUserIdWithItemsAsync(
                        userId.Value,
                        cancellationToken);

            if (cart == null)
            {
                return Guid.Empty;
            }


            // ======================================================
            // 3. CHECK CART EMPTY
            // ======================================================

            if (cart.CartItems == null ||
                !cart.CartItems.Any())
            {
                return Guid.Empty;
            }


            // ======================================================
            // 4. VALIDATE CART ITEMS
            // ======================================================

            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Quantity <= 0)
                {
                    return Guid.Empty;
                }

                if (cartItem.ProductVariant == null)
                {
                    return Guid.Empty;
                }
            }


            // ======================================================
            // 5. CALCULATE SUBTOTAL
            // ======================================================

            decimal subtotal = 0m;

            foreach (var cartItem in cart.CartItems)
            {
                subtotal +=
                    cartItem.UnitPrice *
                    cartItem.Quantity;
            }


            // ======================================================
            // 6. COUPON
            // ======================================================

            decimal discountPercentage = 0m;


            if (!string.IsNullOrWhiteSpace(
                command.CouponCode))
            {
                var coupon = await _unitOfWork.DiscountRepository.GetActiveDiscountAsync(
        DateTime.Now.AddHours(6),
       cancellationToken);

                if (coupon == null)
                {
                    return Guid.Empty;
                }

                if (!coupon.IsActive)
                {
                    return Guid.Empty;
                }

                var now = DateTime.UtcNow;

                if (now < coupon.StartAt || now > coupon.EndAt)
                {
                    return Guid.Empty;
                }

                discountPercentage = coupon.DiscountPercentage;
            }

            
            // ======================================================
            // 7. DISCOUNT
            // ======================================================

            var discountAmount = 
                subtotal *
                discountPercentage /
                100m;


            // ======================================================
            // 8. TOTAL
            // ======================================================

            var total =
                Math.Max(
                    0m,
                    subtotal - discountAmount);


            // ======================================================
            // 9. BEGIN TRANSACTION
            // ======================================================

            await _transactionManager
                .BeginTransactionAsync(
                    cancellationToken);

            try
            {
                // ==================================================
                // 10. DECREASE STOCK
                // ==================================================

                foreach (var cartItem in cart.CartItems)
                {
                    var stockUpdated =
                        await _unitOfWork.InventoryRepository
                            .DecreaseStockAsync(
                                cartItem.ProductVariantId,
                                cartItem.Quantity,
                                cancellationToken);

                    if (!stockUpdated)
                    {
                        await _transactionManager
                            .RollbackTransactionAsync(
                                cancellationToken);

                        return Guid.Empty;
                    }
                }


                // ==================================================
                // 11. CREATE ORDER
                // ==================================================

                var order =
                    new Order
                    {
                        Id =
                             IdentityGenerator.NewSequentialGuid(),

                        UserId =
                            userId.Value,

                        FirstName =
                            command.FirstName,

                        LastName =
                            command.LastName,

                        Email =
                            command.Email,

                        Phone =
                            command.Phone,

                        Address =
                            command.Address,

                        TownCity =
                            command.TownCity,

                        CountryState =
                            command.CountryState,

                        PostcodeZip =
                            command.PostcodeZip,

                        OrderNotes =
                            command.OrderNotes,

                        Subtotal =
                            subtotal,

                        DiscountAmount =
                            discountAmount,

                        Total =
                            total,

                        CreatedAt =
                            DateTime.UtcNow
                    };


                // ==================================================
                // 12. CREATE ORDER ITEMS
                // ==================================================

                foreach (var cartItem in cart.CartItems)
                {
                    var variant =
                        cartItem.ProductVariant;

                    var product =
                        variant.Product;

                    var productName =
                        product?.ProductName
                        ?? "Product";

                    var itemTotal =
                        cartItem.UnitPrice *
                        cartItem.Quantity;


                    var orderItem =
                        new OrderItem
                        {
                            Id =
                                 IdentityGenerator.NewSequentialGuid(),

                            OrderId =
                                order.Id,

                            ProductVariantId =
                                cartItem.ProductVariantId,

                            ProductName =
                                productName,

                            Sku =
                                variant.Sku,

                            Color =
                                variant.Color,

                            Size =
                                variant.Size,

                            UnitPrice =
                                cartItem.UnitPrice,

                            Quantity =
                                cartItem.Quantity,

                            TotalPrice =
                                itemTotal
                        };


                    order.OrderItems.Add(
                        orderItem);
                }


                // ==================================================
                // 13. CREATE PAYMENT
                // ==================================================

                var isBkash =
                    command.PaymentType ==
                    PaymentType.Bkash;


                var payment =
                    new Payment
                    {
                        Id =
                            IdentityGenerator.NewSequentialGuid(),

                        OrderId =
                            order.Id,

                        PaymentType =
                            command.PaymentType,

                        Amount =
                            total,

                        MobileNumber =
                            isBkash
                                ? command.MobileNumber
                                : null,

                        TransactionId =
                            isBkash
                                ? command.TransactionId
                                : null,

                        Status =
                            isBkash
                                ? PaymentStatus.Paid
                                : PaymentStatus.Pending,

                        CreatedAt =
                            DateTime.UtcNow,

                        PaidAt =
                            isBkash
                                ? DateTime.UtcNow
                                : null
                    };


                order.Payment =
                    payment;


                // ==================================================
                // 14. ADD ORDER
                // ==================================================

                await _unitOfWork.OrderRepository
                    .AddAsync(
                        order,
                        cancellationToken);


                // ==================================================
                // 15. CLEAR CART
                // ==================================================

                await _unitOfWork.CartRepository
                    .ClearAsync(
                        cart.Id,
                        cancellationToken);


                // ==================================================
                // 16. SAVE
                // ==================================================

                await _unitOfWork
                    .SaveAsync(
                        cancellationToken);


                // ==================================================
                // 17. COMMIT
                // ==================================================

                await _transactionManager
                    .CommitTransactionAsync(
                        cancellationToken);


                // ==================================================
                // 18. SUCCESS
                // ==================================================

                return order.Id;
            }
            catch
            {
                // ==================================================
                // 19. ROLLBACK
                // ==================================================

                await _transactionManager
                    .RollbackTransactionAsync(
                        cancellationToken);

                return Guid.Empty;
            }
        }
    }

    }


