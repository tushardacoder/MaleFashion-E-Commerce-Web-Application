using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class GetOrderDetailsQueryHandler
       : IQueryHandler<
           GetOrderDetailsQuery,
           OrderDetailsViewModel>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderDetailsQueryHandler(
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<OrderDetailsViewModel> Handle(
            GetOrderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            // =====================================================
            // 1. GET ORDER
            // =====================================================

            var order =
                await _orderRepository.GetByIdAsync(
                    request.OrderId,
                    cancellationToken);


            // =====================================================
            // 2. ORDER NOT FOUND
            // =====================================================

            if (order == null)
            {
                throw new KeyNotFoundException(
                    "Order not found.");
            }


            // =====================================================
            // 3. SECURITY CHECK
            // =====================================================

            if (order.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to view this order.");
            }


            // =====================================================
            // 4. CREATE VIEW MODEL
            // =====================================================

            var viewModel =
                new OrderDetailsViewModel
                {
                    // ---------------------------------------------
                    // ORDER
                    // ---------------------------------------------

                    Id =
                        order.Id,

                    CreatedAt =
                        order.CreatedAt,


                    // ---------------------------------------------
                    // CUSTOMER
                    // ---------------------------------------------

                    FirstName =
                        order.FirstName,

                    LastName =
                        order.LastName,

                    Email =
                        order.Email,

                    Phone =
                        order.Phone,


                    // ---------------------------------------------
                    // SHIPPING
                    // ---------------------------------------------

                    Address =
                        order.Address,

                    TownCity =
                        order.TownCity,

                    CountryState =
                        order.CountryState,

                    PostcodeZip =
                        order.PostcodeZip,

                    OrderNotes =
                        order.OrderNotes,


                    // ---------------------------------------------
                    // PRICE
                    // ---------------------------------------------

                    Subtotal =
                        order.Subtotal,

                    DiscountAmount =
                        order.DiscountAmount,

                    Total =
                        order.Total,


                    // ---------------------------------------------
                    // PAYMENT
                    // ---------------------------------------------

                    PaymentType =
                        order.Payment?
                            .PaymentType
                            .ToString()
                        ?? "Unknown",

                    PaymentStatus =
                        order.Payment?
                            .Status
                            .ToString()
                        ?? "Pending",

                    PaymentAmount =
                        order.Payment?.Amount
                        ?? 0,

                    MobileNumber =
                        order.Payment?.MobileNumber,

                    TransactionId =
                        order.Payment?.TransactionId,

                    PaidAt =
                        order.Payment?.PaidAt
                };


            // =====================================================
            // 5. MAP ORDER ITEMS
            // =====================================================

            foreach (var item in order.OrderItems)
            {
                viewModel.Items.Add(
                    new OrderDetailsItemViewModel
                    {
                        Id =
                            item.Id,

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
                    });
            }


            // =====================================================
            // 6. RETURN
            // =====================================================

            return viewModel;
        }
    }
}
