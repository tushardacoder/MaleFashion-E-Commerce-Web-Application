using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{

    public class GetOrderDetailsAdminQueryHandler
       : IQueryHandler<
           GetOrderDetailsAdminQuery,
           OrderDetailsAdminViewModel?>
    {
        private readonly IApplicationUnitOfWork
            _applicationUnitOfWork;

        public GetOrderDetailsAdminQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork =
                applicationUnitOfWork;
        }

        public async Task<OrderDetailsAdminViewModel?> Handle(
            GetOrderDetailsAdminQuery query,
            CancellationToken cancellationToken)
        {
            // 1. Get order from database
            var order =
                await _applicationUnitOfWork
                    .OrderRepository
                    .GetOrderDetailsAsync(
                        query.OrderId,
                        cancellationToken);

            // 2. Order not found
            if (order == null)
            {
                return null;
            }

            // 3. Map Order -> Admin ViewModel
            var model =
                new OrderDetailsAdminViewModel
                {
                    Id = order.Id,

                    UserId = order.UserId,

                    // CUSTOMER
                    FirstName = order.FirstName,

                    LastName = order.LastName,

                    Email = order.Email,

                    Phone = order.Phone,

                    // SHIPPING
                    Address = order.Address,

                    TownCity = order.TownCity,

                    CountryState = order.CountryState,

                    PostcodeZip = order.PostcodeZip,

                    OrderNotes = order.OrderNotes,

                    // PRICE
                    Subtotal = order.Subtotal,

                    DiscountAmount = order.DiscountAmount,

                    Total = order.Total,

                    // DATE
                    CreatedAt = order.CreatedAt,

                    // ITEMS
                    OrderItems =
                        order.OrderItems
                            .Select(item =>
                                new OrderItemDetailsAdminViewModel
                                {
                                    Id = item.Id,

                                    ProductVariantId =
                                        item.ProductVariantId,

                                    ProductName =
                                        item.ProductName,

                                    Color =
                                        item.Color,

                                    Size =
                                        item.Size,

                                    Sku =
                                        item.Sku,

                                    UnitPrice =
                                        item.UnitPrice,

                                    Quantity =
                                        item.Quantity,

                                    TotalPrice =
                                        item.TotalPrice
                                })
                            .ToList()
                };

            // 4. Payment
            if (order.Payment != null)
            {
                model.Payment =
                    new PaymentDetailsAdminViewModel
                    {
                        Id =
                            order.Payment.Id,

                        PaymentType =
                            order.Payment.PaymentType,

                        Amount =
                            order.Payment.Amount,

                        MobileNumber =
                            order.Payment.MobileNumber,

                        TransactionId =
                            order.Payment.TransactionId,

                        Status =
                            order.Payment.Status,

                        CreatedAt =
                            order.Payment.CreatedAt,

                        PaidAt =
                            order.Payment.PaidAt
                    };
            }

            // 5. Return admin details
            return model;
        }
    }
}
