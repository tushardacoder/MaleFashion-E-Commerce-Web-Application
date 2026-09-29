using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class GetShopProductsQueryHandler
        : IQueryHandler<GetShopProductsQuery, ShopViewModel>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetShopProductsQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShopViewModel> Handle(
            GetShopProductsQuery query,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // LOAD PRODUCTS
            // ==========================================

            var products =
                await _unitOfWork.ProductRepository.GetShopProductsAsync(
                    cancellationToken);


            // ==========================================
            // FILTER
            // ==========================================

            var filteredProducts = products
                .AsEnumerable();


            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search =
                    query.Search.Trim();

                filteredProducts = filteredProducts.Where(x =>
                    x.ProductName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Branding.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Description.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));
            }


            // ==========================================
            // CATEGORY
            // ==========================================

            if (query.CategoryId.HasValue)
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.CategoryId ==
                        query.CategoryId.Value);
            }


            // ==========================================
            // BRAND
            // ==========================================

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.Branding.Equals(
                            query.Brand,
                            StringComparison.OrdinalIgnoreCase));
            }


            // ==========================================
            // PRICE FROM
            // ==========================================

            if (query.MinPrice.HasValue)
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.ProductPrize >=
                        query.MinPrice.Value);
            }


            // ==========================================
            // PRICE TO
            // ==========================================

            if (query.MaxPrice.HasValue)
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.ProductPrize <=
                        query.MaxPrice.Value);
            }


            // ==========================================
            // SIZE
            // ==========================================

            if (!string.IsNullOrWhiteSpace(query.Size))
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.Variants.Any(v =>
                            v.IsActive &&
                            v.Size == query.Size));
            }


            // ==========================================
            // COLOR
            // ==========================================

            if (!string.IsNullOrWhiteSpace(query.Color))
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.Variants.Any(v =>
                            v.IsActive &&
                            v.Color == query.Color));
            }


            // ==========================================
            // TAG
            // ==========================================

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                filteredProducts =
                    filteredProducts.Where(x =>
                        x.Tags.Any(t =>
                            t.Equals(
                                query.Tag,
                                StringComparison.OrdinalIgnoreCase)));
            }


            // ==========================================
            // TOTAL
            // ==========================================

            var productList =
                filteredProducts.ToList();

            var totalProducts =
                productList.Count;


            // ==========================================
            // SORT
            // ==========================================

            productList = query.Sort switch
            {
                "price-low" =>
                    productList
                        .OrderBy(x => x.ProductPrize)
                        .ToList(),

                "price-high" =>
                    productList
                        .OrderByDescending(x => x.ProductPrize)
                        .ToList(),

                "name" =>
                    productList
                        .OrderBy(x => x.ProductName)
                        .ToList(),

                "name-desc" =>
                    productList
                        .OrderByDescending(x => x.ProductName)
                        .ToList(),

                _ =>
                    productList
                        .OrderBy(x => x.ProductName)
                        .ToList()
            };


            // ==========================================
            // PAGINATION
            // ==========================================

            var page =
                query.Page < 1
                    ? 1
                    : query.Page;

            var pageSize =
                query.PageSize <= 0
                    ? 12
                    : query.PageSize;


            var pagedProducts =
                productList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


            // ==========================================
            // CREATE VIEW MODEL
            // ==========================================

            var model = new ShopViewModel
            {
                Search = query.Search,

                CategoryId = query.CategoryId,

                Brand = query.Brand,

                Size = query.Size,

                Color = query.Color,

                Tag = query.Tag,

                MinPrice = query.MinPrice,

                MaxPrice = query.MaxPrice,

                Sort = query.Sort,

                CurrentPage = page,

                PageSize = pageSize,

                TotalProducts = totalProducts
            };


            // ==========================================
            // MAP PRODUCTS
            // ==========================================

            foreach (var product in pagedProducts)
            {
                var activeVariants =
                    product.Variants
                        .Where(x =>
                            x.IsActive &&
                            x.Inventory != null &&
                            x.Inventory.Quantity > 0)
                        .ToList();


                // ======================================
                // PRIMARY IMAGE
                // ======================================

                var primaryImage =
                    activeVariants
                        .SelectMany(x => x.Images)
                        .Where(x => x.IsPrimary)
                        .OrderBy(x => x.DisplayOrder)
                        .FirstOrDefault();


                // ======================================
                // FALLBACK IMAGE
                // ======================================

                if (primaryImage == null)
                {
                    primaryImage =
                        activeVariants
                            .SelectMany(x => x.Images)
                            .OrderBy(x => x.DisplayOrder)
                            .FirstOrDefault();
                }


                // ======================================
                // MAP PRODUCT
                // ======================================

                model.Products.Add(
                    new ShopProductViewModel
                    {
                        Id = product.Id,

                        ProductName =
                            product.ProductName,

                        Branding =
                            product.Branding,

                        Price =
                            product.ProductPrize,

                        PrimaryImage =
                            primaryImage?.ImageName,

                        Images =
            activeVariants
                .SelectMany(x => x.Images)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => x.ImageName)
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
                            activeVariants.Any()
                    });
            }


            // ==========================================
            // CATEGORIES
            // ==========================================

            model.Categories =
                products
                    .Where(x => x.Category != null)
                    .GroupBy(x => new
                    {
                        x.CategoryId,
                        Name = x.Category.CategoryName
                    })
                    .Select(x =>
                        new ShopCategoryViewModel
                        {
                            Id =
                                x.Key.CategoryId,

                            Name =
                                x.Key.Name,

                            ProductCount =
                                x.Count()
                        })
                    .OrderBy(x => x.Name)
                    .ToList();


            // ==========================================
            // BRANDS
            // ==========================================

            model.Brands =
                products
                    .Select(x => x.Branding)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();


            // ==========================================
            // SIZES
            // ==========================================

            model.Sizes =
                products
                    .SelectMany(x => x.Variants)
                    .Where(x => x.IsActive)
                    .Select(x => x.Size)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();


            // ==========================================
            // COLORS
            // ==========================================

            model.Colors =
                products
                    .SelectMany(x => x.Variants)
                    .Where(x => x.IsActive)
                    .Select(x => x.Color)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();


            // ==========================================
            // TAGS
            // ==========================================

            model.Tags =
                products
                    .SelectMany(x =>
                        x.Tags ?? new List<string>())
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();


            return model;
        }
    }
}
