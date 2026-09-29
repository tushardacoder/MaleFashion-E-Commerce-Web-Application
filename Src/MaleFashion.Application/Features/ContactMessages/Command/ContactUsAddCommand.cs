using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Cortex.Mediator.Commands;

namespace MaleFashion.Application.Features.ContactMessages.Command
{
    public class ContactUsAddCommand : ICommand<ContactUs>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Message { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public string RecaptchaToken { get; set; } = default!;

    }

}
