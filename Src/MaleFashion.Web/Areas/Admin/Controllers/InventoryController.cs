using Cortex.Mediator;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Command;
using MaleFashion.Application.Features.Inventories.Query;
using MaleFashion.Infrastructure.Extensions;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Codes;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace MaleFashion.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {


        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;

        public InventoryController(
            IMediator mediator,
            IMapper mapper,
            ILogger<InventoryController> logger, IApplicationUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
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
        // GET PAGED INVENTORIES
        // ==========================================

      
public async Task<IActionResult> GetPagedInventories(
    [FromBody] InventoryQueryModel model,
    CancellationToken cancellationToken)
        {
            try
            {
                // ==========================================
                // MAP REQUEST
                // ==========================================

                var query =
                    _mapper.Map<
                        GetAllInventoriesByPagingQuery>(
                        model);


                
// ==========================================
// SEARCH
// ==========================================

         query.SearchText =
    model.Search.Value ?? string.Empty;

                _logger.LogInformation(
                    "Search = {Search}",
                    query.SearchText);

                // ==========================================
                // SORTING
                // ==========================================

                query.SortText =
                    model.FormatSortExpression(
                        "ProductName",
                        "Color",
                        "Size",
                        "Quantity",
                        "IsActive",
                        "UpdatedAt");


                // ==========================================
                // GET INVENTORY
                // ==========================================

                var result =
                    await _mediator
                        .SendQueryAsync(
                            query,
                            cancellationToken);


                // ==========================================
                // GET VARIANT IDS
                // ==========================================

                var inventoryList =
                    result.Data.ToList();


                var variantIds =
                    inventoryList
                        .Select(x =>
                            x.ProductVariantId)
                        .Distinct()
                        .ToList();


                // ==========================================
                // GET PRODUCT VARIANTS
                // ==========================================

                var variants =
                    await _unitOfWork
                        .InventoryRepository
                        .GetProductVariantsByIds(
                            variantIds,
                            cancellationToken);


                // ==========================================
                // CREATE DICTIONARY
                // ==========================================

                var variantDictionary =
                    variants.ToDictionary(
                        x => x.Id);


                // ==========================================
                // DATATABLE DATA
                // ==========================================

                var data =
                    inventoryList.Select(item =>
                    {
                        variantDictionary.TryGetValue(
                            item.ProductVariantId,
                            out var variant);


                        return new
                        {
                            id =
                               item.Id,

                            productName =
                              variant
                                    ?.Product
                                    ?.ProductName
                                ?? "",

                            color =
                             variant
                                    ?.Color
                                ?? "",

                            size =
                          variant
                                    ?.Size
                                ?? "",

                            quantity =
                             item.Quantity,

                            isActive =
                             item.IsActive,

                            updatedAt =
                                HttpUtility.HtmlEncode(item.UpdatedAt
                                  .AddHours(6)
                                  .ToString("dd-MM-yyyy hh:mm tt"))
                        };
                    });


                // ==========================================
                // RESPONSE
                // ==========================================

                return Json(new
                {
                    recordsTotal =
                        result.Total,

                    recordsFiltered =
                        result.TotalDisplay,

                    data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to load inventories.");


                return Json(new
                {
                    recordsTotal = 0,

                    recordsFiltered = 0,

                    data =
                        Array.Empty<object>()
                });
            }
        }



       

        // ==========================================
        // CREATE GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken)
        {
            var model = new InventoryModel
            {
                IsActive = true
            };


            await LoadProductVariants(
                model,
                cancellationToken);


            return View(model);
        }


        // ==========================================
        // CREATE POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            InventoryModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadProductVariants(
                    model,
                    cancellationToken);

                return View(model);
            }


            try
            {
                var command =
                    new InventoryAddCommand
                    {
                        ProductVariantId =
                            model.ProductVariantId,

                        Quantity =
                            model.Quantity,

                        IsActive =
                            model.IsActive
                    };


                var result =
                    await _mediator
                        .SendCommandAsync(
                            command,
                            cancellationToken);


                if (result == null)
                {
                    ModelState.AddModelError(
                        "ProductVariantId",
                        "Inventory already exists for this product variant.");

                    await LoadProductVariants(
                        model,
                        cancellationToken);

                    return View(model);
                }


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Inventory successfully created.",

                        Type =
                            ResponseTypes.Success
                    });


                return RedirectToAction(
                    nameof(Create));
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to create Inventory.";

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


                await LoadProductVariants(
                    model,
                    cancellationToken);

                return View(model);
            }
        }


        // ==========================================
        // UPDATE GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Update(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query =
                new GetInventoryByIdQuery
                {
                    Id = id
                };


            var inventory =
                await _mediator
                    .SendQueryAsync(
                        query,
                        cancellationToken);


            if (inventory == null)
            {
                return NotFound();
            }


            var model =
                new InventoryModel
                {
                    Id = inventory.Id,

                    ProductVariantId =
                        inventory.ProductVariantId,

                    Quantity =
                        inventory.Quantity,

                    IsActive =
                        inventory.IsActive,

                    UpdatedAt =
                        inventory.UpdatedAt,

                    ProductName =
                        inventory.ProductVariant
                            ?.Product
                            ?.ProductName
                        ?? string.Empty,

                    Color =
                        inventory.ProductVariant
                            ?.Color
                        ?? string.Empty,

                    Size =
                        inventory.ProductVariant
                            ?.Size
                        ?? string.Empty
                };


            return View(model);
        }


        // ==========================================
        // UPDATE POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            InventoryModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                var command =
                    new InventoryUpdateCommand
                    {
                        Id = model.Id,

                        ProductVariantId =
                            model.ProductVariantId,

                        Quantity =
                            model.Quantity,

                        IsActive =
                            model.IsActive
                    };


                var result =
                    await _mediator
                        .SendCommandAsync(
                            command,
                            cancellationToken);


                if (result == null)
                {
                    ModelState.AddModelError(
                        "ProductVariantId",
                        "Another inventory record already uses this product variant.");

                    return View(model);
                }


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Inventory successfully updated.",

                        Type =
                            ResponseTypes.Success
                    });


                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to update Inventory.";

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


        // ==========================================
        // DELETE
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var command =
                    new InventoryDeleteCommand
                    {
                        Id = id
                    };


                await _mediator
                    .SendCommandAsync(
                        command,
                        cancellationToken);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Inventory successfully deleted.",

                        Type =
                            ResponseTypes.Success
                    });
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to delete Inventory.";

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


            return RedirectToAction(
                nameof(Index));
        }


         

// ==========================================
// LOAD AVAILABLE PRODUCT VARIANTS
// ==========================================

private async Task LoadProductVariants(
    InventoryModel model,
    CancellationToken cancellationToken)
        {
            var query =
                new GetAvailableProductVariantsQuery();


            var variants =
                await _mediator
                    .SendQueryAsync(
                        query,
                        cancellationToken);


            model.ProductVariants =
                variants
                    .Select(x =>
                        new SelectListItem
                        {
                            Value =
                                x.Id.ToString(),

                            Text =
                                string.Join(
                                    " - ",
                                    new[]
                                    {
                                x.Product?.ProductName,
                                x.Color,
                                x.Size
                                    }
                                    .Where(x =>
                                        !string.IsNullOrWhiteSpace(x))
                            ),

                            Selected =
                                x.Id ==
                                model.ProductVariantId
                        })
                    .ToList();
        }
    }


    }





