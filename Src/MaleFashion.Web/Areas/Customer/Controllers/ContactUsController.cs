using Cortex.Mediator;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Application.Features.ContactMessages.Command;
using MaleFashion.Infrastructure.Extensions;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Areas.Customer.Models;
using MaleFashion.Web.Codes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion.Web.Areas.Customer.Controllers
{

    [Area("Customer")]

    
    public class ContactUsController : Controller
    {

        private readonly ILogger<ContactUsController> _logger;
        private readonly IMediator _mediator;
        private readonly IGoogleReCaptchaService _reCaptchaService;
        private readonly IConfiguration _configuration;


        public ContactUsController(IMediator mediator,
     IGoogleReCaptchaService reCaptchaService,
     IConfiguration configuration,
     ILogger<ContactUsController> logger)
        {

            _mediator = mediator;
            _reCaptchaService = reCaptchaService;
            _configuration = configuration;
            _logger = logger;
        }

       
        [HttpGet]
        public IActionResult Contact()
        {
            ViewBag.RecaptchaSiteKey =
                _configuration["GoogleReCaptcha:SiteKey"];

            return View(new ContactUsModel());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(
      ContactUsModel model,
      CancellationToken cancellationToken)
        {
            ViewBag.RecaptchaSiteKey =
                _configuration["GoogleReCaptcha:SiteKey"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isRecaptchaValid =
                await _reCaptchaService.VerifyAsync(
                    model.RecaptchaToken,
                    "contact");

            if (!isRecaptchaValid)
            {
                ModelState.AddModelError(
                    "RecaptchaToken",
                    "reCAPTCHA verification failed.");

                return View(model);
            }

            try
            {
                var command = new ContactUsAddCommand
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message
                };

                var result =
                    await _mediator.SendCommandAsync(command);

                if (result is null)
                {
                    TempData.Put(
                        Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = "Failed to send your message.",
                            Type = ResponseTypes.Danger
                        });

                    return View(model);
                }

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Your message has been sent successfully.",
                        Type = ResponseTypes.Success
                    });

                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create Contact Us message.");

                TempData.Put(
                    Constants.ResponseTempKey,
                    new ResponseModel
                    {
                        Message = "Failed to send your message.",
                        Type = ResponseTypes.Danger
                    });

                return View(model);
            }
        }
    }
}
