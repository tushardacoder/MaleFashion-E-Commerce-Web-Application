using Cortex.Mediator;
using MaleFashion.Application.Exceptions;
using MaleFashion.Application.Features.Categories.Command;
using MaleFashion.Application.Features.Categories.Query;
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
    public class CategoryController : Controller
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        


        public CategoryController(ILogger<CategoryController> logger,
           IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
         
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryModel
            {
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    CategoryModel model,
    CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Please provide all required information.",
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }

            try
            {
                var command = new CategoryAddCommand
                {
                    CategoryName = model.CategoryName,
                    IsActive = model.IsActive
                };

                var result = await _mediator.SendCommandAsync(command);

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = "Category already exists.",
                            Type = ResponseTypes.Danger
                        });

                    return View(model);
                }

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Category successfully created.",
                        Type = ResponseTypes.Success
                    });

                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category.");

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Failed to create category.",
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetPagedCategories(
     [FromBody] CategoryQueryModel model)
        {
            try
            {
                // ==========================================
                // MAP DATATABLE REQUEST
                // ==========================================

                var query =
                    _mapper.Map<GetAllCategoriesByPagingQuery>(
                        model);


                // ==========================================
                // SEARCH
                // ==========================================

                query.SearchText = model.Search.Value ?? string.Empty;
                _logger.LogInformation(
    "Search = {Search}",
    query.SearchText);

                query.SortText =
                    model.FormatSortExpression(
                        "CategoryName",
                        "IsActive");


                // ==========================================
                // SORTING
                // ==========================================

                query.SortText =
                    model.FormatSortExpression(
                        "CategoryName",
                        "IsActive");


                // ==========================================
                // SEND QUERY
                // ==========================================

                var (items, total, totalDisplay) =
                    await _mediator.SendQueryAsync<
                        GetAllCategoriesByPagingQuery,
                        (IList<Category>, int, int)>(
                            query);


                // ==========================================
                // DATATABLES RESPONSE
                // ==========================================


                var categories = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,

                    data = items.Select(item => new
                    {
                        id = item.Id,
                        categoryName =
                            HttpUtility.HtmlEncode(item.CategoryName),
                        isActive = item.IsActive
                    }).ToArray()
                };


                return Json(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get category list");

                return Json(
                    DataTables.EmptyResult);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(
    Guid id,
    CancellationToken cancellationToken)
        {
            try
            {
                // ==========================================
                // GET CATEGORY
                // ==========================================

                var query = new GetCategoryByIdQuery
                {
                    Id = id
                };

                var result =
                    await _mediator.SendQueryAsync(
                        query,
                        cancellationToken);


                // ==========================================
                // CHECK CATEGORY
                // ==========================================

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = "Category doesn't exist.",
                            Type = ResponseTypes.Danger
                        });

                    return RedirectToAction(nameof(Index));
                }


                // ==========================================
                // MAP TO VIEW MODEL
                // ==========================================

                var model =
                    _mapper.Map<CategoryModel>(result);


                // ==========================================
                // RETURN VIEW
                // ==========================================

                return View(model);
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to load category.";

                _logger.LogError(
                    ex,
                    errorMessage);

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = errorMessage,
                        Type = ResponseTypes.Danger
                    });

                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
    CategoryModel model,
    CancellationToken cancellationToken)
        {
            // ==========================================
            // MODEL VALIDATION
            // ==========================================

            if (!ModelState.IsValid)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Please provide all information.",
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }


            try
            {
                // ==========================================
                // CHECK CATEGORY EXISTS
                // ==========================================

                var query = new GetCategoryByIdQuery
                {
                    Id = model.Id
                };

                var category =
                    await _mediator.SendQueryAsync(
                        query,
                        cancellationToken);


                if (category is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = "Category doesn't exist.",
                            Type = ResponseTypes.Danger
                        });

                    return RedirectToAction(nameof(Index));
                }


                // ==========================================
                // MAP MODEL → COMMAND
                // ==========================================

                var command =
                    _mapper.Map<CategoryUpdateCommand>(
                        model);


                // ==========================================
                // SEND UPDATE COMMAND
                // ==========================================

                await _mediator.SendCommandAsync(
                    command,
                    cancellationToken);


                // ==========================================
                // SUCCESS MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Category successfully updated.",
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
                        Message = ex.Message,
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to update category.";

                _logger.LogError(
                    ex,
                    errorMessage);

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = errorMessage,
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleteCommand = new CategoryDeleteCommand { Id = id };
                await _mediator.SendCommandAsync(deleteCommand);

                TempData.Put(Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Category successfully deleted.",
                        Type = ResponseTypes.Success
                    });
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to delete Category.";

                _logger.LogError(ex, errorMessage);

                TempData.Put(Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = errorMessage,
                        Type = ResponseTypes.Danger
                    });
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
