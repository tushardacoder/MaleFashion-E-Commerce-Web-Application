using Cortex.Mediator;
using MaleFashion.Application.Features.Orders.Query;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MaleFashion.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace MaleFashion.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private readonly ILogger<OrderController>
            _logger;

        private readonly IMediator
            _mediator;

        private readonly IMapper
            _mapper;


        public OrderController(
            IMediator mediator,
            IMapper mapper,
            ILogger<OrderController> logger)
        {
            _mediator = mediator;

            _mapper = mapper;

            _logger = logger;
        }


        // ==========================================
        // INDEX
        // ==========================================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // ==========================================
        // PAGED ORDERS
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> GetPagedOrders(
            [FromBody] OrderQueryModel model)
        {
            try
            {
                var query =
                    _mapper.Map<
                        GetAllOrdersByPagingQuery>(
                            model);


                // SEARCH

                query.SearchText =
                    model.Search.Value;


                // SORT

                query.SortText =
                    model.FormatSortExpression(
                        "FirstName",
                        "Email",
                        "Phone",
                        "Subtotal",
                        "DiscountAmount",
                        "Total",
                        "CreatedAt");


                // QUERY

                var (
                    items,
                    total,
                    totalDisplay) =
                    await _mediator
                        .SendQueryAsync<
                            GetAllOrdersByPagingQuery,
                            (IList<Order>, int, int)>(
                                query);


                // RESPONSE

                var orders =
                    new
                    {
                        recordsTotal =
                            total,

                        recordsFiltered =
                            totalDisplay,

                        data =
                            items
                                .Select(item =>
                                    new string[]
                                    {
                                        item.Id.ToString(),

                                        HttpUtility
                                            .HtmlEncode(
                                                $"{item.FirstName} {item.LastName}"),

                                        HttpUtility
                                            .HtmlEncode(
                                                item.Email),

                                        HttpUtility
                                            .HtmlEncode(
                                                item.Phone),

                                        $"৳{item.Subtotal:N2}",

                                        $"৳{item.DiscountAmount:N2}",

                                        $"৳{item.Total:N2}",

                                        item.CreatedAt
                                            .AddHours(6)
                                            .ToString(
                                                "dd-MM-yyyy hh:mm tt"),

                                        item.Id.ToString()
                                    })
                                .ToArray()
                    };


                return Json(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get order list");


                return Json(
                    DataTables.EmptyResult);
            }
        }


        // ==========================================
        // DETAILS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Details(
            Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var query =
                    new GetOrderDetailsAdminQuery
                    {
                        OrderId = id
                    };


                var model =
                    await _mediator
                        .SendQueryAsync<
                            GetOrderDetailsAdminQuery,
                            OrderDetailsAdminViewModel?>(
                                query);


                if (model == null)
                {
                    return NotFound();
                }


                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get order details for {OrderId}",
                    id);


                return RedirectToAction(
                    nameof(Index));
            }
        }


    }
}


