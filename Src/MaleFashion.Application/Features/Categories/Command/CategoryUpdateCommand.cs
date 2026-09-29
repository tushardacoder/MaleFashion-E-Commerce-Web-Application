using Cortex.Mediator.Commands;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Categories.Command
{
    public class CategoryUpdateCommand : ICommand<Category>
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
