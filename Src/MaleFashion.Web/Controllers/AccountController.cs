using Demo.Infrastructure.Identity;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Web.Models.Account;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace MaleFashion.Web.Controllers
{

    
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        private readonly IGoogleReCaptchaService _reCaptchaService;



        public AccountController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailService emailService,
            IMapper mapper,
            IGoogleReCaptchaService reCaptchaService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _mapper = mapper;
            _reCaptchaService = reCaptchaService;
        }


        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Register(RegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // No database for now
        //    return RedirectToAction("Index");
        //}


        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var model = new RegisterModel();
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, string recaptchaToken)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // ==========================================
            // RECAPTCHA: Verify before registration
            // ==========================================

            var isCaptchaValid =
                await _reCaptchaService.VerifyAsync(
                    recaptchaToken,
                    "signup");

            if (!isCaptchaValid)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed. Please try again.");
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateOfBirth = model.DateOfBirth;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");

                    var age = DateTime.Now.Year - model.DateOfBirth.Year;
                    await _userManager.AddClaimAsync(user, new Claim("age", age.ToString()));

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        values: new { area = "", userId = userId, code = code, returnUrl = model.ReturnUrl },
                        protocol: Request.Scheme)!;
                    var fullName = $"{user.FirstName} {user.LastName}";

                    var emailBody = $"""
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirm Your Email</title>
</head>

<body style="
    margin:0;
    padding:0;
    background-color:#f5f5f9;
    font-family:Arial, Helvetica, sans-serif;
">

    <table width="100%"
           cellpadding="0"
           cellspacing="0"
           style="padding:40px 15px;background-color:#f5f5f9;">

        <tr>
            <td align="center">

                <table width="600"
                       cellpadding="0"
                       cellspacing="0"
                       style="
                           max-width:600px;
                           width:100%;
                           background:#ffffff;
                           border-radius:12px;
                           overflow:hidden;
                       ">

                    <!-- Header -->

                    <tr>
                        <td style="
                            background:#696cff;
                            padding:30px;
                            text-align:center;
                        ">

                            <h1 style="
                                margin:0;
                                color:#ffffff;
                                font-size:28px;
                            ">
                                MaleFashion
                            </h1>

                            <p style="
                                margin:8px 0 0;
                                color:#eeeeff;
                                font-size:14px;
                            ">
                                Welcome to our platform
                            </p>

                        </td>
                    </tr>

                    <!-- Content -->

                    <tr>
                        <td style="padding:40px;">

                            <h2 style="
                                margin:0 0 20px;
                                color:#333333;
                            ">
                                Confirm your email
                            </h2>

                            <p style="
                                color:#555555;
                                font-size:16px;
                                line-height:1.6;
                            ">
                                Hello {System.Net.WebUtility.HtmlEncode(fullName)},
                            </p>

                            <p style="
                                color:#555555;
                                font-size:15px;
                                line-height:1.6;
                            ">
                                Thank you for creating an account with
                                <strong>MaleFashion</strong>.
                            </p>

                            <p style="
                                color:#555555;
                                font-size:15px;
                                line-height:1.6;
                            ">
                                Please confirm your email address by
                                clicking the button below.
                            </p>

                            <!-- Button -->

                            <table cellpadding="0"
                                   cellspacing="0"
                                   style="margin:30px auto;">

                                <tr>
                                    <td style="
                                        background:#696cff;
                                        border-radius:8px;
                                    ">

                                        <a href="{HtmlEncoder.Default.Encode(callbackUrl)}"
                                           style="
                                               display:inline-block;
                                               padding:14px 30px;
                                               color:#ffffff;
                                               text-decoration:none;
                                               font-size:16px;
                                               font-weight:bold;
                                           ">
                                            Confirm Email
                                        </a>

                                    </td>
                                </tr>

                            </table>

                            <p style="
                                color:#777777;
                                font-size:13px;
                                line-height:1.6;
                            ">
                                If the button doesn't work, copy and paste
                                the following link into your browser:
                            </p>

                            <p style="
                                word-break:break-all;
                                font-size:12px;
                            ">
                                <a href="{HtmlEncoder.Default.Encode(callbackUrl)}"
                                   style="color:#696cff;">
                                    {HtmlEncoder.Default.Encode(callbackUrl)}
                                </a>
                            </p>

                        </td>
                    </tr>

                    <!-- Footer -->

                    <tr>
                        <td style="
                            padding:25px;
                            text-align:center;
                            background:#f8f9fa;
                            border-top:1px solid #eeeeee;
                        ">

                            <p style="
                                margin:0 0 8px;
                                color:#777777;
                                font-size:13px;
                            ">
                                © 2026 MaleFashion. All rights reserved.
                            </p>

                            <p style="
                                margin:0;
                                color:#999999;
                                font-size:12px;
                            ">
                                This is an automated email.
                                Please do not reply.
                            </p>

                        </td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>

</body>
</html>
""";

                    await _emailService.SendEmailAsync(
                        fullName,
                        model.Email,
                        "Confirm your MaleFashion account",
                        emailBody);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(model.ReturnUrl);
                        return RedirectToAction(nameof(RegisterConfirmation), new
                        {
                            email = model.Email
                        });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(
      string email,
      string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(
                    $"Unable to load user with email '{email}'.");
            }

            var model = new RegisterConfirmationViewModel
            {
                Email = email,
                DisplayConfirmAccountLink = true
            };

            if (model.DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);

                var code =
                    await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(code));

                model.EmailConfirmationUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new
                    {
                        userId = userId,
                        code = code,
                        returnUrl = returnUrl
                    },
                    protocol: Request.Scheme);
            }

            return View(model);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(
            string userId,
            string code,
            string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing.");
            }

            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Confirmation code is missing.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            try
            {
                code = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(code));
            }
            catch
            {
                return BadRequest("Invalid confirmation code.");
            }

            var result =
                await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                //return RedirectToAction(nameof(EmailConfirmed));
                return View();
            }

            return BadRequest(
                string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description)));
        }


        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult EmailConfirmed()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var model = new LoginModel();

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string recaptchaToken)
        {
            model.ReturnUrl ??= Url.Content("~/");

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var isCaptchaValid =
        await _reCaptchaService.VerifyAsync(
            recaptchaToken,
            "login");

            if (!isCaptchaValid)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed.");

                return View(model);
            }


            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
               // var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(
       "User {Email} logged in.",
       model.Email);

                    var user = await _userManager.FindByEmailAsync(
                        model.Email);

                    if (user == null)
                    {
                        await _signInManager.SignOutAsync();

                        ModelState.AddModelError(
                            string.Empty,
                            "Unable to find user.");

                        return View(model);
                    }

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction(
                            "Index",
                            "Home",
                            new
                            {
                                area = "Admin"
                            });
                    }

                    if (await _userManager.IsInRoleAsync(user, "Member"))
                    {
                        return RedirectToAction(
                            "Index",
                            "Home",
                            new
                            {
                                area = "Customer"
                            });
                    }

                    return LocalRedirect(model.ReturnUrl);

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", "Account", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction("AccessDenied", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}


        //[HttpGet]
        //public IActionResult Login(string? returnUrl = null)
        //{
        //    var model = new LoginModel
        //    {
        //        ReturnUrl = returnUrl
        //    };

        //    return View(model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
          
          
        //        // This needs to be a redirect so that the browser performs a new
        //        // request and the identity for the user gets updated.
        //        return RedirectToAction(
        //   "Index",
        //   "Home",
        //   new { area = "Customer" }
        //    );
            
        //}

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        // ============================
        // POST: Forgot Password
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordModel model,
            string recaptchaToken)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (string.IsNullOrWhiteSpace(recaptchaToken))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Please complete the security verification.");

                return View(model);
            }


        
            var isCaptchaValid =
                await _reCaptchaService.VerifyAsync(
                    recaptchaToken,
                    "forgot_password");

            if (!isCaptchaValid)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed. Please try again.");

                return View(model);
            }


          
            var user =
                await _userManager.FindByEmailAsync(model.Email);


          
            if (user == null)
            {
                return RedirectToAction(
                    nameof(ForgotPasswordConfirmation));
            }


         
            var code =
                await _userManager
                    .GeneratePasswordResetTokenAsync(user);


            
            code = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(code));


            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new
                {
                    email = model.Email,
                    code = code
                },
                protocol: Request.Scheme);


            if (string.IsNullOrEmpty(callbackUrl))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to generate the password reset link.");

                return View(model);
            }


            // 8. Create email body
            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>

