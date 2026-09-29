using Demo.Infrastructure.Identity;
using MaleFashion.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MaleFashion.Web.ViewComponents
{
    
        public class WishlistCountViewComponent : ViewComponent
        {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistCountViewComponent(
            IWishlistRepository wishlistRepository,
            UserManager<ApplicationUser> userManager)
        {
            _wishlistRepository = wishlistRepository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(0);
            }

            var userId = user.Id;

            var count = await _wishlistRepository
                .GetWishlistItemCountAsync(userId);

            return View(count);
        }
    }
    }

