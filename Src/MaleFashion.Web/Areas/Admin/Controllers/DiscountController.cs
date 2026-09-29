using Cortex.Mediator;
using MaleFashion.Application.Exceptions;
using MaleFashion.Application.Features.Discounts.Command;
using MaleFashion.Application.Features.Discounts.Query;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MaleFashion.Infrastructure.Extensions;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Codes;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace MaleFashion.Web.Areas.Admin.Controllers
{


    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {

        private readonly ILogger<DiscountController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;


        public DiscountController(
            ILogger<DiscountController> logger,
            IMediator mediator,
            IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }


        // =====================================================
        // INDEX
        // =====================================================

        public IActionResult Index()
        {
            return View();
        }


        // =====================================================
        // CREATE - GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DiscountModel
            {
                StartAt = DateTime.Now,
                EndAt = DateTime.Now.AddDays(7),
                IsActive = true
            });
        }


        // =====================================================
        // CREATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DiscountModel model,
            CancellationToken cancellationToken)
        {

            // ==========================================
            // DATE VALIDATION
            // ==========================================

            if (model.EndAt <= model.StartAt)
            {
                ModelState.AddModelError(
                    nameof(model.EndAt),
                    "End date must be greater than Start date.");
            }


            // ==========================================
            // MODEL VALIDATION
            // ==========================================

            if (!ModelState.IsValid)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Please provide all required information.",

                        Type =
                            ResponseTypes.Danger
                    });

                return View(model);
            }


            try
            {

                // ==========================================
                // MAP MODEL → COMMAND
                // ==========================================

                var command = new DiscountAddCommand
                {
                    DiscountName =
                        model.DiscountName.Trim(),

                    Code =
                        model.Code
                            .Trim()
                            .ToUpperInvariant(),

                    DiscountPercentage =
                        model.DiscountPercentage,

                    StartAt =
                        model.StartAt,

                    EndAt =
                        model.EndAt,

                    IsActive =
                        model.IsActive
                };


                // ==========================================
                // SEND COMMAND
                // ==========================================

                var result =
                    await _mediator.SendCommandAsync(
                        command,
                        cancellationToken);


                // ==========================================
                // DUPLICATE CODE
                // ==========================================

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Discount code already exists.",

                            Type =
                                ResponseTypes.Danger
                        });

                    return View(model);
                }


                // ==========================================
                // SUCCESS MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Discount successfully created.",

                        Type =
                            ResponseTypes.Success
                    });


                // ==========================================
                // REDIRECT
                // ==========================================

                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {

                _logger.LogError(
                    ex,
                    "Failed to create discount.");


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Failed to create discount.",

                        Type =
                            ResponseTypes.Danger
                    });


                return View(model);
            }
        }


        // =====================================================
        // UPDATE - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Update(
            Guid id,
            CancellationToken cancellationToken)
        {
            try
            {

                // ==========================================
                // GET DISCOUNT
                // ==========================================

                var query = new GetDiscountByIdQuery
                {
                    Id = id
                };


                var discount =
                    await _mediator.SendQueryAsync<
                        GetDiscountByIdQuery,
                        Discount?>(
                            query,
                            cancellationToken);


                // ==========================================
                // CHECK DISCOUNT
                // ==========================================

                if (discount is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Discount doesn't exist.",

                            Type =
                                ResponseTypes.Danger
                        });

                    return RedirectToAction(nameof(Index));
                }


                // ==========================================
                // MAP TO VIEW MODEL
                // ==========================================

                var model = new DiscountModel
                {
                    Id =
                        discount.Id,

                    DiscountName =
                        discount.DiscountName,

                    Code =
                        discount.Code,

                    DiscountPercentage =
                        discount.DiscountPercentage,

                    StartAt =
                        discount.StartAt,

                    EndAt =
                        discount.EndAt,

                    IsActive =
                        discount.IsActive
                };


                // ==========================================
                // RETURN VIEW
                // ==========================================

                return View(model);
            }
            catch (Exception ex)
            {

                const string errorMessage =
                    "Failed to load discount.";


                _logger.LogError(
                    ex,
                    errorMessage);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            errorMessage,

                        Type =
                            ResponseTypes.Danger
                    });


                return RedirectToAction(nameof(Index));
            }
        }


        // =====================================================
        // UPDATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            DiscountModel model,
            CancellationToken cancellationToken)
        {

            // ==========================================
            // DATE VALIDATION
            // ==========================================

            if (model.EndAt <= model.StartAt)
            {
                ModelState.AddModelError(
                    nameof(model.EndAt),
                    "End date must be greater than Start date.");
            }


            // ==========================================
            // MODEL VALIDATION
            // ==========================================

            if (!ModelState.IsValid)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Please provide all required information.",

                        Type =
                            ResponseTypes.Danger
                    });

                return View(model);
            }


            try
            {

                // ==========================================
                // MAP MODEL → COMMAND
                // ==========================================

                var command =
                    new DiscountUpdateCommand
                    {
                        Id =
                            model.Id,

                        DiscountName =
                            model.DiscountName.Trim(),

                        Code =
                            model.Code
                                .Trim()
                                .ToUpperInvariant(),

                        DiscountPercentage =
                            model.DiscountPercentage,

                        StartAt =
                            model.StartAt,

                        EndAt =
                            model.EndAt,

                        IsActive =
                            model.IsActive
                    };


                // ==========================================
                // SEND UPDATE COMMAND
                // ==========================================

                var result =
                    await _mediator.SendCommandAsync(
                        command,
                        cancellationToken);


                // ==========================================
                // DUPLICATE CODE
                // ==========================================

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Another discount already uses this code.",

                            Type =
                                ResponseTypes.Danger
                        });

                    return View(model);
                }


                // ==========================================
                // SUCCESS MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Discount successfully updated.",

                        Type =
                            ResponseTypes.Success
                    });


                // ==========================================
                // REDIRECT
                // ==========================================

                return RedirectToAction(nameof(Index));
            }
            catch (DuplicateDataException ex)
            {

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            ex.Message,

                        Type =
                            ResponseTypes.Danger
                    });


                return View(model);
            }
            catch (Exception ex)
            {

                const string errorMessage =
                    "Failed to update discount.";


                _logger.LogError(
                    ex,
                    errorMessage);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            errorMessage,

                        Type =
                            ResponseTypes.Danger
                    });


                return View(model);
            }
        }


        // =====================================================
        // DELETE
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            try
            {

                // ==========================================
                // DELETE COMMAND
                // ==========================================

                var deleteCommand =
                    new DiscountDeleteCommand
                    {
                        Id = id
                    };


                await _mediator.SendCommandAsync(
                    deleteCommand);


                // ==========================================
                // SUCCESS MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Discount successfully deleted.",

                        Type =
                            ResponseTypes.Success
                    });
            }
            catch (Exception ex)
            {

                const string errorMessage =
                    "Failed to delete discount.";


                _logger.LogError(
                    ex,
                    errorMessage);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            errorMessage,

                        Type =
                            ResponseTypes.Danger
                    });
            }


            return RedirectToAction(nameof(Index));
        }



        // =====================================================
        // GET PAGED DISCOUNTS
        // =====================================================


        [HttpPost]
        public async Task<IActionResult> GetPagedDiscounts(
         [FromBody] DiscountQueryModel model)
        {
            try
            {
                // ==========================================
                // VALIDATE REQUEST
                // ==========================================

                if (model == null)
                {
                    return BadRequest(new
                    {
                        error = "Invalid request."
                    });
                }


                // ==========================================
                // MAP DATATABLE REQUEST
                // ==========================================

                var query =
                    _mapper.Map<GetAllDiscountsByPagingQuery>(
                        model);


                // ==========================================
                // PAGE
                // ==========================================

                query.PageIndex =
                    (model.Start / model.Length) + 1;

                query.PageSize =
                    model.Length;


                // ==========================================
                // SEARCH
                // ==========================================

                query.SearchText =
                    model.Search.Value ?? string.Empty;


                _logger.LogInformation(
                    "Discount Search = '{SearchText}'",
                    query.SearchText);


                // ==========================================
                // SORT
                // ==========================================

                if (model.Order == null ||
                    !model.Order.Any())
                {
                    query.SortText =
                        "DiscountName ASC";
                }
                else
                {
                    query.SortText =
                        model.FormatSortExpression(
                            "DiscountName",
                            "Code",
                            "DiscountPercentage",
                            "StartAt",
                            "EndAt",
                            "IsActive");
                }


                // ==========================================
                // FALLBACK SORT
                // ==========================================

                if (string.IsNullOrWhiteSpace(
                    query.SortText))
                {
                    query.SortText =
                        "DiscountName ASC";
                }


                _logger.LogInformation(
                    "Discount Filter => Search: {SearchText}, Sort: {SortText}",
                    query.SearchText,
                    query.SortText);


                // ==========================================
                // MEDIATOR
                // ==========================================

                var (items, total, totalDisplay) =
                    await _mediator.SendQueryAsync<
                        GetAllDiscountsByPagingQuery,
                        (IList<Discount>, int, int)>(
                        query);


                // ==========================================
                // NULL PROTECTION
                // ==========================================

                items ??= new List<Discount>();


                // ==========================================
                // RESPONSE
                // ==========================================

                var discounts = new
                {
                    

                    recordsTotal = total,

                    recordsFiltered = totalDisplay,

                    data = items
                        .Select(item => new
                        {
                            id = item.Id,

                            discountName =
                                HttpUtility.HtmlEncode(
                                    item.DiscountName),

                            code =
                                HttpUtility.HtmlEncode(
                                    item.Code),

                            discountAmount =
                                item.DiscountPercentage,

                            startAt =
                                item.StartAt.ToString(
                                    "dd-MM-yyyy HH:mm"),

                            endAt =
                                item.EndAt.ToString(
                                    "dd-MM-yyyy HH:mm"),

                            isActive =
                                item.IsActive
                        })
                        .ToArray()
                };


                return Json(discounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get discount list.");


                return StatusCode(
                    500,
                    new
                    {
                        error = ex.Message,

                        innerError =
                            ex.InnerException?.Message
                    });
            }
        }
    
}
}