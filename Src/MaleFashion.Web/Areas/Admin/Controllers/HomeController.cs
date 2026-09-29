using Demo.Infrastructure.Identity;
using MaleFashion.Application.Contracts.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        // [HttpPost, ValidateAntiForgeryToken]
        // public async Task<IActionResult> Logout()
        // {


        //     // This needs to be a redirect so that the browser performs a new
        //     // request and the identity for the user gets updated.
        //     return RedirectToAction(
        //"Index",
        //"Home",
        //new { area = "Customer" }
        // );

        // }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


    }
}
