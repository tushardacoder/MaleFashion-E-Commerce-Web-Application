using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Command
{
    public class ProductAddCommandHandler
       : ICommandHandler<ProductAddCommand, Product>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public ProductAddCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(
            ProductAddCommand command,
            CancellationToken cancellationToken)
        {
            //var duplicate =
            //    await _unitOfWork
            //        .ProductRepository
            //        .IsDuplicateProductName(
            //            command.ProductName,
            //            null,
            //            cancellationToken);

            //if (duplicate)
            //    return null!;

            var product = new Product
            {
                Id = IdentityGenerator.NewSequentialGuid(),

                ProductName =
                    command.ProductName.Trim(),

                Branding =
                    command.Branding.Trim(),

                ProductPrize =
                    command.ProductPrize,

                Tags =
                    command.Tags
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList(),

                Description =
                    command.Description,

                CustomerPreview =
                    command.CustomerPreview,

                AdditionalInfo =
                    command.AdditionalInfo,

                IsActive =
                    command.IsActive,

                CategoryId =
                    command.CategoryId
            };

            foreach (var variantCommand
                     in command.Variants)
            {
                var variant = new ProductVariant
                {
                    Id =
                        IdentityGenerator.NewSequentialGuid(),

                    ProductId =
                        product.Id,

                    Sku =
                        variantCommand.Sku.Trim(),

                    Size =
                        variantCommand.Size.Trim(),

                    Color =
                        variantCommand.Color.Trim(),

                    IsActive =
                        variantCommand.IsActive
                };

                foreach (var imageCommand
                         in variantCommand.Images)
                {
                    var image = new ProductImage
                    {
                        Id =
                            IdentityGenerator.NewSequentialGuid(),

                        ProductVariantId =
                            variant.Id,

                        ImageName =
                            imageCommand.ImageName,

                        DisplayOrder =
                            imageCommand.DisplayOrder,

                        IsPrimary =
                            imageCommand.IsPrimary
                    };

                    variant.Images.Add(image);
                }

                product.Variants.Add(variant);
            }

            await _unitOfWork.ProductRepository
                .AddAsync(
                    product,
                    cancellationToken);

            await _unitOfWork.SaveAsync(
                cancellationToken);

            return product;
        }
    }
}
