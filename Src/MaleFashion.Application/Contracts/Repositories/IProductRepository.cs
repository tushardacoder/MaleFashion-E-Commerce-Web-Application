using MaleFashion.Application.Features.Products.Query;
using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace MaleFashion.Application.Contracts.Repositories
{
    public interface IProductRepository
       : IRepository<Product, Guid>
    {
        Task<(IList<Product>, int, int)> GetPagedProducts(
            GetAllProductsByPagingQuery query,
            CancellationToken cancellationToken);

        Task<Product?> GetProductDetailsAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<bool> IsDuplicateProductName(
            string productName,
            Guid? id,
            CancellationToken cancellationToken);

        Task<IList<Product>> GetShopProductsAsync(
    CancellationToken cancellationToken);

        Task<Product?> GetShopProductDetailsAsync(
    Guid id,
    CancellationToken cancellationToken);


        Task<IList<Product>> GetRelatedProductsAsync(
    Guid productId,
    Guid? categoryId,
    CancellationToken cancellationToken);

        Task<Product?> GetByIdWithVariantsAsync(
      Guid productId,
      CancellationToken cancellationToken);

        Task<ProductVariant?> GetVariantByIdAsync(
            Guid variantId,
            CancellationToken cancellationToken);


        Task<ProductVariant?> GetVariantInventoryByIdAsync(
               Guid id,
               CancellationToken cancellationToken);

    }
}





