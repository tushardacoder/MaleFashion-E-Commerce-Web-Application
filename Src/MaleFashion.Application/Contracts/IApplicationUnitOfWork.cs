using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public ISqlUtility SqlUtility { get; set; }

        public IContactUsRepository ContactUsRepository { get; }

        public IDiscountRepository DiscountRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IInventoryRepository InventoryRepository { get; }

        public IWishlistRepository WishlistRepository { get; }

        public ICartRepository CartRepository { get; }

        public IOrderRepository OrderRepository { get; }
    }
}
