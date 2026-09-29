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

  public class ProductAddCommandHandlerTests
    {
        private AutoMock _mock;

        private ProductAddCommandHandler _handler;

        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;

        private Mock<IProductRepository> _mockProductRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock =
                AutoMock.GetLoose();
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
                _mock.Create<ProductAddCommandHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_ValidCommand_CreatesProductWithVariantsAndImages()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var command = new ProductAddCommand
            {
                ProductName = "  Classic Shirt  ",

                Branding = "  Fashion Brand  ",

                ProductPrize = 2500m,

                Tags = new List<string>
                {
                    " Shirt ",
                    "Casual",
                    "shirt",
                    "",
                    "   "
                },

                Description = "Product description",

                CustomerPreview = "Customer preview",

                AdditionalInfo = "Additional information",

                IsActive = true,

                CategoryId = categoryId,

                Variants = new List<ProductVariantCommand>
                {
                    new ProductVariantCommand
                    {
                        Sku = " SKU-001 ",

                        Size = " M ",

                        Color = " Black ",

                        IsActive = true,

                        Images = new List<ProductImageCommand>
                        {
                            new ProductImageCommand
                            {
                                ImageName = "shirt-black.jpg",

                                DisplayOrder = 1,

                                IsPrimary = true
                            },

                            new ProductImageCommand
                            {
                                ImageName = "shirt-black-side.jpg",

                                DisplayOrder = 2,

                                IsPrimary = false
                            }
                        }
                    }
                }
            };

            Product addedProduct = null!;

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>(
                    (product, cancellationToken) =>
                    {
                        addedProduct = product;
                    })
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            var variant = result.Variants.ElementAt(0);

            var firstImage = variant.Images.ElementAt(0);

            var secondImage = variant.Images.ElementAt(1);

            // Assert
            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Id.ShouldNotBe(Guid.Empty),

                () => result.ProductName
                    .ShouldBe("Classic Shirt"),

                () => result.Branding
                    .ShouldBe("Fashion Brand"),

                () => result.ProductPrize
                    .ShouldBe(2500m),

                () => result.Description
                    .ShouldBe("Product description"),

                () => result.CustomerPreview
                    .ShouldBe("Customer preview"),

                () => result.AdditionalInfo
                    .ShouldBe("Additional information"),

                () => result.IsActive
                    .ShouldBeTrue(),

                () => result.CategoryId
                    .ShouldBe(categoryId),

                () => result.Tags.Count
                    .ShouldBe(2),

                () => result.Tags
                    .ShouldContain("Shirt"),

                () => result.Tags
                    .ShouldContain("Casual"),

                () => result.Variants.Count
                    .ShouldBe(1),

                () => variant.Id
                    .ShouldNotBe(Guid.Empty),

                () => variant.ProductId
                    .ShouldBe(result.Id),

                () => variant.Sku
                    .ShouldBe("SKU-001"),

                () => variant.Size
                    .ShouldBe("M"),

                () => variant.Color
                    .ShouldBe("Black"),

                () => variant.IsActive
                    .ShouldBeTrue(),

                () => variant.Images.Count
                    .ShouldBe(2),

                () => firstImage.Id
                    .ShouldNotBe(Guid.Empty),

                () => firstImage.ProductVariantId
                    .ShouldBe(variant.Id),

                () => firstImage.ImageName
                    .ShouldBe("shirt-black.jpg"),

                () => firstImage.DisplayOrder
                    .ShouldBe(1),

                () => firstImage.IsPrimary
                    .ShouldBeTrue(),

                () => secondImage.ImageName
                    .ShouldBe("shirt-black-side.jpg"),

                () => secondImage.DisplayOrder
                    .ShouldBe(2),

                () => secondImage.IsPrimary
                    .ShouldBeFalse(),

                () => addedProduct
                    .ShouldBeSameAs(result),

                () => _mockProductRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Product>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_ProductWithMultipleVariants_CreatesAllVariants()
        {
            // Arrange
            var command = new ProductAddCommand
            {
                ProductName = "T-Shirt",

                Branding = "Brand",

                ProductPrize = 1200m,

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
                        Sku = "SKU-BLACK-M",

                        Size = "M",

                        Color = "Black",

                        IsActive = true
                    },

                    new ProductVariantCommand
                    {
                        Sku = "SKU-WHITE-L",

                        Size = "L",

                        Color = "White",

                        IsActive = true
                    },

                    new ProductVariantCommand
                    {
                        Sku = "SKU-BLUE-XL",

                        Size = "XL",

                        Color = "Blue",

                        IsActive = false
                    }
                }
            };

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _mockProductRepository
                .Setup(x => x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(x => x.SaveAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            var firstVariant =
                result.Variants.ElementAt(0);

            var secondVariant =
                result.Variants.ElementAt(1);

            var thirdVariant =
                result.Variants.ElementAt(2);

            // Assert
            this.ShouldSatisfyAllConditions(

                () => result.ShouldNotBeNull(),

                () => result.Variants.Count
                    .ShouldBe(3),

                () => firstVariant.Sku
                    .ShouldBe("SKU-BLACK-M"),

                () => firstVariant.Size
                    .ShouldBe("M"),

                () => firstVariant.Color
                    .ShouldBe("Black"),

                () => firstVariant.IsActive
                    .ShouldBeTrue(),

                () => secondVariant.Sku
                    .ShouldBe("SKU-WHITE-L"),

                () => secondVariant.Size
                    .ShouldBe("L"),

                () => secondVariant.Color
                    .ShouldBe("White"),

                () => secondVariant.IsActive
                    .ShouldBeTrue(),

                () => thirdVariant.Sku
                    .ShouldBe("SKU-BLUE-XL"),

                () => thirdVariant.Size
                    .ShouldBe("XL"),

                () => thirdVariant.Color
                    .ShouldBe("Blue"),

                () => thirdVariant.IsActive
                    .ShouldBeFalse(),

                () => result.Variants.All(
                    x => x.ProductId == result.Id),

                () => _mockProductRepository.Verify(
                    x => x.AddAsync(
                        It.IsAny<Product>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockUnitOfWork.Verify(
                    x => x.SaveAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        
           
      
        
    }
}