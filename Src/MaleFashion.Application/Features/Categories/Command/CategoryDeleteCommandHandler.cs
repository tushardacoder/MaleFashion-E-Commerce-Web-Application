using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Categories.Command
{
    public class CategoryDeleteCommandHandler : ICommandHandler<CategoryDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public CategoryDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CategoryDeleteCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.CategoryRepository.RemoveAsync(command.Id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

    }
}
