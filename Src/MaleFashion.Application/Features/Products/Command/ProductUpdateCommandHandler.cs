using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Exceptions;
using MaleFashion.Application.Features.Products.Command;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;

public class ProductUpdateCommandHandler
       : ICommandHandler<ProductUpdateCommand, Product>
{
    private readonly IApplicationUnitOfWork _unitOfWork;

    public ProductUpdateCommandHandler(
        IApplicationUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> Handle(
        ProductUpdateCommand command,
        CancellationToken cancellationToken)
    {
        // ==========================================
        // CHECK DUPLICATE PRODUCT NAME
        // ==========================================

        //var duplicate =
        //    await _unitOfWork
        //        .ProductRepository
        //        .IsDuplicateProductName(
        //            command.ProductName,
        //            command.Id,
        //            cancellationToken);

        //if (duplicate)
        //{
        //    throw new DuplicateDataException(
        //        "Product name already exists.");
        //}

        // ==========================================
        // GET EXISTING PRODUCT
        // ==========================================

        var product =
            await _unitOfWork
                .ProductRepository
                .GetProductDetailsAsync(
                    command.Id,
                    cancellationToken);

        if (product == null)
        {
            throw new Exception(
                "Product doesn't exist.");
        }

        // ==========================================
        // UPDATE PRODUCT
        // ==========================================

        product.ProductName =
            command.ProductName.Trim();

        product.Branding =
            command.Branding.Trim();

        product.ProductPrize =
            command.ProductPrize;

        product.Tags =
            command.Tags
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .ToList();

        product.Description =
            command.Description;

        product.CustomerPreview =
            command.CustomerPreview;

        product.AdditionalInfo =
            command.AdditionalInfo;

        product.IsActive =
            command.IsActive;

        product.CategoryId =
            command.CategoryId;

        // ==========================================
        // DELETE REMOVED VARIANTS
        // ==========================================

        var submittedVariantIds =
            command.Variants
                .Where(x =>
                    x.Id != Guid.Empty)
                .Select(x =>
                    x.Id)
                .ToHashSet();

        var removedVariants =
            product.Variants
                .Where(x =>
                    !submittedVariantIds
                        .Contains(x.Id))
                .ToList();

        foreach (var variant in removedVariants)
        {
            product.Variants.Remove(
                variant);
        }

        // ==========================================
        // ADD / UPDATE VARIANTS
        // ==========================================

        foreach (var variantCommand
                 in command.Variants)
        {
            ProductVariant? variant = null;

            // ======================================
            // FIND EXISTING VARIANT
            // ======================================

            if (variantCommand.Id != Guid.Empty)
            {
                variant =
                    product.Variants
                        .FirstOrDefault(
                            x =>
                                x.Id ==
                                variantCommand.Id);
            }

            // ======================================
            // NEW VARIANT
            // ======================================

            if (variant == null)
            {
                variant = new ProductVariant
                {
                    Id =
                        IdentityGenerator
                            .NewSequentialGuid(),

                    ProductId =
                        product.Id
                };

                product.Variants.Add(
                    variant);
            }

            // ======================================
            // UPDATE VARIANT
            // ======================================

            variant.Sku =
                variantCommand.Sku.Trim();

            variant.Size =
                variantCommand.Size.Trim();

            variant.Color =
                variantCommand.Color.Trim();

            variant.IsActive =
                variantCommand.IsActive;

            // ======================================
            // DELETE REMOVED IMAGES
            // ======================================

            var submittedImageIds =
                variantCommand.Images
                    .Where(x =>
                        x.Id != Guid.Empty)
                    .Select(x =>
                        x.Id)
                    .ToHashSet();

            var removedImages =
                variant.Images
                    .Where(x =>
                        !submittedImageIds
                            .Contains(x.Id))
                    .ToList();

            foreach (var image
                     in removedImages)
            {
                variant.Images.Remove(
                    image);
            }

            // ======================================
            // ADD / UPDATE IMAGES
            // ======================================

            foreach (var imageCommand
                     in variantCommand.Images)
            {
                ProductImage? image = null;

                // ==================================
                // FIND EXISTING IMAGE
                // ==================================

                if (imageCommand.Id != Guid.Empty)
                {
                    image =
                        variant.Images
                            .FirstOrDefault(
                                x =>
                                    x.Id ==
                                    imageCommand.Id);
                }

                // ==================================
                // NEW IMAGE
                // ==================================

                if (image == null)
                {
                    image = new ProductImage
                    {
                        Id =
                            IdentityGenerator
                                .NewSequentialGuid(),

                        ProductVariantId =
                            variant.Id
                    };

                    variant.Images.Add(
                        image);
                }

                // ==================================
                // UPDATE IMAGE
                // ==================================

                image.ImageName =
                    imageCommand.ImageName;

                image.DisplayOrder =
                    imageCommand.DisplayOrder;

                image.IsPrimary =
                    imageCommand.IsPrimary;
            }
        }

        // ==========================================
        // SAVE CHANGES
        // ==========================================

        await _unitOfWork.SaveAsync(
            cancellationToken);

        return product;
    }


    //public async Task<Product> Handle(
    // ProductUpdateCommand command,
    // CancellationToken cancellationToken)
    //{
    //    // ==========================================
    //    // CHECK DUPLICATE PRODUCT NAME
    //    // ==========================================

    //    var duplicate =
    //        await _unitOfWork
    //            .ProductRepository
    //            .IsDuplicateProductName(
    //                command.ProductName,
    //                command.Id,
    //                cancellationToken);

    //    if (duplicate)
    //    {
    //        throw new DuplicateDataException(
    //            "Product name already exists.");
    //    }

    //    // ==========================================
    //    // GET EXISTING PRODUCT
    //    // ==========================================

    //    var product =
    //        await _unitOfWork
    //            .ProductRepository
    //            .GetProductDetailsAsync(
    //                command.Id,
    //                cancellationToken);

    //    if (product == null)
    //    {
    //        throw new Exception(
    //            "Product doesn't exist.");
    //    }

    //    // ==========================================
    //    // UPDATE PRODUCT
    //    // ==========================================

    //    product.ProductName =
    //        command.ProductName.Trim();

    //    product.Branding =
    //        command.Branding.Trim();

    //    product.ProductPrize =
    //        command.ProductPrize;

    //    product.Tags =
    //        command.Tags
    //            .Where(x => !string.IsNullOrWhiteSpace(x))
    //            .Select(x => x.Trim())
    //            .Distinct(StringComparer.OrdinalIgnoreCase)
    //            .ToList();

    //    product.Description =
    //        command.Description;

    //    product.CustomerPreview =
    //        command.CustomerPreview;

    //    product.AdditionalInfo =
    //        command.AdditionalInfo;

    //    product.IsActive =
    //        command.IsActive;

    //    product.CategoryId =
    //        command.CategoryId;

    //    // ==========================================
    //    // SUBMITTED VARIANT IDS
    //    // ==========================================

    //    var submittedVariantIds =
    //        command.Variants
    //            .Where(x => x.Id != Guid.Empty)
    //            .Select(x => x.Id)
    //            .ToHashSet();

    //    // ==========================================
    //    // DELETE REMOVED VARIANTS
    //    // ==========================================

    //    var removedVariants =
    //        product.Variants
    //            .Where(x =>
    //                !submittedVariantIds.Contains(x.Id))
    //            .ToList();

    //    foreach (var variant in removedVariants)
    //    {
    //        product.Variants.Remove(variant);
    //    }

    //    // ==========================================
    //    // ADD / UPDATE VARIANTS
    //    // ==========================================

    //    foreach (var variantCommand in command.Variants)
    //    {
    //        ProductVariant variant;

    //        // ======================================
    //        // EXISTING VARIANT
    //        // ======================================

    //        if (variantCommand.Id != Guid.Empty)
    //        {
    //            variant =
    //                product.Variants
    //                    .FirstOrDefault(
    //                        x => x.Id == variantCommand.Id);

    //            // ==================================
    //            // INVALID VARIANT ID
    //            // ==================================

    //            if (variant == null)
    //            {
    //                throw new Exception(
    //                    $"Product variant '{variantCommand.Id}' " +
    //                    $"does not belong to this product.");
    //            }
    //        }
    //        else
    //        {
    //            // ==================================
    //            // NEW VARIANT
    //            // ==================================

    //            variant = new ProductVariant
    //            {
    //                Id =
    //                    IdentityGenerator
    //                        .NewSequentialGuid(),

    //                ProductId =
    //                    product.Id
    //            };

    //            product.Variants.Add(variant);
    //        }

    //        // ======================================
    //        // UPDATE VARIANT
    //        // ======================================

    //        variant.Sku =
    //            variantCommand.Sku.Trim();

    //        variant.Size =
    //            variantCommand.Size.Trim();

    //        variant.Color =
    //            variantCommand.Color.Trim();

    //        variant.IsActive =
    //            variantCommand.IsActive;

    //        // ======================================
    //        // SUBMITTED IMAGE IDS
    //        // ======================================

    //        var submittedImageIds =
    //            variantCommand.Images
    //                .Where(x => x.Id != Guid.Empty)
    //                .Select(x => x.Id)
    //                .ToHashSet();

    //        // ======================================
    //        // DELETE REMOVED IMAGES
    //        // ======================================

    //        var removedImages =
    //            variant.Images
    //                .Where(x =>
    //                    !submittedImageIds.Contains(x.Id))
    //                .ToList();

    //        foreach (var image in removedImages)
    //        {
    //            variant.Images.Remove(image);
    //        }

    //        // ======================================
    //        // ADD / UPDATE IMAGES
    //        // ======================================

    //        foreach (var imageCommand in variantCommand.Images)
    //        {
    //            ProductImage image;

    //            // ==================================
    //            // EXISTING IMAGE
    //            // ==================================

    //            if (imageCommand.Id != Guid.Empty)
    //            {
    //                image =
    //                    variant.Images
    //                        .FirstOrDefault(
    //                            x => x.Id == imageCommand.Id);

    //                // ==================================
    //                // INVALID IMAGE ID
    //                // ==================================

    //                if (image == null)
    //                {
    //                    throw new Exception(
    //                        $"Product image '{imageCommand.Id}' " +
    //                        $"does not belong to variant " +
    //                        $"'{variant.Id}'.");
    //                }
    //            }
    //            else
    //            {
    //                // ==================================
    //                // NEW IMAGE
    //                // ==================================

    //                image = new ProductImage
    //                {
    //                    Id =
    //                        IdentityGenerator
    //                            .NewSequentialGuid(),

    //                    ProductVariantId =
    //                        variant.Id
    //                };

    //                variant.Images.Add(image);
    //            }

    //            // ==================================
    //            // UPDATE IMAGE
    //            // ==================================

    //            image.ImageName =
    //                imageCommand.ImageName;

    //            image.DisplayOrder =
    //                imageCommand.DisplayOrder;

    //            image.IsPrimary =
    //                imageCommand.IsPrimary;
    //        }
    //    }

    //    // ==========================================
    //    // SAVE
    //    // ==========================================

    //    await _unitOfWork.SaveAsync(
    //        cancellationToken);

    //    return product;
    //}
}