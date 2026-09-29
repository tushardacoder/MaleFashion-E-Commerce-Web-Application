using Cortex.Mediator.Commands;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Command
{
    public class AddOrderCommand : ICommand<Guid>
    {
        public Guid? UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? TownCity { get; set; }

        public string? CountryState { get; set; }

        public string? PostcodeZip { get; set; }

        public string? OrderNotes { get; set; }

        public string? CouponCode { get; set; }

        public PaymentType PaymentType { get; set; }

        public string? MobileNumber { get; set; }

        public string? TransactionId { get; set; }
    }
}
