using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.ContactMessages.Query
{
    public class GetAllContactUsByPagingQueryHandler
     : IQueryHandler<GetAllContactUsByPagingQuery, (IList<ContactUs>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAllContactUsByPagingQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<ContactUs>, int, int)> Handle(
            GetAllContactUsByPagingQuery query,
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ContactUsRepository
                .GetPagedContactUs(query, cancellationToken);
        }
    }
}
