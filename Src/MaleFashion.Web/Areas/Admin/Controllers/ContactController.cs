using Cortex.Mediator;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Application.Features.ContactMessages.Query;
using MaleFashion.Domain.Entities;
using MaleFashion.Domain.Utilities;
using MaleFashion.Web.Areas.Admin.Models;
using MaleFashion.Web.Areas.Customer.Controllers;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace MaleFashion.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {

        private readonly ILogger<ContactController> _logger;
        private readonly IMediator _mediator;
        private readonly IGoogleReCaptchaService _reCaptchaService;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        public ContactController(IMediator mediator,
            IMapper mapper,
    IGoogleReCaptchaService reCaptchaService,
    IConfiguration configuration,
    ILogger<ContactController> logger)
        {

            _mediator = mediator;
            _mapper = mapper;
            _reCaptchaService = reCaptchaService;
            _configuration = configuration;
            _logger = logger;
        }


        // GET: /Admin/Contact
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPagedContacts(
    [FromBody] ContactUsQueryModel model)
        {
            try
            {
                // ==========================================
                // MAP DATATABLE REQUEST
                // ==========================================

                var query =
                    _mapper.Map<
                        GetAllContactUsByPagingQuery>(
                            model);


                // ==========================================
                // SEARCH
                // ==========================================

                query.SearchText =
                    model.Search.Value;


                // ==========================================
                // SORTING
                // ==========================================

                query.SortText =
                    model.FormatSortExpression(
                        "Name",
                        "Email",
                        "Message",
                        "CreatedAt");


                // ==========================================
                // SEND QUERY
                // ==========================================

                var (items, total, totalDisplay) =
                    await _mediator.SendQueryAsync<
                        GetAllContactUsByPagingQuery,
                        (IList<ContactUs>, int, int)>(
                            query);


                // ==========================================
                // DATATABLES RESPONSE
                // ==========================================

                var contacts = new
                {
                    recordsTotal = total,

                    recordsFiltered = totalDisplay,

                    data = items
                        .Select(item => new string[]
                        {
                            HttpUtility.HtmlEncode(
                                item.Name),

                            HttpUtility.HtmlEncode(
                                item.Email),

                            HttpUtility.HtmlEncode(
                                item.Message),

                            HttpUtility.HtmlEncode(
                                item.CreatedAt
                                    .AddHours(6)
                                    .ToString(
                                        "dd-MM-yyyy hh:mm tt")),

                            item.Id.ToString()

                        })
                        .ToArray()
                };


                return Json(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get contact list");


                return Json(
                    DataTables.EmptyResult);
            }
        
    
    }
    }
}
