using System;
using System.Collections.Generic;
using System.Text;
using Cortex.Mediator.Commands;


namespace MaleFashion.Application.Features.Categories.Command
{
    public class CategoryDeleteCommand : ICommand
    {

        public Guid Id { get; set; }
    }
}
