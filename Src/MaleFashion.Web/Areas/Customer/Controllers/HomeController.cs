using Cortex.Mediator;
using Demo.Infrastructure.Identity;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Application.Features.Carts.Command;
using MaleFashion.Application.Features.Carts.Query;
using MaleFashion.Application.Features.ContactMessages.Command;
using MaleFashion.Application.Features.Discounts.Query;
using MaleFashion.Application.Features.Orders.Command;
using MaleFashion.Application.Features.Orders.Query;
using MaleFashion.Application.Features.Products.Query;
using MaleFashion.Application.Features.Wishlists.Command;
using MaleFashion.Application.Features.Wishlists.Query;
using MaleFashion.Domain.Utilities;
using MaleFashion.Infrastructure.Extensions;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Areas.Customer.Models;
using MaleFashion.Web.Codes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace MaleFashion.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Member")]
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IGoogleReCaptchaService _reCaptchaService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
     IMediator mediator,
     IGoogleReCaptchaService reCaptchaService,
     IConfiguration configuration,
     ILogger<HomeController> logger,UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _reCaptchaService = reCaptchaService;
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }





        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Blogdetails()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public async Task<IActionResult> Shop(
             string? search,
             Guid? categoryId,
             string? brand,
             string? size,
             string? color,
             string? tag,
             decimal? minPrice,
             decimal? maxPrice,
             string? sort,
             int page = 1,
             CancellationToken cancellationToken = default)
        {
            var query = new GetShopProductsQuery
            {
                Search = search,

                CategoryId = categoryId,

                Brand = brand,

                Size = size,

                Color = color,

                Tag = tag,

                MinPrice = minPrice,

                MaxPrice = maxPrice,

                Sort = sort,

                Page = page,

                PageSize = 12
            };

            var model =
           await _mediator.QueryAsync(
               query,
               cancellationToken);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ShopDetails(
    Guid id,
    CancellationToken cancellationToken)
        {
            var query =
                new GetShopProductDetailsQuery(id);

            var model =
                await _mediator.QueryAsync(
                    query,
                    cancellationToken);

            return View(model);
        }

       

        public IActionResult FAQ()
        {
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddToWishlist(
            Guid productId,
            Guid productVariantId)
        {
            // ==========================================
            // AUTHENTICATION
            // ==========================================

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = "",
                        returnUrl = Url.Action(
                            "ShopDetails",
                            "Home",
                            new
                            {
                                area = "Customer",
                                productId = productId
                            })
                    });
            }


            // ==========================================
            // GET USER ID
            // ==========================================

            if (!TryGetUserId(out Guid userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // ==========================================
            // VALIDATE VARIANT
            // ==========================================

            if (productVariantId == Guid.Empty)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Please select a valid color and size."
                    });

                return RedirectToAction(
                    "ShopDetails",
                    new
                    {
                        area = "Customer",
                        productId = productId
                    });
            }


            // ==========================================
            // SEND COMMAND
            // ==========================================

            var result =
                await _mediator.SendAsync(
                    new AddToWishlistCommand
                    {
                        UserId = userId,
                        ProductVariantId = productVariantId
                    },
                    HttpContext.RequestAborted);


            // ==========================================
            // RESPONSE
            // ==========================================

            TempData.Put(
                Constants.ResponseTempKey,
                new ResponseModel
                {
                    Type = result
                        ? ResponseTypes.Success
                        : ResponseTypes.Warning,

                    Message = result
                        ? "Product added to wishlist."
                        : "This product variant is already in your wishlist."
                });


            // ==========================================
            // REDIRECT
            // ==========================================
            return RedirectToAction(
                 "Wishlist",
                "Home",
               new { area = "Customer" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> RemoveFromWishlist(
    Guid wishlistItemId)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message = "Please login first."
                    });

                return Json(new
                {
                    success = false,
                    message = "Please login first."
                });
            }

            if (!TryGetUserId(out Guid userId))
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message = "User authentication failed."
                    });

                return Json(new
                {
                    success = false,
                    message = "User authentication failed."
                });
            }

            if (wishlistItemId == Guid.Empty)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message = "Invalid wishlist item."
                    });

                return Json(new
                {
                    success = false,
                    message = "Invalid wishlist item."
                });
            }

            var result =
                await _mediator.SendAsync(
                    new RemoveFromWishlistCommand
                    {
                        UserId = userId,
                        WishlistItemId = wishlistItemId
                    },
                    HttpContext.RequestAborted);

            if (!result)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Warning,
                        Message = "Wishlist item was not found."
                    });

                return Json(new
                {
                    success = false,
                    message = "Wishlist item was not found."
                });
            }

            TempData.Put(
                Constants.ResponseTempKey,
                new ResponseModel
                {
                    Type = ResponseTypes.Success,
                    Message = "Product removed from favourites."
                });

            return Json(new
            {
                success = true,
                message = "Product removed from favourites."
            });
        }



        [HttpGet]
        public async Task<IActionResult> Wishlist()
        {
            // ==========================================
            // AUTHENTICATION
            // ==========================================

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // ==========================================
            // GET USER ID
            // ==========================================

            if (!TryGetUserId(
                    out Guid userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // ==========================================
            // GET WISHLIST
            // ==========================================

            var model =
                await _mediator.QueryAsync(
                    new GetWishlistQuery(userId),
                    HttpContext.RequestAborted);


            // ==========================================
            // RETURN VIEW
            // ==========================================

            return View(model);
        }


        //[HttpGet]
        //public IActionResult Shoppingcart()
        //{
        //    return View(new CartViewModel());
        //}

       // [HttpGet]
       // public IActionResult Shoppingcart(
       //Guid productId,
       //string productName,
       //decimal price,
       //int quantity = 1)
       // {
       //     if (productId == Guid.Empty)
       //     {
       //         return View(new CartViewModel());
       //     }

       //     var item = new CartItemViewModel
       //     {
       //         Id = Guid.NewGuid(),

       //         ProductId = productId,

       //         ProductName = productName ?? "",

       //         Quantity = quantity,

       //         UnitPrice = price
       //     };

       //     var model = new CartViewModel
       //     {
       //         Id = Guid.NewGuid(),

       //         Items = new List<CartItemViewModel>
       // {
       //     item
       // },

       //         Discount = 0
       //     };

       //     return View(model);
       // }

       
public async Task<IActionResult> Shoppingcart(
    CancellationToken cancellationToken)
        {
            var userIdString =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)
                ?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new { area = "" });
            }


            var query = new GetCartQuery
            {
                UserId = userId
            };


            var model =
                await _mediator.QueryAsync(
                    query,
                    cancellationToken);


            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(
            Guid productId,
            Guid productVariantId,
            int quantity,
            CancellationToken cancellationToken)
        {
            // =========================================
            // 1. CHECK LOGIN
            // =========================================

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        returnUrl = Url.Action(
                            "ShopDetails",
                            "Home",
                            new
                            {
                                area = "Customer",
                                id = productId
                            })
                    });
            }


            // =========================================
            // 2. GET USER ID FROM IDENTITY
            // =========================================

            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(
                userIdString,
                out var userId))
            {
                return Unauthorized();
            }


            // =========================================
            // 3. VALIDATE PRODUCT ID
            // =========================================

            if (productId == Guid.Empty)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Invalid product."
                    });

                return RedirectToAction(
                    nameof(ShopDetails),
                    new
                    {
                        id = productId
                    });
            }


            // =========================================
            // 4. VALIDATE PRODUCT VARIANT
            // =========================================

            if (productVariantId == Guid.Empty)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Please select a valid color and size."
                    });

                return RedirectToAction(
                    nameof(ShopDetails),
                    new
                    {
                        id = productId
                    });
            }


            // =========================================
            // 5. VALIDATE QUANTITY
            // =========================================

            if (quantity <= 0)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Please select a valid quantity."
                    });

                return RedirectToAction(
                    nameof(ShopDetails),
                    new
                    {
                        id = productId
                    });
            }


            // =========================================
            // 6. CREATE COMMAND
            // =========================================

            var command = new AddToCartCommand
            {
                UserId = userId,

                ProductId = productId,

                ProductVariantId =
                    productVariantId,

                Quantity = quantity
            };


            // =========================================
            // 7. SEND COMMAND
            // =========================================

            try
            {
                var result =
                    await _mediator.SendAsync(
                        command,
                        cancellationToken);


                // =========================================
                // 8. SUCCESS MESSAGE
                // =========================================

                if (result)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Type = ResponseTypes.Success,
                            Message =
                                "Product added to cart successfully."
                        });
                }
                else
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Type = ResponseTypes.Danger,
                            Message =
                                "Unable to add product to cart."
                        });
                }
            }
            catch (Exception ex)
            {
                // =========================================
                // 9. ERROR MESSAGE
                // =========================================

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message = ex.Message
                    });

                return RedirectToAction(
                    nameof(ShopDetails),
                    new
                    {
                        id = productId
                    });
            }


            // =========================================
            // 10. REDIRECT TO SHOPPING CART
            // =========================================

            return RedirectToAction(
                nameof(Shoppingcart));
        }


        public async Task<IActionResult> UpdateCartQuantity(
            Guid productVariantId,
            int quantity,
            CancellationToken cancellationToken)
        {
            var userIdString =
      User.FindFirst(
          System.Security.Claims.ClaimTypes.NameIdentifier)
      ?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to update your cart."
                });
            }

            if (quantity < 1)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Quantity must be at least 1."
                    });

                return Json(new
                {
                    success = false,
                    message = "Quantity must be at least 1."
                });
            }

            var command = new UpdateCartQuantityCommand
            {
                UserId = userId,

                ProductVariantId =
                    productVariantId,

                Quantity =
                    quantity
            };

            var result =
                await _mediator.SendAsync(
                    command,
                    cancellationToken);

            if (!result)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Unable to update cart quantity."
                    });

                return Json(new
                {
                    success = false,
                    message = "Unable to update cart quantity."
                });
            }

            return Json(new
            {
                success = true,
                message = "Cart quantity updated successfully."
            });
        }



        public async Task<IActionResult> RemoveFromCart(
    Guid productVariantId,
    CancellationToken cancellationToken)
        {
            var userIdString =
     User.FindFirst(
         System.Security.Claims.ClaimTypes.NameIdentifier)
     ?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to remove this item."
                });
            }

            var command = new RemoveFromCartCommand
            {
                UserId = userId,

                ProductVariantId =
                    productVariantId
            };

            var result =
                await _mediator.SendAsync(
                    command,
                    cancellationToken);

            if (!result)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "Unable to remove item from cart."
                    });

                return Json(new
                {
                    success = false,
                    message = "Unable to remove item from cart."
                });
            }

            return Json(new
            {
                success = true,
                message = "Item removed from cart successfully."
            });
        }



        [HttpGet]
        public async Task<IActionResult> Checkout(
       CancellationToken cancellationToken)
        {
            // Get logged-in user's ID from Claims
            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            // User is not logged in
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }

          

            var query = new GetCartQuery
            {
                UserId = userId,
             

            };

            var activeDiscount =
    await _mediator.SendQueryAsync(
        new GetActiveDiscountQuery(),
        cancellationToken);

            

            // Get cart from database
            var cart = await _mediator.QueryAsync(
                query,
                cancellationToken);

            if (activeDiscount == null)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type = ResponseTypes.Danger,
                        Message =
                            "GetActiveDiscountQuery returned NULL."
                    });

               // Console.WriteLine("null");
            }
            else
            {
                
               // Console.WriteLine(activeDiscount.Code);
               // Console.WriteLine(activeDiscount.DiscountPercentage);
                cart.CouponCode = activeDiscount.Code;
                cart.Discount = activeDiscount.DiscountPercentage;
                cart.CouponName=activeDiscount.DiscountName;
            }
            //if (activeDiscount != null)
            //{
            //    cart.CouponCode = activeDiscount.Code;
            //    cart.Discount = activeDiscount.DiscountPercentage;
            //    cart.CouponName=activeDiscount.DiscountName;
                

           
            return View(cart);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(
    AddOrderCommand command,
    CancellationToken cancellationToken)
        {
            // ======================================================
            // 1. CHECK LOGIN
            // ======================================================

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // ======================================================
            // 2. GET USER ID FROM CLAIM
            // ======================================================

            if (!TryGetUserId(out var userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // ======================================================
            // 3. GET APPLICATION USER
            // ======================================================

            var user =
                await _userManager.FindByIdAsync(
                    userId.ToString());

            if (user == null)
            {
                return Unauthorized();
            }


            // ======================================================
            // 4. SET TRUSTED USER DATA
            // ======================================================

            command.UserId =
                userId;

            command.FirstName =
                user.FirstName;

            command.LastName =
                user.LastName;

            command.Email =
                user.Email ?? string.Empty;


            // ======================================================
            // 5. VALIDATE SHIPPING
            // ======================================================

            if (string.IsNullOrWhiteSpace(
                command.Phone))
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type =
                            ResponseTypes.Danger,

                        Message =
                            "Phone number is required."
                    });

                return RedirectToAction(
                    nameof(Checkout));
            }


            if (string.IsNullOrWhiteSpace(
                command.Address))
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type =
                            ResponseTypes.Danger,

                        Message =
                            "Address is required."
                    });

                return RedirectToAction(
                    nameof(Checkout));
            }


            if (string.IsNullOrWhiteSpace(
                command.TownCity))
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type =
                            ResponseTypes.Danger,

                        Message =
                            "Town / City is required."
                    });

                return RedirectToAction(
                    nameof(Checkout));
            }


            // ======================================================
            // 6. VALIDATE PAYMENT
            // ======================================================

            if (command.PaymentType ==
                PaymentType.Bkash)
            {
                if (string.IsNullOrWhiteSpace(
                    command.MobileNumber))
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Type =
                                ResponseTypes.Danger,

                            Message =
                                "bKash mobile number is required."
                        });

                    return RedirectToAction(
                        nameof(Checkout));
                }


                if (string.IsNullOrWhiteSpace(
                    command.TransactionId))
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Type =
                                ResponseTypes.Danger,

                            Message =
                                "bKash transaction ID is required."
                        });

                    return RedirectToAction(
                        nameof(Checkout));
                }
            }


            // ======================================================
            // 7. SEND COMMAND
            // ======================================================

            var orderId =
                await _mediator.SendAsync(
                    command,
                    cancellationToken);


            // ======================================================
            // 8. CHECK FAILURE
            // ======================================================

            if (orderId == Guid.Empty)
            {
                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Type =
                            ResponseTypes.Danger,

                        Message =
                            "Unable to place the order. " +
                            "Please check your cart and product stock."
                    });

                return RedirectToAction(
                    nameof(Checkout));
            }


            // ======================================================
            // 9. SUCCESS
            // ======================================================

            TempData.Put(
                Constants.ResponseTempKey,
                new ResponseModel
                {
                    Type =
                        ResponseTypes.Success,

                    Message =
                        "Your order has been placed successfully."
                });


            // ======================================================
            // 10. ORDER SUCCESS PAGE
            // ======================================================

            return RedirectToAction(
                nameof(OrderSuccess),
                new
                {
                    orderId
                });
        }


        public IActionResult OrderSuccess(Guid orderId)
        {
            ViewBag.OrderId = orderId;

            return View();
        }


        public async Task<IActionResult> OrderHistory(
    CancellationToken cancellationToken)
        {
            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);


            if (!Guid.TryParse(
                    userIdString,
                    out var userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            var result =
                await _mediator.QueryAsync(
                    new GetOrderHistoryQuery
                    {
                        UserId = userId
                    },
                    cancellationToken);


            return View(result);
        }


       
        public async Task<IActionResult> OrderDetails(
    Guid id,
    CancellationToken cancellationToken)
        {
            // =========================================================
            // 1. GET LOGGED-IN USER ID
            // =========================================================

            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);


            // =========================================================
            // 2. CHECK USER ID
            // =========================================================

            if (!Guid.TryParse(
                    userIdString,
                    out var userId))
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        area = ""
                    });
            }


            // =========================================================
            // 3. GET ORDER DETAILS
            // =========================================================

            try
            {
                var result =
                    await _mediator.QueryAsync(
                        new GetOrderDetailsQuery
                        {
                            OrderId = id,

                            UserId = userId
                        },
                        cancellationToken);


                return View(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        private bool TryGetUserId(
         out Guid userId)
        {
            userId = Guid.Empty;

            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            return Guid.TryParse(
                userIdString,
                out userId);
        }



    }
}
