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
   
    public class GetShopProductsQueryHandlerTests
    {
        private AutoMock _mock;

        private GetShopProductsQueryHandler _handler;

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
                _mock.Create<GetShopProductsQueryHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockProductRepository?.Reset();

            _mockUnitOfWork?.Reset();
        }

        [Test]
        public async Task Handle_ValidQuery_ReturnsProductsWithCorrectDetails()
        {
            // Arrange

            var categoryId =
                Guid.NewGuid();

            var product1Id =
                Guid.NewGuid();

            var product2Id =
                Guid.NewGuid();

            var variant1Id =
                Guid.NewGuid();

            var variant2Id =
                Guid.NewGuid();

            var category =
                new Category
                {
                    Id = categoryId,

                    CategoryName =
                        "Shirts"
                };

            var product1 =
                new Product
                {
                    Id =
                        product1Id,

                    ProductName =
                        "Classic Shirt",

                    Branding =
                        "Fashion Brand",

                    ProductPrize =
                        2500m,

                    Description =
                        "Classic cotton shirt",

                    CategoryId =
                        categoryId,

                    Category =
                        category,

                    IsActive =
                        true,

                    Tags =
                        new List<string>
                        {
                            "Shirt",
                            "Casual"
                        },

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    variant1Id,

                                ProductId =
                                    product1Id,

                                Size =
                                    "M",

                                Color =
                                    "Black",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variant1Id,

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
                                                Guid.NewGuid(),

                                            ProductVariantId =
                                                variant1Id,

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
                                                Guid.NewGuid(),

                                            ProductVariantId =
                                                variant1Id,

                                            ImageName =
                                                "shirt-black-side.jpg",

                                            DisplayOrder =
                                                2,

                                            IsPrimary =
                                                false
                                        }
                                    }
                            }
                        }
                };

            var product2 =
                new Product
                {
                    Id =
                        product2Id,

                    ProductName =
                        "Polo Shirt",

                    Branding =
                        "Premium Brand",

                    ProductPrize =
                        3000m,

                    Description =
                        "Premium polo shirt",

                    CategoryId =
                        categoryId,

                    Category =
                        category,

                    IsActive =
                        true,

                    Tags =
                        new List<string>
                        {
                            "Polo"
                        },

                    Variants =
                        new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                Id =
                                    variant2Id,

                                ProductId =
                                    product2Id,

                                Size =
                                    "L",

                                Color =
                                    "White",

                                IsActive =
                                    true,

                                Inventory =
                                    new Inventory
                                    {
                                        Id =
                                            Guid.NewGuid(),

                                        ProductVariantId =
                                            variant2Id,

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
                                                variant2Id,

                                            ImageName =
                                                "polo-white.jpg",

                                            DisplayOrder =
                                                1,

                                            IsPrimary =
                                                true
                                        }
                                    }
                            }
                        }
                };

            var products =
                new List<Product>
                {
                    product1,
                    product2
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Page =
                        1,

                    PageSize =
                        12
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            var firstProduct =
                result.Products.ElementAt(0);

            var secondProduct =
                result.Products.ElementAt(1);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result
                    .ShouldNotBeNull(),

                () => result.Products
                    .Count
                    .ShouldBe(2),

                () => result.TotalProducts
                    .ShouldBe(2),

                () => result.CurrentPage
                    .ShouldBe(1),

                () => result.PageSize
                    .ShouldBe(12),

                () => firstProduct.Id
                    .ShouldBe(product1Id),

                () => firstProduct.ProductName
                    .ShouldBe("Classic Shirt"),

                () => firstProduct.Branding
                    .ShouldBe("Fashion Brand"),

                () => firstProduct.Price
                    .ShouldBe(2500m),

                () => firstProduct.PrimaryImage
                    .ShouldBe("shirt-black.jpg"),

                () => firstProduct.Images
                    .ShouldContain("shirt-black.jpg"),

                () => firstProduct.Images
                    .ShouldContain("shirt-black-side.jpg"),

                () => firstProduct.Colors
                    .ShouldContain("Black"),

                () => firstProduct.Sizes
                    .ShouldContain("M"),

                () => firstProduct.Tags
                    .ShouldContain("Shirt"),

                () => firstProduct.IsActive
                    .ShouldBeTrue(),

                () => firstProduct.IsInStock
                    .ShouldBeTrue(),

                () => secondProduct.ProductName
                    .ShouldBe("Polo Shirt"),

                () => secondProduct.PrimaryImage
                    .ShouldBe("polo-white.jpg"),

                () => result.Categories
                    .Count
                    .ShouldBe(1),

                () => result.Categories
                    .ElementAt(0)
                    .Name
                    .ShouldBe("Shirts"),

                () => result.Categories
                    .ElementAt(0)
                    .ProductCount
                    .ShouldBe(2),

                () => result.Brands
                    .ShouldContain("Fashion Brand"),

                () => result.Brands
                    .ShouldContain("Premium Brand"),

                () => result.Sizes
                    .ShouldContain("M"),

                () => result.Sizes
                    .ShouldContain("L"),

                () => result.Colors
                    .ShouldContain("Black"),

                () => result.Colors
                    .ShouldContain("White"),

                () => result.Tags
                    .ShouldContain("Shirt"),

                () => result.Tags
                    .ShouldContain("Casual"),

                () => result.Tags
                    .ShouldContain("Polo"),

                () => _mockProductRepository.Verify(
                    x => x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once)
            );
        }

        [Test]
        public async Task Handle_SearchFilter_ReturnsMatchingProducts()
        {
            // Arrange

            var products =
                CreateProducts();

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Search =
                        "Classic"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.TotalProducts
                    .ShouldBe(1),

                () => result.Products.Count
                    .ShouldBe(1),

                () => result.Products
                    .ElementAt(0)
                    .ProductName
                    .ShouldBe("Classic Shirt")
            );
        }

        [Test]
        public async Task Handle_CategoryFilter_ReturnsProductsFromCategory()
        {
            // Arrange

            var categoryId =
                Guid.NewGuid();

            var otherCategoryId =
                Guid.NewGuid();

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Shirt",
                        "Brand",
                        2000m,
                        categoryId),

                    CreateProduct(
                        "Pant",
                        "Brand",
                        3000m,
                        otherCategoryId)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    CategoryId =
                        categoryId
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products.Count
                .ShouldBe(1);

            result.Products
                .ElementAt(0)
                .ProductName
                .ShouldBe("Shirt");
        }

        [Test]
        public async Task Handle_BrandFilter_ReturnsMatchingBrand()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Shirt",
                        "Nike",
                        2000m),

                    CreateProduct(
                        "Polo",
                        "Adidas",
                        3000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Brand =
                        "nike"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products
                .ElementAt(0)
                .Branding
                .ShouldBe("Nike");
        }

        [Test]
        public async Task Handle_PriceFilter_ReturnsProductsWithinPriceRange()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Cheap Shirt",
                        "Brand",
                        1000m),

                    CreateProduct(
                        "Medium Shirt",
                        "Brand",
                        2500m),

                    CreateProduct(
                        "Expensive Shirt",
                        "Brand",
                        5000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    MinPrice =
                        2000m,

                    MaxPrice =
                        3000m
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products
                .ElementAt(0)
                .ProductName
                .ShouldBe("Medium Shirt");
        }

        [Test]
        public async Task Handle_SizeFilter_ReturnsProductsWithMatchingActiveVariant()
        {
            // Arrange

            var product =
                CreateProduct(
                    "Large Shirt",
                    "Brand",
                    2500m);

            product.Variants =
                new List<ProductVariant>
                {
                    CreateVariant(
                        "L",
                        "Black",
                        true,
                        5)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        product
                    });

            var query =
                new GetShopProductsQuery
                {
                    Size =
                        "L"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products.Count
                .ShouldBe(1);
        }

        [Test]
        public async Task Handle_SizeFilter_InactiveVariantDoesNotMatch()
        {
            // Arrange

            var product =
                CreateProduct(
                    "Shirt",
                    "Brand",
                    2500m);

            product.Variants =
                new List<ProductVariant>
                {
                    CreateVariant(
                        "XL",
                        "Black",
                        false,
                        10)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        product
                    });

            var query =
                new GetShopProductsQuery
                {
                    Size =
                        "XL"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(0);

            result.Products
                .ShouldBeEmpty();
        }

        [Test]
        public async Task Handle_ColorFilter_ReturnsProductsWithMatchingActiveVariant()
        {
            // Arrange

            var product =
                CreateProduct(
                    "Black Shirt",
                    "Brand",
                    2500m);

            product.Variants =
                new List<ProductVariant>
                {
                    CreateVariant(
                        "M",
                        "Black",
                        true,
                        10)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        product
                    });

            var query =
                new GetShopProductsQuery
                {
                    Color =
                        "Black"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products
                .ElementAt(0)
                .ProductName
                .ShouldBe("Black Shirt");
        }

        [Test]
        public async Task Handle_TagFilter_ReturnsProductsWithMatchingTag()
        {
            // Arrange

            var product1 =
                CreateProduct(
                    "Casual Shirt",
                    "Brand",
                    2000m);

            product1.Tags =
                new List<string>
                {
                    "Casual",
                    "Shirt"
                };

            var product2 =
                CreateProduct(
                    "Formal Shirt",
                    "Brand",
                    3000m);

            product2.Tags =
                new List<string>
                {
                    "Formal"
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        product1,
                        product2
                    });

            var query =
                new GetShopProductsQuery
                {
                    Tag =
                        "casual"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.TotalProducts
                .ShouldBe(1);

            result.Products
                .ElementAt(0)
                .ProductName
                .ShouldBe("Casual Shirt");
        }

        [Test]
        public async Task Handle_SortPriceLow_ReturnsProductsInAscendingPriceOrder()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Expensive",
                        "Brand",
                        5000m),

                    CreateProduct(
                        "Cheap",
                        "Brand",
                        1000m),

                    CreateProduct(
                        "Medium",
                        "Brand",
                        3000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Sort =
                        "price-low"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.Products.ElementAt(0).Price
                    .ShouldBe(1000m),

                () => result.Products.ElementAt(1).Price
                    .ShouldBe(3000m),

                () => result.Products.ElementAt(2).Price
                    .ShouldBe(5000m)
            );
        }

        [Test]
        public async Task Handle_SortPriceHigh_ReturnsProductsInDescendingPriceOrder()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Cheap",
                        "Brand",
                        1000m),

                    CreateProduct(
                        "Expensive",
                        "Brand",
                        5000m),

                    CreateProduct(
                        "Medium",
                        "Brand",
                        3000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Sort =
                        "price-high"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.Products.ElementAt(0).Price
                    .ShouldBe(5000m),

                () => result.Products.ElementAt(1).Price
                    .ShouldBe(3000m),

                () => result.Products.ElementAt(2).Price
                    .ShouldBe(1000m)
            );
        }

        [Test]
        public async Task Handle_SortName_ReturnsProductsInAscendingNameOrder()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Zebra Shirt",
                        "Brand",
                        2000m),

                    CreateProduct(
                        "Classic Shirt",
                        "Brand",
                        3000m),

                    CreateProduct(
                        "Modern Shirt",
                        "Brand",
                        2500m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Sort =
                        "name"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.Products.ElementAt(0).ProductName
                    .ShouldBe("Classic Shirt"),

                () => result.Products.ElementAt(1).ProductName
                    .ShouldBe("Modern Shirt"),

                () => result.Products.ElementAt(2).ProductName
                    .ShouldBe("Zebra Shirt")
            );
        }

        [Test]
        public async Task Handle_SortNameDesc_ReturnsProductsInDescendingNameOrder()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Classic Shirt",
                        "Brand",
                        3000m),

                    CreateProduct(
                        "Modern Shirt",
                        "Brand",
                        2500m),

                    CreateProduct(
                        "Zebra Shirt",
                        "Brand",
                        2000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Sort =
                        "name-desc"
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.Products.ElementAt(0).ProductName
                    .ShouldBe("Zebra Shirt"),

                () => result.Products.ElementAt(1).ProductName
                    .ShouldBe("Modern Shirt"),

                () => result.Products.ElementAt(2).ProductName
                    .ShouldBe("Classic Shirt")
            );
        }

        [Test]
        public async Task Handle_DefaultSort_ReturnsProductsByNameAscending()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Zebra",
                        "Brand",
                        2000m),

                    CreateProduct(
                        "Apple",
                        "Brand",
                        3000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery();

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            result.Products
                .ElementAt(0)
                .ProductName
                .ShouldBe("Apple");

            result.Products
                .ElementAt(1)
                .ProductName
                .ShouldBe("Zebra");
        }

        [Test]
        public async Task Handle_Pagination_ReturnsCorrectPage()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "A Product",
                        "Brand",
                        1000m),

                    CreateProduct(
                        "B Product",
                        "Brand",
                        2000m),

                    CreateProduct(
                        "C Product",
                        "Brand",
                        3000m),

                    CreateProduct(
                        "D Product",
                        "Brand",
                        4000m),

                    CreateProduct(
                        "E Product",
                        "Brand",
                        5000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Page =
                        2,

                    PageSize =
                        2
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.TotalProducts
                    .ShouldBe(5),

                () => result.CurrentPage
                    .ShouldBe(2),

                () => result.PageSize
                    .ShouldBe(2),

                () => result.Products.Count
                    .ShouldBe(2),

                () => result.Products.ElementAt(0)
                    .ProductName
                    .ShouldBe("C Product"),

                () => result.Products.ElementAt(1)
                    .ProductName
                    .ShouldBe("D Product")
            );
        }

        [Test]
        public async Task Handle_InvalidPageAndPageSize_UsesDefaultValues()
        {
            // Arrange

            var products =
                new List<Product>
                {
                    CreateProduct(
                        "Product 1",
                        "Brand",
                        1000m)
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query =
                new GetShopProductsQuery
                {
                    Page =
                        0,

                    PageSize =
                        0
                };

            // Act

            var result =
                await _handler.Handle(
                    query,
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result.CurrentPage
                    .ShouldBe(1),

                () => result.PageSize
                    .ShouldBe(12),

                () => result.Products.Count
                    .ShouldBe(1)
            );
        }

       
        [Test]
        public async Task Handle_OutOfStockVariant_IsExcludedFromProductImagesAndStock()
        {
            // Arrange

            var product =
                CreateProduct(
                    "Out Of Stock Shirt",
                    "Brand",
                    2000m);

            var variantId =
                Guid.NewGuid();

            product.Variants =
                new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        Id =
                            variantId,

                        ProductId =
                            product.Id,

                        Size =
                            "M",

                        Color =
                            "Black",

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
                                    0,

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
                                        "out-of-stock.jpg",

                                    DisplayOrder =
                                        1,

                                    IsPrimary =
                                        true
                                }
                            }
                    }
                };

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>
                    {
                        product
                    });

            // Act

            var result =
                await _handler.Handle(
                    new GetShopProductsQuery(),
                    CancellationToken.None);

            var mappedProduct =
                result.Products.ElementAt(0);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => mappedProduct.IsInStock
                    .ShouldBeFalse(),

                () => mappedProduct.PrimaryImage
                    .ShouldBeNull(),

                () => mappedProduct.Images
                    .ShouldBeEmpty(),

                () => mappedProduct.Colors
                    .ShouldBeEmpty(),

                () => mappedProduct.Sizes
                    .ShouldBeEmpty()
            );
        }

        [Test]
        public async Task Handle_EmptyProducts_ReturnsEmptyShopViewModel()
        {
            // Arrange

            _mockProductRepository
                .Setup(x =>
                    x.GetShopProductsAsync(
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Product>());

            // Act

            var result =
                await _handler.Handle(
                    new GetShopProductsQuery(),
                    CancellationToken.None);

            // Assert

            this.ShouldSatisfyAllConditions(

                () => result
                    .ShouldNotBeNull(),

                () => result.Products
                    .ShouldBeEmpty(),

                () => result.TotalProducts
                    .ShouldBe(0),

                () => result.Categories
                    .ShouldBeEmpty(),

                () => result.Brands
                    .ShouldBeEmpty(),

                () => result.Sizes
                    .ShouldBeEmpty(),

                () => result.Colors
                    .ShouldBeEmpty(),

                () => result.Tags
                    .ShouldBeEmpty()
            );
        }

        // ==========================================================
        // HELPER METHODS
        // ==========================================================

        private static List<Product> CreateProducts()
        {
            return new List<Product>
            {
                CreateProduct(
                    "Classic Shirt",
                    "Fashion Brand",
                    2500m),

                CreateProduct(
                    "Polo Shirt",
                    "Premium Brand",
                    3500m),

                CreateProduct(
                    "Formal Pant",
                    "Formal Brand",
                    4500m)
            };
        }

        private static Product CreateProduct(
            string productName,
            string branding,
            decimal price,
            Guid? categoryId = null)
        {
            var productId =
                Guid.NewGuid();

            var actualCategoryId =
                categoryId ??
                Guid.NewGuid();

            var variantId =
                Guid.NewGuid();

            return new Product
            {
                Id =
                    productId,

                ProductName =
                    productName,

                Branding =
                    branding,

                ProductPrize =
                    price,

                Description =
                    $"{productName} description",

                CategoryId =
                    actualCategoryId,

                IsActive =
                    true,

                Tags =
                    new List<string>
                    {
                        "Shirt"
                    },

                Variants =
                    new List<ProductVariant>
                    {
                        CreateVariant(
                            "M",
                            "Black",
                            true,
                            10)
                    }
            };
        }

        private static ProductVariant CreateVariant(
            string size,
            string color,
            bool isActive,
            int quantity)
        {
            var variantId =
                Guid.NewGuid();

            return new ProductVariant
            {
                Id =
                    variantId,

                ProductId =
                    Guid.NewGuid(),

                Size =
                    size,

                Color =
                    color,

                IsActive =
                    isActive,

                Inventory =
                    new Inventory
                    {
                        Id =
                            Guid.NewGuid(),

                        ProductVariantId =
                            variantId,

                        Quantity =
                            quantity,

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
                                "product.jpg",

                            DisplayOrder =
                                1,

                            IsPrimary =
                                true
                        }
                    }
            };
        }
    }
}