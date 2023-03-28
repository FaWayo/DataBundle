using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataBundle.Models;
using DataBundle.BundleService;

namespace DataBundle.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ITelegramBotService _botService;

        public CustomerController(ILogger<CustomerController> logger, ITelegramBotService botService)
        {
            _logger = logger;
            _botService = botService;
        }

        [Route("add")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CustomerInfo customer)
        {
            try
            {
                var response = await _botService.AddCustomer(customer);
                if(response == "Customer was added successfully")
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Customer already exists");
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Adding customer error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not add a customer");
            }
        }
    }
}
