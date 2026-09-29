using MaleFashion.Application.Features.ContactMessages.Query;
using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{
    public interface IContactUsRepository:IRepository<ContactUs,Guid>
    {
        Task<(IList<ContactUs>, int, int)> GetPagedContactUs(
   GetAllContactUsByPagingQuery query,
   CancellationToken cancellationToken);
    }
}
