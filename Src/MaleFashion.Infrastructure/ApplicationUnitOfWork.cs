using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Entities;
using MaleFashion.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {

        public ISqlUtility SqlUtility { get; set; }

        public IContactUsRepository ContactUsRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public IDiscountRepository DiscountRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public IInventoryRepository InventoryRepository { get; private set; }

        public IWishlistRepository WishlistRepository { get; private set; }

        public ICartRepository CartRepository { get; private set;  }

        public IOrderRepository OrderRepository { get; private set; }

        public ITransactionManager Transaction { get; private set; }
        public ApplicationUnitOfWork(ApplicationDbContext dbContext,IContactUsRepository contactUsRepository, ICategoryRepository categoryRepository, IDiscountRepository discountRepository, IProductRepository productRepository, IInventoryRepository inventoryRepository, IWishlistRepository wishlistRepository, ICartRepository cartRepository,IOrderRepository orderrepository, ITransactionManager transaction)
            : base(dbContext)
        {
             ContactUsRepository = contactUsRepository;
             CategoryRepository= categoryRepository;
            DiscountRepository = discountRepository;
            ProductRepository = productRepository;
            InventoryRepository = inventoryRepository;
            WishlistRepository = wishlistRepository;
            CartRepository = cartRepository;
            OrderRepository = orderrepository;
            Transaction= transaction;
        }

       
    }
}
