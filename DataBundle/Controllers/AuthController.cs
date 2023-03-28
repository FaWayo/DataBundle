using DataBundle.BundleService;
using DataBundle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataBundle.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<TelegramBotController> _logger;
        private readonly ITelegramBotService _tbotService;
        public AuthController(ILogger<TelegramBotController> logger, ITelegramBotService tbotService, IOTPManager oTPManager)
        {
            _logger = logger;
            _tbotService = tbotService;
        }

        [Route("checkPin")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckPin([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                var response = await _tbotService.CheckPin(customerInfo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking pin error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not check the user's pin");
            }
        }


        [Route("getToken")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] User user)
        {
            try
            {
                var response = await _tbotService.getToken(user);
                if(response == "No user match found")
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Generating token error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not generate token");
            }
        }

        [Route("checkCustomer")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CustomerExists([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                var response = await _tbotService.CustomerExists(customerInfo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking pin error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not check if the Customer exists");
            }
        }

        [Route("checkUsername")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UsernameExists([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                var response = await _tbotService.UsernameExists(customerInfo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking pin error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not check if the Customer exists");
            }
        }


    }
}
