using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
   

    public class OrderHistoryViewModel
    {
        public List<OrderHistoryItemViewModel> Orders { get; set; }
            = new();
    }
}
