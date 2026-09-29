using Cortex.Mediator;
using MaleFashion.Application.Features.Carts.Query;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace MaleFashion.Web.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {

        private readonly IMediator _mediator;

        public CartSummaryViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdString =
                HttpContext.User
                    .FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return View(new CartSummaryDTO
                {
                    Quantity = 0,
                    Total = 0
                });
            }

            var query = new GetCartQuery
            {
                UserId = userId
            };

            var cart = await _mediator.SendQueryAsync<
                GetCartQuery,
                CartViewModel>(query);

            var model = new CartSummaryDTO
            {
                Quantity = cart?.Items?.Sum(x => x.Quantity) ?? 0,
                Total = cart?.Total ?? 0
            };

            return View(model);
        }
    }
}
