using Demo.Infrastructure.Identity;
using MaleFashion.Infrastructure.Extensions;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Codes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        // ==========================================
        // USERS PAGE
        // ==========================================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


      
        // ==========================================
        // GET PAGED USERS
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPagedUsers(
            [FromBody] UserQueryModel model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    message = "Invalid request."
                });
            }


            // ==========================================
            // BASE QUERY
            // ==========================================

            var query = _userManager.Users
                .AsNoTracking()
                .AsQueryable();


            // ==========================================
            // TOTAL USERS
            // ==========================================

            var recordsTotal = await query.CountAsync();


            // ==========================================
            // SINGLE SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(model.SearchText))
            {
                var searchText = model.SearchText.Trim();

                query = query.Where(x =>

                    // First Name
                    (x.FirstName != null &&
                     x.FirstName.Contains(searchText))

                    ||

                    // Last Name
                    (x.LastName != null &&
                     x.LastName.Contains(searchText))

                    ||

                    // Email
                    (x.Email != null &&
                     x.Email.Contains(searchText))
                );
            }


            // ==========================================
            // FILTERED USERS
            // ==========================================

            var recordsFiltered = await query.CountAsync();


            // ==========================================
            // ORDERING
            // ==========================================

            query = query
                .OrderByDescending(x => x.Id);


            // ==========================================
            // PAGING
            // ==========================================

            if (model.Length > 0)
            {
                query = query
                    .Skip(model.Start)
                    .Take(model.Length);
            }


            // ==========================================
            // SELECT USERS
            // ==========================================

            var users = await query
                .Select(x => new
                {
                    id = x.Id,

                    firstName = x.FirstName,

                    lastName = x.LastName,

                    email = x.Email,

                    phoneNumber = x.PhoneNumber,

                    emailConfirmed = x.EmailConfirmed,

                    lockoutEnabled = x.LockoutEnabled,

                    lockoutEnd = x.LockoutEnd
                })
                .ToListAsync();


            // ==========================================
            // BLOCK STATUS
            // ==========================================

            var now = DateTimeOffset.UtcNow;

            var data = users.Select(x => new
            {
                id = x.id,

                firstName = x.firstName,

                lastName = x.lastName,

                email = x.email,

                phoneNumber = x.phoneNumber,

                emailConfirmed = x.emailConfirmed,

                isBlocked =
                    x.lockoutEnabled &&
                    x.lockoutEnd.HasValue &&
                    x.lockoutEnd.Value > now
            });


            // ==========================================
            // DATATABLE RESPONSE
            // ==========================================

            return Json(new
            {
                recordsTotal = recordsTotal,

                recordsFiltered = recordsFiltered,

                data = data
            });
        }


        // ==========================================
        // BLOCK USER
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            var user = await _userManager
                .FindByIdAsync(id.ToString());


            // ==========================================
            // USER NOT FOUND
            // ==========================================

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            // ==========================================
            // PREVENT ADMIN BLOCKING HIMSELF
            // ==========================================

            var currentUserId =
                _userManager.GetUserId(User);

            if (user.Id.ToString() == currentUserId)
            {
                return BadRequest(new
                {
                    message = "You cannot block your own account."
                });
            }


            // ==========================================
            // ENABLE LOCKOUT
            // ==========================================

            var enableResult =
                await _userManager.SetLockoutEnabledAsync(
                    user,
                    true);


            if (!enableResult.Succeeded)
            {
                return BadRequest(new
                {
                    message = string.Join(
                        ", ",
                        enableResult.Errors.Select(
                            x => x.Description))
                });
            }


            // ==========================================
            // SET LOCKOUT END
            // ==========================================

            var result =
                await _userManager.SetLockoutEndDateAsync(
                    user,
                    DateTimeOffset.UtcNow.AddYears(100));


            // ==========================================
            // CHECK RESULT
            // ==========================================

            if (!result.Succeeded)
            {
                //return BadRequest(new
                //{
                //    message = string.Join(
                //        ", ",
                //        result.Errors.Select(
                //            x => x.Description))
                //});
                TempData.Put(Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = string.Join(
                        ", ",
                        result.Errors.Select(
                            x => x.Description)),
                            Type = ResponseTypes.Danger
                        });
            }


           

            TempData.Put(Constants.ResponseTempKey,
                  new ResponseModel
                  {
                      Message = "User blocked successfully.",
                      Type = ResponseTypes.Success
                  });

            

            return Ok();

        }


        // ==========================================
        // UNBLOCK USER
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            var user = await _userManager
                .FindByIdAsync(id.ToString());


            // ==========================================
            // USER NOT FOUND
            // ==========================================

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }


            // ==========================================
            // REMOVE LOCKOUT END
            // ==========================================

            var result =
                await _userManager.SetLockoutEndDateAsync(
                    user,
                    null);


            // ==========================================
            // CHECK RESULT
            // ==========================================

            if (!result.Succeeded)
            {
                TempData.Put(Constants.ResponseTempKey,
                        new ResponseModel
                        {
                            Message = string.Join(
                        ", ",
                        result.Errors.Select(
                            x => x.Description)),
                            Type = ResponseTypes.Danger
                        });
            }


            // ==========================================
            // SUCCESS
            // ==========================================

           
            TempData.Put(Constants.ResponseTempKey,
                new ResponseModel
                {
                    Message = "User Unblocked successfully.",
                    Type = ResponseTypes.Success
                });

            return Ok();
        }
    }
 }
