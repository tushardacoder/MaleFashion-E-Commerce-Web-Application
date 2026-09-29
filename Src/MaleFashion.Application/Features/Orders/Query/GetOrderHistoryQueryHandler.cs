using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{

    public class GetOrderHistoryQueryHandler
        : IQueryHandler<
            GetOrderHistoryQuery,
            OrderHistoryViewModel>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderHistoryQueryHandler(
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<OrderHistoryViewModel> Handle(
            GetOrderHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var orders =
                await _orderRepository.GetByUserIdAsync(
                    request.UserId,
                    cancellationToken);


            var viewModel =
                new OrderHistoryViewModel();


            foreach (var order in orders)
            {
                viewModel.Orders.Add(
                    new OrderHistoryItemViewModel
                    {
                        Id = order.Id,

                        CreatedAt =
                            order.CreatedAt,

                        Subtotal =
                            order.Subtotal,

                        DiscountAmount =
                            order.DiscountAmount,

                        Total =
                            order.Total,

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

                        ItemCount =
                            order.OrderItems
                                .Sum(x => x.Quantity)
                    });
            }


            return viewModel;
        }

    }
    }
