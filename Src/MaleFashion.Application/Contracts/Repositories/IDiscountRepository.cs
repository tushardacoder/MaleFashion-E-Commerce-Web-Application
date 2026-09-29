using MaleFashion.Application.Features.Discounts.Query;
using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{
    public interface IDiscountRepository
       : IRepository<Discount, Guid>
    {
        Task<(IList<Discount>, int, int)> GetPagedDiscounts(
            GetAllDiscountsByPagingQuery query,
            CancellationToken cancellationToken);


        Task<bool> IsDuplicateDiscountCode(
            string code,
            Guid? id,
            CancellationToken cancellationToken);


        Task<Discount?> GetActiveDiscountAsync(
            DateTime currentTime,
            CancellationToken cancellationToken);
    }
}
