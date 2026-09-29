using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class GetShopProductDetailsQueryHandler
       : IQueryHandler<
           GetShopProductDetailsQuery,
           ShopProductDetailsViewModel>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetShopProductDetailsQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShopProductDetailsViewModel> Handle(
            GetShopProductDetailsQuery query,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // GET PRODUCT
            // ==========================================

            var product =
                await _unitOfWork
                    .ProductRepository
                    .GetShopProductDetailsAsync(
                        query.Id,
                        cancellationToken);

            // ==========================================
            // PRODUCT NOT FOUND
            // ==========================================
            if (product == null)
            {
                return new ShopProductDetailsViewModel();
            }

            var relatedProducts =
    await _unitOfWork
        .ProductRepository
        .GetRelatedProductsAsync(
            product.Id,
            product.CategoryId,
            cancellationToken);
            // ==========================================
            // ACTIVE VARIANTS
            // ==========================================

            var activeVariants =
                product.Variants
                    .Where(x => x.IsActive)
                    .ToList();

            // ==========================================
            // IN STOCK VARIANTS
            // ==========================================

            var inStockVariants =
                activeVariants
                    .Where(x =>
                        x.Inventory != null &&
                        x.Inventory.Quantity > 0)
                    .ToList();

            // ==========================================
            // PRODUCT IMAGES
            // ==========================================

            var allImages =
                activeVariants
                    .SelectMany(x => x.Images)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();

            // ==========================================
            // PRIMARY IMAGE
            // ==========================================

            var primaryImage =
                allImages
                    .FirstOrDefault(x => x.IsPrimary);

            if (primaryImage == null)
            {
                primaryImage =
                    allImages.FirstOrDefault();
            }


            // ==========================================
            // Related Product
            // ==========================================

            var relatedProductModels =
    relatedProducts
        .Select(x =>
        {
            var activeVariants =
                x.Variants
                    .Where(v => v.IsActive)
                    .ToList();

            var images =
                activeVariants
                    .SelectMany(v => v.Images)
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageName)
                    .Where(i =>
                        !string.IsNullOrWhiteSpace(i))
                    .Distinct()
                    .ToList();

            var primaryImage =
                activeVariants
                    .SelectMany(v => v.Images)
                    .Where(i => i.IsPrimary)
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageName)
                    .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(primaryImage))
            {
                primaryImage =
                    images.FirstOrDefault();
            }

            var isInStock =
                activeVariants.Any(v =>
                    v.Inventory != null &&
                    v.Inventory.Quantity > 0);

            return new ShopProductViewModel
            {
                Id = x.Id,

                ProductName =
                    x.ProductName,

                Branding =
                    x.Branding,

                Price =
                    x.ProductPrize,

                PrimaryImage =
                    primaryImage,

                Images =
                    images,

                Colors =
                    activeVariants
                        .Select(v => v.Color)
                        .Where(c =>
                            !string.IsNullOrWhiteSpace(c))
                        .Distinct()
                        .ToList(),

                Sizes =
                    activeVariants
                        .Select(v => v.Size)
                        .Where(s =>
                            !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList(),

                Tags =
                    x.Tags ?? new(),

                IsActive =
                    x.IsActive,

                IsInStock =
                    isInStock,

             

            };
        })
        .ToList();

         

            // ==========================================
            // CREATE VIEW MODEL
            // ==========================================

            var model =
                new ShopProductDetailsViewModel
                {
                    Id = product.Id,

                    ProductName =
                        product.ProductName,

                    Branding =
                        product.Branding,

                    Price =
                        product.ProductPrize,

                    Description =
                        product.Description,

                    CustomerPreview = product.CustomerPreview,

                    PrimaryImage =
                        primaryImage?.ImageName,
                    
                    Images =
                        allImages
                            .Select(x => x.ImageName)
                            .Where(x =>
                                !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .ToList(),

                    Colors =
                        activeVariants
                            .Select(x => x.Color)
                            .Where(x =>
                                !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .ToList(),

                    Sizes =
                        activeVariants
                            .Select(x => x.Size)
                            .Where(x =>
                                !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .ToList(),

                    Tags =
                        product.Tags ?? new(),

                    IsActive =
                        product.IsActive,

                    IsInStock =
                        inStockVariants.Any(),

                           // ==========================================
                           // RELATED PRODUCTS
                           // ==========================================

                     RelatedProducts =relatedProductModels,



                    // ==========================================
                    // Added for wishlis
                    // ==========================================
                  
                   Variants =
                         product.Variants
                        .Select(v =>
                      new ShopProductVariantViewModel
            {
                      Id = v.Id,

                      Color = v.Color,

                     Size = v.Size,

                     IsInStock =
                       v.Inventory != null &&
                       v.Inventory.Quantity > 0,

                          InventoryQuantity = v.Inventory?.Quantity ?? 0}).ToList()


                };

                return model;
        }
    }
}
