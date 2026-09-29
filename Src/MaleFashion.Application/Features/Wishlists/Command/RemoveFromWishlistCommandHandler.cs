using Cortex.Mediator.Commands;
using MaleFashion.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Command
{
    public class RemoveFromWishlistCommandHandler
        : ICommandHandler<
            RemoveFromWishlistCommand,
            bool>
    {


        private readonly IApplicationUnitOfWork _unitOfWork;

        public RemoveFromWishlistCommandHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            RemoveFromWishlistCommand command,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // VALIDATION
            // ==========================================

            if (command.UserId == Guid.Empty)
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            if (command.WishlistItemId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Wishlist item ID is required.");
            }


            // ==========================================
            // GET WISHLIST ITEM
            // AND VERIFY USER OWNERSHIP
            // ==========================================

            var item =
                await _unitOfWork
                    .WishlistRepository
                    .GetItemByIdForUserAsync(
                        command.WishlistItemId,
                        command.UserId,
                        cancellationToken);


            // ==========================================
            // ITEM NOT FOUND
            // ==========================================

            if (item == null)
            {
                return false;
            }


            // ==========================================
            // REMOVE ITEM
            // ==========================================

            await _unitOfWork
                .WishlistRepository
                .RemoveItemAsync(
                    item,
                    cancellationToken);


            // ==========================================
            // SAVE
            // ==========================================

            await _unitOfWork
                .SaveAsync(
                    cancellationToken);


            return true;
        }

    }

}
