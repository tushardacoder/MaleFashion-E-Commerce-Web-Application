using Autofac.Extras.Moq;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Products.Query;
using MaleFashion.Domain.Entities;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.UnitTests.Features.Products.Queries
{
 
 public class GetShopProductDetailsQueryHandlerTests
    {
        private AutoMock _mock;

        private GetShopProductDetailsQueryHandler _handler;

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

            _mockUnitOfWork
                .SetupGet(x => x.ProductRepository)
                .Returns(_mockProductRepository.Object);

            _handler =
                _mock.Create<GetShopProductDetailsQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_ProductFound_ReturnsCompleteShopProductDetails()
        {
            // Arrange

            var productId =
                Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            var variantId1 =
                Guid.NewGuid();

            var variantId2 =
                Guid.NewGuid();

            var imageId1 =
                Guid.NewGuid();

            var imageId2 =
                Guid.NewGuid();

            var product =
                new Product
                {
                    Id = productId,

                    ProductName =
                        "Classic Shirt",

                    Branding =
                        "Fashion Brand",

                    ProductPrize =
                        2500m,

                    Description =
                        "Premium classic shirt",

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags = new List<string>
                    {
                        "Shirt",
                        "Casual"
                    },

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id = variantId1,

                                ProductId =
                                    productId,

                                Sku =
                                    "SKU-BLACK-M",

                                Color =
                                    "Black",

                                Size =
                                    "M",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variantId1,

                                        Quantity =
                                            10,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            imageId1,

                                        ProductVariantId =
                                            variantId1,

                                        ImageName =
                                            "shirt-black.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            true
                                    },

                                    new ProductImage
                                    {
                                        Id =
                                            imageId2,

                                        ProductVariantId =
                                            variantId1,

                                        ImageName =
                                            "shirt-black-side.jpg",

                                        DisplayOrder =
                                            2,

                                        IsPrimary =
                                            false
                                    }
                                }
                            },

                            new ProductVariant
                            {
                                Id = variantId2,

                                ProductId =
                                    productId,

                                Sku =
                                    "SKU-WHITE-L",

                                Color =
                                    "White",

                                Size =
                                    "L",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variantId2,

                                        Quantity =
                                            0,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>()
                            },

                            new ProductVariant
                            {
                                Id =
                                    Guid.NewGuid(),

                                ProductId =
                                    productId,

                                Sku =
                                    "SKU-BLUE-XL",

                                Color =
                                    "Blue",

                                Size =
                                    "XL",

                                IsActive =
                                    false,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            Guid.NewGuid(),

                                        Quantity =
                                            20,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>()
                            }
                        }
                };

            var relatedProductId =
                Guid.NewGuid();

            var relatedProductVariantId =
                Guid.NewGuid();

            var relatedProduct =
                new Product
                {
                    Id =
                        relatedProductId,

                    ProductName =
                        "Slim Fit Shirt",

                    Branding =
                        "Related Brand",

                    ProductPrize =
                        3000m,

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>
                        {
                            "Formal",
                            "Shirt"
                        },

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    relatedProductVariantId,

                                ProductId =
                                    relatedProductId,

                                Color =
                                    "Black",

                                Size =
                                    "M",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            relatedProductVariantId,

                                        Quantity =
                                            5,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            relatedProductVariantId,

                                        ImageName =
                                            "related-shirt.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            true
                                    }
                                }
                            }
                        }
                };

            var query =
                new GetShopProductDetailsQuery(
                    productId);

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(x =>
                    x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        relatedProduct
                    });

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            var firstVariant =
                result.Variants.ElementAt(0);

            var secondVariant =
                result.Variants.ElementAt(1);

            var relatedProductResult =
                result.RelatedProducts.ElementAt(0);

            this.ShouldSatisfyAllConditions(

                () => result
                    .ShouldNotBeNull(),

                () => result.Id
                    .ShouldBe(productId),

                () => result.ProductName
                    .ShouldBe("Classic Shirt"),

                () => result.Branding
                    .ShouldBe("Fashion Brand"),

                () => result.Price
                    .ShouldBe(2500m),

                () => result.Description
                    .ShouldBe("Premium classic shirt"),

                () => result.IsActive
                    .ShouldBeTrue(),

                () => result.IsInStock
                    .ShouldBeTrue(),

                () => result.Tags.Count
                    .ShouldBe(2),

                () => result.Tags
                    .ShouldContain("Shirt"),

                () => result.Tags
                    .ShouldContain("Casual"),

                () => result.PrimaryImage
                    .ShouldBe("shirt-black.jpg"),

                () => result.Images.Count
                    .ShouldBe(2),

                () => result.Images
                    .ShouldContain("shirt-black.jpg"),

                () => result.Images
                    .ShouldContain("shirt-black-side.jpg"),

                () => result.Colors.Count
                    .ShouldBe(2),

                () => result.Colors
                    .ShouldContain("Black"),

                () => result.Colors
                    .ShouldContain("White"),

                () => result.Sizes.Count
                    .ShouldBe(2),

                () => result.Sizes
                    .ShouldContain("M"),

                () => result.Sizes
                    .ShouldContain("L"),

                () => result.Variants.Count
                    .ShouldBe(3),

                () => firstVariant.Id
                    .ShouldBe(variantId1),

                () => firstVariant.Color
                    .ShouldBe("Black"),

                () => firstVariant.Size
                    .ShouldBe("M"),

                () => firstVariant.IsInStock
                    .ShouldBeTrue(),

                () => firstVariant.InventoryQuantity
                    .ShouldBe(10),

                () => secondVariant.Id
                    .ShouldBe(variantId2),

                () => secondVariant.Color
                    .ShouldBe("White"),

                () => secondVariant.Size
                    .ShouldBe("L"),

                () => secondVariant.IsInStock
                    .ShouldBeFalse(),

                () => secondVariant.InventoryQuantity
                    .ShouldBe(0),

                () => result.RelatedProducts.Count
                    .ShouldBe(1),

                () => relatedProductResult.Id
                    .ShouldBe(relatedProductId),

                () => relatedProductResult.ProductName
                    .ShouldBe("Slim Fit Shirt"),

                () => relatedProductResult.Branding
                    .ShouldBe("Related Brand"),

                () => relatedProductResult.Price
                    .ShouldBe(3000m),

                () => relatedProductResult.PrimaryImage
                    .ShouldBe("related-shirt.jpg"),

                () => relatedProductResult.Images
                    .ShouldContain("related-shirt.jpg"),

                () => relatedProductResult.Colors
                    .ShouldContain("Black"),

                () => relatedProductResult.Sizes
                    .ShouldContain("M"),

                () => relatedProductResult.IsInStock
                    .ShouldBeTrue(),

                () => _mockProductRepository.Verify(
                    x => x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()),
                    Times.Once),

                () => _mockProductRepository.Verify(
                    x => x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

       

        [Test]
        public async Task Handle_ProductWithoutPrimaryImage_UsesFirstImage()
        {
            // Arrange

            var productId =
                Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            var variantId =
                Guid.NewGuid();

            var product =
                new Product
                {
                    Id =
                        productId,

                    ProductName =
                        "No Primary Image Product",

                    Branding =
                        "Brand",

                    ProductPrize =
                        1500m,

                    Description =
                        "Description",

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>(),

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    variantId,

                                ProductId =
                                    productId,

                                Color =
                                    "Black",

                                Size =
                                    "M",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variantId,

                                        Quantity =
                                            5,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variantId,

                                        ImageName =
                                            "first.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            false
                                    },

                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variantId,

                                        ImageName =
                                            "second.jpg",

                                        DisplayOrder =
                                            2,

                                        IsPrimary =
                                            false
                                    }
                                }
                            }
                        }
                };

            var query =
                new GetShopProductDetailsQuery(
                    productId);

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(x =>
                    x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>());

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.PrimaryImage
                    .ShouldBe("first.jpg"),

                () => result.Images.Count
                    .ShouldBe(2),

                () => result.Images.ElementAt(0)
                    .ShouldBe("first.jpg"),

                () => result.Images.ElementAt(1)
                    .ShouldBe("second.jpg")
            );
        }

        [Test]
        public async Task Handle_RelatedProductWithoutPrimaryImage_UsesFirstOrderedImage()
        {
            // Arrange

            var productId =
                Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            var relatedProductId =
                Guid.NewGuid();

            var relatedVariantId =
                Guid.NewGuid();

            var product =
                new Product
                {
                    Id =
                        productId,

                    ProductName =
                        "Main Product",

                    Branding =
                        "Brand",

                    ProductPrize =
                        2000m,

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>(),

                    Variants =
                        new List<ProductVariant>()
                };

            var relatedProduct =
                new Product
                {
                    Id =
                        relatedProductId,

                    ProductName =
                        "Related Product",

                    Branding =
                        "Related Brand",

                    ProductPrize =
                        1800m,

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>
                        {
                            "Casual"
                        },

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    relatedVariantId,

                                ProductId =
                                    relatedProductId,

                                Color =
                                    "Blue",

                                Size =
                                    "L",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            relatedVariantId,

                                        Quantity =
                                            3,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            relatedVariantId,

                                        ImageName =
                                            "second.jpg",

                                        DisplayOrder =
                                            2,

                                        IsPrimary =
                                            false
                                    },

                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            relatedVariantId,

                                        ImageName =
                                            "first.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            false
                                    }
                                }
                            }
                        }
                };

            var query =
                new GetShopProductDetailsQuery(
                    productId);

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(x =>
                    x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        relatedProduct
                    });

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            var related =
                result.RelatedProducts
                    .ElementAt(0);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => related.PrimaryImage
                    .ShouldBe("first.jpg"),

                () => related.Images.Count
                    .ShouldBe(2),

                () => related.Images.ElementAt(0)
                    .ShouldBe("first.jpg"),

                () => related.Images.ElementAt(1)
                    .ShouldBe("second.jpg"),

                () => related.IsInStock
                    .ShouldBeTrue(),

                () => related.Colors
                    .ShouldContain("Blue"),

                () => related.Sizes
                    .ShouldContain("L")
            );
        }

        [Test]
        public async Task Handle_VariantWithoutInventory_ReturnsZeroInventoryAndOutOfStock()
        {
            // Arrange

            var productId =
                Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            var variantId =
                Guid.NewGuid();

            var product =
                new Product
                {
                    Id =
                        productId,

                    ProductName =
                        "Test Product",

                    Branding =
                        "Brand",

                    ProductPrize =
                        1000m,

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>(),

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    variantId,

                                ProductId =
                                    productId,

                                Color =
                                    "Black",

                                Size =
                                    "M",

                                IsActive =
                                    true,

                                Inventory =
                                    null,

                                Images =
                                    new List<ProductImage>()
                            }
                        }
                };

            var query =
                new GetShopProductDetailsQuery(
                    productId);

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(x =>
                    x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>());

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            var variant =
                result.Variants.ElementAt(0);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.IsInStock
                    .ShouldBeFalse(),

                () => variant.IsInStock
                    .ShouldBeFalse(),

                () => variant.InventoryQuantity
                    .ShouldBe(0)
            );
        }

        [Test]
        public async Task Handle_InactiveVariant_IsExcludedFromColorsSizesImagesAndStock()
        {
            // Arrange

            var productId =
                Guid.NewGuid();

            var categoryId =
                Guid.NewGuid();

            var activeVariantId =
                Guid.NewGuid();

            var inactiveVariantId =
                Guid.NewGuid();

            var product =
                new Product
                {
                    Id =
                        productId,

                    ProductName =
                        "Test Product",

                    Branding =
                        "Brand",

                    ProductPrize =
                        2000m,

                    CategoryId =
                        categoryId,

                    IsActive =
                        true,

                    Tags =
                        new List<string>(),

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    activeVariantId,

                                ProductId =
                                    productId,

                                Color =
                                    "Black",

                                Size =
                                    "M",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            activeVariantId,

                                        Quantity =
                                            5,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            activeVariantId,

                                        ImageName =
                                            "active.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            true
                                    }
                                }
                            },

                            new ProductVariant
                            {
                                Id =
                                    inactiveVariantId,

                                ProductId =
                                    productId,

                                Color =
                                    "Red",

                                Size =
                                    "XL",

                                IsActive =
                                    false,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            inactiveVariantId,

                                        Quantity =
                                            20,

                                        IsActive =
                                            true
                                    },

                                Images =
                                    new List<ProductImage>
                                {
                                    new ProductImage
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            inactiveVariantId,

                                        ImageName =
                                            "inactive.jpg",

                                        DisplayOrder =
                                            1,

                                        IsPrimary =
                                            true
                                    }
                                }
                            }
                        }
                };

            var query =
                new GetShopProductDetailsQuery(
                    productId);

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductDetailsAsync(
                        productId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(x =>
                    x.GetRelatedProductsAsync(
                        productId,
                        categoryId,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>());

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.Colors.Count
                    .ShouldBe(1),

                () => result.Colors
                    .ShouldContain("Black"),

                () => result.Colors
                    .ShouldNotContain("Red"),

                () => result.Sizes.Count
                    .ShouldBe(1),

                () => result.Sizes
                    .ShouldContain("M"),

                () => result.Sizes
                    .ShouldNotContain("XL"),

                () => result.Images.Count
                    .ShouldBe(1),

                () => result.Images
                    .ShouldContain("active.jpg"),

                () => result.Images
                    .ShouldNotContain("inactive.jpg"),

                () => result.IsInStock
                    .ShouldBeTrue()
            );
        }
    }
}