<body style='
    font-family: Arial, sans-serif;
    background-color: #f5f5f5;
    padding: 30px;
'>

    <div style='
        max-width: 600px;
        margin: auto;
        background: #ffffff;
        padding: 30px;
        border-radius: 8px;
    '>

        <h2 style='color: #111111;'>
            Reset Your Password
        </h2>

        <p>
            Hello {HtmlEncoder.Default.Encode(user.UserName ?? "User")},
        </p>

        <p>
            We received a request to reset your Male Fashion account password.
        </p>

        <p>
            Click the button below to create a new password.
        </p>

        <p style='margin: 30px 0;'>

            <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'
               style='
                    display: inline-block;
                    padding: 12px 25px;
                    background-color: #111111;
                    color: #ffffff;
                    text-decoration: none;
                    font-weight: bold;
                    border-radius: 4px;
               '>

                Reset Password

            </a>

        </p>

        <p>
            If you did not request a password reset,
            you can safely ignore this email.
        </p>

        <hr style='border: none; border-top: 1px solid #eeeeee;'>

        <p style='color: #777777;'>
            Thanks,<br />
            <strong>Male Fashion Team</strong>
        </p>

    </div>

</body>
</html>
";


            try
            {
                await _emailService.SendEmailAsync(
                     user.UserName ?? "User",
                    model.Email,
                    "Reset Your Male Fashion Password",
                    emailBody);
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to send the password reset email. Please try again later.");

                return View(model);
            }


            
            return RedirectToAction(
                nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult ResetPassword()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ResetPassword(
    string code,
    string email)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Invalid password reset token.");
            }

            var model = new ResetPasswordModel
            {
                Code = code,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
    ResetPasswordModel model,
    string recaptchaToken)
        {
         
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (string.IsNullOrWhiteSpace(recaptchaToken))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed. Please try again.");

                return View(model);
            }


            var isCaptchaValid =
                await _reCaptchaService.VerifyAsync(
                    recaptchaToken,
                    "reset_password");


            if (!isCaptchaValid)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Security verification failed. Please try again.");

                return View(model);
            }


         
            var user =
                await _userManager.FindByEmailAsync(model.Email);


            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid password reset request.");

                return View(model);
            }


            string decodedCode;

            try
            {
                decodedCode =
                    Encoding.UTF8.GetString(
                        WebEncoders.Base64UrlDecode(model.Code));
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid or expired password reset link.");

                return View(model);
            }


            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    decodedCode,
                    model.Password);


            
            if (result.Succeeded)
            {
                return RedirectToAction(
                    nameof(Login));
            }


           
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }


            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToAction("Index", "Home");
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

    }
}
