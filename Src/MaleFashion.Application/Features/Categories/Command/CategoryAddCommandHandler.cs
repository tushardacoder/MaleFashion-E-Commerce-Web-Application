using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Categories.Command
{
    public class CategoryAddCommandHandler
     : ICommandHandler<CategoryAddCommand, Category>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryAddCommandHandler(
            IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Category> Handle(
            CategoryAddCommand command,
            CancellationToken cancellationToken)
        {
            var isDuplicate = await _unitOfWork
                .CategoryRepository
                .IsDuplicateCategoryName(
                    command.CategoryName,
                    null,
                    cancellationToken);

            if (isDuplicate)
            {
                return null;
            }

            var category = _mapper.Map<Category>(command);

            category.Id = IdentityGenerator.NewSequentialGuid();

            await _unitOfWork.CategoryRepository.AddAsync(
                category,
                cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return category;
        }
    }
}
