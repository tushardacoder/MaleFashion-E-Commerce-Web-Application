using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace MaleFashion.Application.Features.ContactMessages.Query
{
    public class GetAllContactUsByPagingQuery : IQuery<(IList<ContactUs>, int, int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public string? SortText { get; set; }

    }
}
