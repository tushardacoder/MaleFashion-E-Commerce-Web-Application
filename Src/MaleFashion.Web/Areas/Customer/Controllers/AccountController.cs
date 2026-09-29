using Demo.Infrastructure.Identity;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Web.Models.Account;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion.Web.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize(Roles = "Member")]
    public class AccountController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
       

      
        private readonly IMapper _mapper;

        private readonly IGoogleReCaptchaService _reCaptchaService;



        public AccountController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
          
            IMapper mapper,
            IGoogleReCaptchaService reCaptchaService)
        {
            _userManager = userManager;
            _userStore = userStore;
           
            _signInManager = signInManager;
           
            _mapper = mapper;
            _reCaptchaService = reCaptchaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
    ChangePasswordModel model,
    string recaptchaToken)
        {
            // ------------------------------------------
            // Verify Google reCAPTCHA
            // ------------------------------------------

            var isCaptchaValid =
                await _reCaptchaService.VerifyAsync(
                    recaptchaToken,
                    "change_password");

            if (!isCaptchaValid)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed. Please try again.");

                return View(model);
            }


            // ------------------------------------------
            // Validate Model
            // ------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // ------------------------------------------
            // Get Currently Logged-in User
            // ------------------------------------------

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                await _signInManager.SignOutAsync();

                return RedirectToAction(
         "Index",
         "Home",
         new { area = " " });
            }


            // ------------------------------------------
            // VERIFY CURRENT / OLD PASSWORD
            // ------------------------------------------

            var isOldPasswordCorrect =
                await _userManager.CheckPasswordAsync(
                    user,
                    model.CurrentPassword);

            if (!isOldPasswordCorrect)
            {
                ModelState.AddModelError(
                    nameof(model.CurrentPassword),
                    "Your current password is incorrect.");

                return View(model);
            }


            // ------------------------------------------
            // Prevent using the same password
            // ------------------------------------------

            if (model.CurrentPassword == model.NewPassword)
            {
                ModelState.AddModelError(
                    nameof(model.NewPassword),
                    "Your new password cannot be the same as your current password.");

                return View(model);
            }


            // ------------------------------------------
            // Change Password
            // ------------------------------------------

            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);


            // ------------------------------------------
            // Success
            // ------------------------------------------

            if (result.Succeeded)
            {
              


                // Refresh authentication cookie
                await _signInManager.RefreshSignInAsync(user);


                TempData["SuccessMessage"] =
                    "Your password has been changed successfully.";


                return RedirectToAction(
                    nameof(ChangePassword));
            }


            // ------------------------------------------
            // Identity Errors
            // ------------------------------------------

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }


            return View(model);
        }

    }
}
