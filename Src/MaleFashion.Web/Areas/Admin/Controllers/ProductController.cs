
using Cortex.Mediator;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Exceptions;
using MaleFashion.Application.Features.Products.Command;
using MaleFashion.Application.Features.Products.Query;
using MaleFashion.Application.Services;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
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
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public ProductController(
            ILogger<ProductController> logger,
            IMediator mediator,
            IMapper mapper,
            IApplicationUnitOfWork unitOfWork,
            IFileStorageService fileStorageService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
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
        // CREATE GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken)
        {
            var model = new ProductModel
            {
                IsActive = true
            };

            await LoadCategories(
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
            ProductModel model,
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
                        Message =
                            "Please provide all required information.",
                        Type =
                            ResponseTypes.Danger
                    });

                await LoadCategories(
                    model,
                    cancellationToken);

                return View(model);
            }


            var savedFiles = new List<string>();


            try
            {
                // ==========================================
                // MAP MODEL → COMMAND
                // ==========================================

                var command =
                    _mapper.Map<ProductAddCommand>(
                        model);


                // ==========================================
                // TAGS
                // ==========================================

                command.Tags =
                    ParseTags(model.TagsText);


                // ==========================================
                // VARIANTS
                // ==========================================

                command.Variants = new();


                foreach (var variantModel in model.Variants)
                {
                    var variantCommand =
                        new ProductVariantCommand
                        {
                            Id = Guid.NewGuid(),

                            Sku =
                                variantModel.Sku,

                            Size =
                                variantModel.Size,

                            Color =
                                variantModel.Color,

                            IsActive =
                                variantModel.IsActive,

                            Images = new()
                        };


                    // ==========================================
                    // VARIANT IMAGES
                    // ==========================================

                    foreach (
                        var imageModel
                        in variantModel.Images)
                    {
                        if (imageModel.Image == null)
                        {
                            continue;
                        }


                        await using var stream =
                            imageModel.Image.OpenReadStream();


                        var imageName =
                            await _fileStorageService.SaveImageAsync(
                                stream,
                                imageModel.Image.FileName,
                                "products",
                                cancellationToken);


                        savedFiles.Add(imageName);


                        variantCommand.Images.Add(
                            new ProductImageCommand
                            {
                                Id = Guid.NewGuid(),

                                ImageName =
                                    imageName,

                                DisplayOrder =
                                    imageModel.DisplayOrder,

                                IsPrimary =
                                    imageModel.IsPrimary
                            });
                    }


                    command.Variants.Add(
                        variantCommand);
                }


                // ==========================================
                // SEND COMMAND
                // ==========================================

                var result =
                    await _mediator.SendCommandAsync(
                        command,
                        cancellationToken);


                // ==========================================
                // DUPLICATE PRODUCT
                // ==========================================

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Product already exists.",
                            Type =
                                ResponseTypes.Danger
                        });


                    await DeleteSavedFiles(
                        savedFiles,
                        cancellationToken);


                    await LoadCategories(
                        model,
                        cancellationToken);

                    return View(model);
                }


                // ==========================================
                // SUCCESS
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Product successfully created.",
                        Type =
                            ResponseTypes.Success
                    });


                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create product.");


                // ==========================================
                // CLEANUP UPLOADED FILES
                // ==========================================

                await DeleteSavedFiles(
                    savedFiles,
                    cancellationToken);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Failed to create product.",
                        Type =
                            ResponseTypes.Danger
                    });


                await LoadCategories(
                    model,
                    cancellationToken);

                return View(model);
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
                // ==========================================
                // GET PRODUCT
                // ==========================================

                var query =
                    new GetProductByIdQuery
                    {
                        Id = id
                    };


                var product =
                    await _mediator.SendQueryAsync(
                        query,
                        cancellationToken);


                // ==========================================
                // CHECK PRODUCT
                // ==========================================

                if (product is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Product doesn't exist.",
                            Type =
                                ResponseTypes.Danger
                        });


                    return RedirectToAction(
                        nameof(Index));
                }


                return View(product);
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to load product.";

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


                return RedirectToAction(
                    nameof(Index));
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
            try
            {
                // ==========================================
                // GET PRODUCT
                // ==========================================

                var query =
                    new GetProductByIdQuery
                    {
                        Id = id
                    };


                var product =
                    await _mediator.SendQueryAsync(
                        query,
                        cancellationToken);


                // ==========================================
                // CHECK PRODUCT
                // ==========================================

                if (product is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Product doesn't exist.",
                            Type =
                                ResponseTypes.Danger
                        });


                    return RedirectToAction(
                        nameof(Index));
                }


                // ==========================================
                // MAP PRODUCT → MODEL
                // ==========================================

                var model =
                    new ProductModel
                    {
                        Id =
                            product.Id,

                        ProductName =
                            product.ProductName,

                        Branding =
                            product.Branding,

                        ProductPrize =
                            product.ProductPrize,

                        TagsText =
                            string.Join(
                                ", ",
                                product.Tags),

                        Description =
                            product.Description,

                        CustomerPreview =
                            product.CustomerPreview,

                        AdditionalInfo =
                            product.AdditionalInfo,

                        IsActive =
                            product.IsActive,

                        CategoryId =
                            product.CategoryId,

                        Variants =
                            product.Variants
                                .Select(v =>
                                    new ProductVariantModel
                                    {
                                        Id =
                                            v.Id,

                                        Sku =
                                            v.Sku,

                                        Size =
                                            v.Size,

                                        Color =
                                            v.Color,

                                        IsActive =
                                            v.IsActive,

                                        Images =
                                            v.Images
                                                .OrderBy(
                                                    i =>
                                                        i.DisplayOrder)
                                                .Select(i =>
                                                    new ProductImageModel
                                                    {
                                                        Id =
                                                            i.Id,

                                                        ImageName =
                                                            i.ImageName,

                                                        DisplayOrder =
                                                            i.DisplayOrder,

                                                        IsPrimary =
                                                            i.IsPrimary
                                                    })
                                                .ToList()
                                    })
                                .ToList()
                    };


                // ==========================================
                // LOAD CATEGORIES
                // ==========================================

                await LoadCategories(
                    model,
                    cancellationToken);


                return View(model);
            }
            catch (Exception ex)
            {
                const string errorMessage =
                    "Failed to load product.";

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


                return RedirectToAction(
                    nameof(Index));
            }
        }


        // ==========================================
        // UPDATE POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            ProductModel model,
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
                        Message =
                            "Please provide all required information.",
                        Type =
                            ResponseTypes.Danger
                    });


                await LoadCategories(
                    model,
                    cancellationToken);

                return View(model);
            }


            var newFiles =
                new List<string>();


            try
            {
                // ==========================================
                // GET EXISTING PRODUCT
                // ==========================================

                var existing =
                    await _mediator.SendQueryAsync(
                        new GetProductByIdQuery
                        {
                            Id = model.Id
                        },
                        cancellationToken);


                if (existing is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message =
                                "Product doesn't exist.",
                            Type =
                                ResponseTypes.Danger
                        });


                    return RedirectToAction(
                        nameof(Index));
                }


                // ==========================================
                // GET OLD IMAGE NAMES
                // ==========================================

                var oldImageNames =
                    existing.Variants
                        .SelectMany(
                            x => x.Images)
                        .Select(
                            x => x.ImageName)
                        .Where(
                            x =>
                                !string.IsNullOrWhiteSpace(x))
                        .ToHashSet();


                // ==========================================
                // MAP MODEL → COMMAND
                // ==========================================

                var command =
                    _mapper.Map<ProductUpdateCommand>(
                        model);


                command.Tags =
                    ParseTags(model.TagsText);


                command.Variants = new();


                var finalImageNames =
                    new HashSet<string>();


                // ==========================================
                // VARIANTS
                // ==========================================

                foreach (
                    var variantModel
                    in model.Variants)
                {
                    var variantCommand =
                        new ProductVariantCommand
                        {
                            Id = variantModel.Id ?? Guid.NewGuid(),

                            Sku =
                                variantModel.Sku,

                            Size =
                                variantModel.Size,

                            Color =
                                variantModel.Color,

                            IsActive =
                                variantModel.IsActive,

                            Images = new()
                        };


                    // ==========================================
                    // IMAGES
                    // ==========================================

                    foreach (
                        var imageModel
                        in variantModel.Images)
                    {
                        var imageName =
                            imageModel.ImageName;


                        // ==========================================
                        // NEW IMAGE
                        // ==========================================

                        if (imageModel.Image != null)
                        {
                            await using var stream =
                                imageModel.Image.OpenReadStream();


                            imageName =
                                await _fileStorageService
                                    .SaveImageAsync(
                                        stream,
                                        imageModel.Image.FileName,
                                        "products",
                                        cancellationToken);


                            newFiles.Add(
                                imageName);
                        }


                        // ==========================================
                        // NO IMAGE
                        // ==========================================

                        if (string.IsNullOrWhiteSpace(
                            imageName))
                        {
                            continue;
                        }


                        finalImageNames.Add(
                            imageName);


                        variantCommand.Images.Add(
                            new ProductImageCommand
                            {
                                Id =
                                    imageModel.Id
                                    ?? Guid.NewGuid(),

                                ImageName =
                                    imageName,

                                DisplayOrder =
                                    imageModel.DisplayOrder,

                                IsPrimary =
                                    imageModel.IsPrimary
                            });
                    }


                    command.Variants.Add(
                        variantCommand);
                }


                // ==========================================
                // UPDATE PRODUCT
                // ==========================================

                await _mediator.SendCommandAsync(
                    command,
                    cancellationToken);


                // ==========================================
                // DELETE OLD FILES
                // ==========================================

                var filesToDelete =
                    oldImageNames
                        .Except(finalImageNames)
                        .ToList();


                foreach (
                    var oldImage
                    in filesToDelete)
                {
                    try
                    {
                        await _fileStorageService
                            .DeleteImageAsync(
                                oldImage,
                                "products",
                                cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(
                            ex,
                            "Could not delete old product image {ImageName}",
                            oldImage);
                    }
                }


                // ==========================================
                // SUCCESS
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Product successfully updated.",
                        Type =
                            ResponseTypes.Success
                    });


                return RedirectToAction(
                    nameof(Index));
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


                await LoadCategories(
                    model,
                    cancellationToken);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update product.");


                // ==========================================
                // DELETE NEW FILES
                // ==========================================

                await DeleteSavedFiles(
                    newFiles,
                    cancellationToken);


                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message =
                            "Failed to update product.",
                        Type =
                            ResponseTypes.Danger
                    });


                await LoadCategories(
                    model,
                    cancellationToken);

                return View(model);
            }
        }

   
          // ==========================================
         // DELETE
          // ==========================================

             [HttpPost]
             [ValidateAntiForgeryToken]
         public async Task<IActionResult> Delete(
                    Guid id,
                    CancellationToken cancellationToken)
        {
            try
            {
                // ==========================================
                // GET PRODUCT
                // ==========================================

                var product =
                    await _mediator.SendQueryAsync(
                        new GetProductByIdQuery
                        {
                            Id = id
                        },
                        cancellationToken);

                if (product is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Type = ResponseTypes.Warning,
                            Message = "Product doesn't exist."
                        });

                    return RedirectToAction(nameof(Index));
                }


                // ==========================================
                // GET IMAGE NAMES
                // ==========================================

                var imageNames =
                    product.Variants
                        .SelectMany(x => x.Images)
                        .Select(x => x.ImageName)
                        .Where(
                            x =>
                                !string.IsNullOrWhiteSpace(x))
                        .ToList();


                // ==========================================
                // DELETE DATABASE RECORD
                // ==========================================

                await _mediator.SendCommandAsync(
                    new ProductDeleteCommand
                    {
                        Id = id
                    },
                    cancellationToken);


                // ==========================================
                // DELETE PHYSICAL FILES
                // ==========================================

                foreach (var imageName in imageNames)
                {
                    try
                    {
                        await _fileStorageService
                            .DeleteImageAsync(
                                imageName,
                                "products",
                                cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(
                            ex,
                            "Could not delete product image {ImageName}",
                            imageName);
                    }
                }


                // ==========================================
                // SUCCESS MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Success,
                        Message = "Product successfully deleted."
                    });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to delete product {ProductId}",
                    id);


                // ==========================================
                // ERROR MESSAGE
                // ==========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Warning,
                        Message = "Failed to delete product."
                    });

                return RedirectToAction(nameof(Index));
            }
        }




        // ==========================================
        // DATATABLES
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> GetPagedProducts(
            [FromBody] ProductListModel model,
            CancellationToken cancellationToken)
        {
            try
            {
                // ==========================================
                // MAP DATATABLE REQUEST
                // ==========================================

                var query =
                    _mapper.Map<
                        GetAllProductsByPagingQuery>(
                            model);


                // ==========================================
                // SEARCH
                // ==========================================

                query.SearchText =
                    model.Search.Value
                    ?? string.Empty;


                _logger.LogInformation(
                    "Product Search = {Search}",
                    query.SearchText);


                // ==========================================
                // SORTING
                // ==========================================

                query.SortText =
                    model.FormatSortExpression(
                        "ProductName",
                        "Branding",
                        "ProductPrize",
                        "IsActive");


                // ==========================================
                // SEND QUERY
                // ==========================================

                var (items, total, totalDisplay) =
                    await _mediator.SendQueryAsync<
                        GetAllProductsByPagingQuery,
                        (IList<Product>, int, int)>(
                            query);


                // ==========================================
                // DATATABLE RESPONSE
                // ==========================================

                var products =
                    new
                    {
                        

                        recordsTotal =
                            total,

                        recordsFiltered =
                            totalDisplay,

                        data =
                            items.Select(item => new
                            {
                                id =
                                    item.Id,

                                productName =
                                    HttpUtility.HtmlEncode(
                                        item.ProductName),

                                branding =
                                    HttpUtility.HtmlEncode(
                                        item.Branding),
                                

                                productPrize =
                                    item.ProductPrize,

                                isActive =
                                    item.IsActive
                            }).ToArray()
                    };


                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get product list");


                return Json(
                    DataTables.EmptyResult);
            }
        }


        // ==========================================
        // LOAD CATEGORIES
        // ==========================================

        private async Task LoadCategories(
            ProductModel model,
            CancellationToken cancellationToken)
        {
            var categories =
                await _unitOfWork
                    .CategoryRepository
                    .GetAllAsync(
                        cancellationToken);


            model.Categories =
                categories
                    .Where(x => x.IsActive)
                    .Select(x =>
                        new SelectListItem
                        {
                            Value =
                                x.Id.ToString(),

                            Text =
                                x.CategoryName
                        })
                    .ToList();
        }


        // ==========================================
        // DELETE SAVED FILES
        // ==========================================

        private async Task DeleteSavedFiles(
            IEnumerable<string> files,
            CancellationToken cancellationToken)
        {
            foreach (var file in files)
            {
                try
                {
                    await _fileStorageService
                        .DeleteImageAsync(
                            file,
                            "products",
                            cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Could not delete uploaded file {FileName}",
                        file);
                }
            }
        }


        // ==========================================
        // TAG PARSER
        // ==========================================

        private static List<string> ParseTags(
            string? tagsText)
        {
            if (string.IsNullOrWhiteSpace(tagsText))
            {
                return new List<string>();
            }


            return tagsText
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(
                    x => x.Trim())
                .Where(
                    x =>
                        !string.IsNullOrWhiteSpace(x))
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}

