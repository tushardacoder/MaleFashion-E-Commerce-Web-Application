using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{


    public interface IInventoryRepository
        : IRepository<Inventory, Guid>
    {
        Task<(IList<Inventory> Data, int Total, int TotalDisplay)>
            GetPagedInventories(
                int pageIndex,
                int pageSize,
                string orderBy,
                string? searchText,
                CancellationToken cancellationToken);

        Task<IList<ProductVariant>>
            GetAvailableProductVariants(
                CancellationToken cancellationToken);

        Task<Inventory?> GetInventoryByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

       
        Task<IList<ProductVariant>>
        GetProductVariantsByIds(
        IList<Guid> ids,
        CancellationToken cancellationToken);

        Task<bool> DecreaseStockAsync(
      Guid productVariantId,
      int quantity,
      CancellationToken cancellationToken);
    }
}
