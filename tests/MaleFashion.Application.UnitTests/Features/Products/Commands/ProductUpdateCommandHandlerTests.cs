using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Products.Command;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Products.Commands
{

    public class ProductUpdateCommandHandlerTests
    {
        private AutoMock _mock;
        private ProductUpdateCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork =
                _mock.Mock<IApplicationUnitOfWork>();

            _mockProductRepository =
                _mock.Mock<IProductRepository>();

            _handler =
                _mock.Create<ProductUpdateCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();
            _mockUnitOfWork?.Reset();
        }

        // =========================================================
        // 1. UPDATE EXISTING PRODUCT
        // =========================================================

        [Test]
        public async Task Handle_ValidCommand_UpdatesProduct()
        {
            // Arrange

            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Old Product",
                Branding = "Old Brand",
                ProductPrize = 1000m,
                Tags = new List<string>
                {
                    "Old"
                },
                Description = "Old Description",
                CustomerPreview = "Old Preview",
                AdditionalInfo = "Old Info",
                IsActive = true,
                CategoryId = Guid.NewGuid()
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "  Updated Product  ",
                Branding = "  Updated Brand  ",
                ProductPrize = 2500m,
                Tags = new List<string>
                {
                    "  Men ",
                    "Fashion",
                    "men",
                    "",
                    "   "
                },
                Description = "Updated Description",
                CustomerPreview = "Updated Preview",
                AdditionalInfo = "Updated Info",
                IsActive = false,
                CategoryId = categoryId
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),

                () => result.Id.ShouldBe(productId),

                () => result.ProductName
                    .ShouldBe("Updated Product"),

                () => result.Branding
                    .ShouldBe("Updated Brand"),

                () => result.ProductPrize
                    .ShouldBe(2500m),

                () => result.Description
                    .ShouldBe("Updated Description"),

                () => result.CustomerPreview
                    .ShouldBe("Updated Preview"),

                () => result.AdditionalInfo
                    .ShouldBe("Updated Info"),

                () => result.IsActive
                    .ShouldBeFalse(),

                () => result.CategoryId
                    .ShouldBe(categoryId),

                () => result.Tags.Count
                    .ShouldBe(2),

                () => result.Tags
                    .ShouldContain("Men"),

                () => result.Tags
                    .ShouldContain("Fashion"),

                () => _mockProductRepository.Verify(
                    x => x.GetProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        // =========================================================
        // 2. PRODUCT DOES NOT EXIST
        // =========================================================

        [Test]
        public async Task Handle_ProductDoesNotExist_ThrowsException()
        {
            // Arrange

            var productId = Guid.NewGuid();

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 1000m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid()
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act

            var exception =
                await Should.ThrowAsync<Exception>(
                    async () =>
                        await _handler.Handle(
                            command,
                            CancellationToken.None));

            // Assert

            this.ShouldSatisfyAllConditions(
                () => exception.Message
                    .ShouldBe("Product doesn't exist."),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Never)
            );
        }

        // =========================================================
        // 3. ADD NEW VARIANT
        // =========================================================

        [Test]
        public async Task Handle_NewVariant_AddsVariant()
        {
            // Arrange

            var productId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                Variants = new List<ProductVariant>()
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 1500m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid(),

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Id = Guid.Empty,
                        Sku = "  SKU-001  ",
                        Size = "  L  ",
                        Color = "  Black  ",
                        IsActive = true
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            var variant =
                result.Variants.Single();

            this.ShouldSatisfyAllConditions(
                () => result.Variants.Count
                    .ShouldBe(1),

                () => variant.Id
                    .ShouldNotBe(Guid.Empty),

                () => variant.ProductId
                    .ShouldBe(productId),

                () => variant.Sku
                    .ShouldBe("SKU-001"),

                () => variant.Size
                    .ShouldBe("L"),

                () => variant.Color
                    .ShouldBe("Black"),

                () => variant.IsActive
                    .ShouldBeTrue(),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        // =========================================================
        // 4. UPDATE EXISTING VARIANT
        // =========================================================

        [Test]
        public async Task Handle_ExistingVariant_UpdatesVariant()
        {
            // Arrange

            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();

            var existingVariant = new ProductVariant
            {
                Id = variantId,
                ProductId = productId,
                Sku = "OLD-SKU",
                Size = "M",
                Color = "White",
                IsActive = true
            };

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                Variants = new List<ProductVariant>
                {
                    existingVariant
                }
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 2000m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid(),

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Id = variantId,
                        Sku = "  NEW-SKU  ",
                        Size = "  XL  ",
                        Color = "  Black  ",
                        IsActive = false
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            var variant =
                result.Variants.Single();

            this.ShouldSatisfyAllConditions(
                () => result.Variants.Count
                    .ShouldBe(1),

                () => variant.Id
                    .ShouldBe(variantId),

                () => variant.Sku
                    .ShouldBe("NEW-SKU"),

                () => variant.Size
                    .ShouldBe("XL"),

                () => variant.Color
                    .ShouldBe("Black"),

                () => variant.IsActive
                    .ShouldBeFalse()
            );
        }


        // =========================================================
        // 6. ADD NEW IMAGE
        // =========================================================

        [Test]
        public async Task Handle_NewImage_AddsImage()
        {
            // Arrange

            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();

            var variant = new ProductVariant
            {
                Id = variantId,
                ProductId = productId,
                Sku = "SKU-001",
                Size = "L",
                Color = "Black",
                IsActive = true,
                Images = new List<ProductImage>()
            };

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                Variants = new List<ProductVariant>
                {
                    variant
                }
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 2000m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid(),

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Id = variantId,
                        Sku = "SKU-001",
                        Size = "L",
                        Color = "Black",
                        IsActive = true,

                        Images = new List<ProductImageCommand>
                        {
                            new ProductImageCommand
                            {
                                Id = Guid.Empty,
                                ImageName = "product.jpg",
                                DisplayOrder = 1,
                                IsPrimary = true
                            }
                        }
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            var updatedVariant =
                result.Variants.Single();

            var image =
                updatedVariant.Images.Single();

            this.ShouldSatisfyAllConditions(
                () => updatedVariant.Images.Count
                    .ShouldBe(1),

                () => image.Id
                    .ShouldNotBe(Guid.Empty),

                () => image.ProductVariantId
                    .ShouldBe(variantId),

                () => image.ImageName
                    .ShouldBe("product.jpg"),

                () => image.DisplayOrder
                    .ShouldBe(1),

                () => image.IsPrimary
                    .ShouldBeTrue()
            );
        }

        // =========================================================
        // 7. UPDATE EXISTING IMAGE
        // =========================================================

        [Test]
        public async Task Handle_ExistingImage_UpdatesImage()
        {
            // Arrange

            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var imageId = Guid.NewGuid();

            var image = new ProductImage
            {
                Id = imageId,
                ProductVariantId = variantId,
                ImageName = "old.jpg",
                DisplayOrder = 1,
                IsPrimary = false
            };

            var variant = new ProductVariant
            {
                Id = variantId,
                ProductId = productId,
                Sku = "SKU-001",
                Size = "L",
                Color = "Black",
                IsActive = true,
                Images = new List<ProductImage>
                {
                    image
                }
            };

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                Variants = new List<ProductVariant>
                {
                    variant
                }
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 2000m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid(),

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Id = variantId,
                        Sku = "SKU-001",
                        Size = "L",
                        Color = "Black",
                        IsActive = true,

                        Images = new List<ProductImageCommand>
                        {
                            new ProductImageCommand
                            {
                                Id = imageId,
                                ImageName = "new.jpg",
                                DisplayOrder = 5,
                                IsPrimary = true
                            }
                        }
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            var updatedImage =
                result.Variants
                    .Single()
                    .Images
                    .Single();

            this.ShouldSatisfyAllConditions(
                () => updatedImage.Id
                    .ShouldBe(imageId),

                () => updatedImage.ImageName
                    .ShouldBe("new.jpg"),

                () => updatedImage.DisplayOrder
                    .ShouldBe(5),

                () => updatedImage.IsPrimary
                    .ShouldBeTrue()
            );
        }

        // =========================================================
        // 8. REMOVE IMAGE
        // =========================================================

        [Test]
        public async Task Handle_RemovedImage_RemovesImage()
        {
            // Arrange

            var productId = Guid.NewGuid();
            var variantId = Guid.NewGuid();

            var imageToRemove = new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductVariantId = variantId,
                ImageName = "remove.jpg",
                DisplayOrder = 1,
                IsPrimary = false
            };

            var imageToKeep = new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductVariantId = variantId,
                ImageName = "keep.jpg",
                DisplayOrder = 2,
                IsPrimary = true
            };

            var variant = new ProductVariant
            {
                Id = variantId,
                ProductId = productId,
                Sku = "SKU-001",
                Size = "L",
                Color = "Black",
                IsActive = true,

                Images = new List<ProductImage>
                {
                    imageToRemove,
                    imageToKeep
                }
            };

            var existingProduct = new Product
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                Variants = new List<ProductVariant>
                {
                    variant
                }
            };

            var command = new ProductUpdateCommand
            {
                Id = productId,
                ProductName = "Product",
                Branding = "Brand",
                ProductPrize = 2000m,
                Tags = new List<string>(),
                Description = "Description",
                CustomerPreview = "Preview",
                AdditionalInfo = "Info",
                IsActive = true,
                CategoryId = Guid.NewGuid(),

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Id = variantId,
                        Sku = "SKU-001",
                        Size = "L",
                        Color = "Black",
                        IsActive = true,

                        Images = new List<ProductImageCommand>
                        {
                            new ProductImageCommand
                            {
                                Id = imageToKeep.Id,
                                ImageName = "keep.jpg",
                                DisplayOrder = 2,
                                IsPrimary = true
                            }
                        }
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.GetProductDetailsAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            var updatedVariant =
                result.Variants.Single();

            this.ShouldSatisfyAllConditions(
                () => updatedVariant.Images.Count
                    .ShouldBe(1),

                () => updatedVariant.Images
                    .ShouldNotContain(imageToRemove),

                () => updatedVariant.Images
                    .ShouldContain(imageToKeep)
            );
        }



    }
}