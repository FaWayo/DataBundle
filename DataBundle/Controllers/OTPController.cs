using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataBundle.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace DataBundle.Controllers
{
    [ApiController]
    [Route("otp")]
    public class OTPController : Controller
    {
        const string SessionOTPKey = "OTP";
       
        private readonly ILogger<TelegramBotController> _logger;
        private readonly IOTPManager _otpmanager;

        public OTPController(ILogger<TelegramBotController> logger, IOTPManager otpmanager)
        {
            _logger = logger;
            _otpmanager = otpmanager;
        }

        [Route("getOTP")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendOTP(CustomerInfo customer)
        {
            try
            {   
                Random random = new Random();
                string otpRandom = (random.Next(100000, 999999)).ToString();

                HttpContext.Session.SetString(SessionOTPKey, otpRandom);

                string message = $"An OTP has been sent to {customer.phoneNumber}";
              
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking pin error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not send the OTP");
            }
            
        }

        [Route("verifyOTP")]
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> VerifyOTP(OTPModel oTP)
        {
            try
            {
           
                var otpSession = HttpContext.Session.GetString(SessionOTPKey);
                Console.WriteLine( otpSession);
              
                if(oTP.OTPCode == null)
                {
                    return BadRequest("Enter OTP Code to match");
                }
                if(oTP.OTPCode == otpSession)
                {
                    return Ok("OTP was verified successfully");
                }
                else
                {
                     return BadRequest("OTP did not match");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking pin error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not verify the OTP");
            }

        }

    }
}
