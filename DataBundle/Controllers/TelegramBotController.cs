using DataBundle.BundleService;
using Microsoft.AspNetCore.Mvc;
using DataBundle.Models;

namespace DataBundle.Controllers
{
    [ApiController]
    [Route("tbot")]
    public class TelegramBotController : Controller
    {
        private readonly ILogger<TelegramBotController> _logger;
        private readonly ITelegramBotService _tbotService;

        public TelegramBotController(ILogger<TelegramBotController> logger, ITelegramBotService bundleService)
        {
            _logger = logger;
            _tbotService = bundleService;
        }

        [Route("getDataBundles")]
        [HttpGet]
        public async Task<IActionResult> GetBundles()
        {
            try
            {
                var bundles = await _tbotService.GetBundles();
                if(bundles == null)
                {
                    return BadRequest("No bundles found");
                }
                return Ok(bundles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetBundles error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not get bundles");
            }
        }

        [Route("dataPurchaseTrans")]
        [HttpPost]
        public async Task<IActionResult> PurchaseBundles([FromBody]PurchaseTranModel purchaseTranModel)
        {
            try
            {
                var response = await _tbotService.BundlePurchaseTran(purchaseTranModel);
                if(response == null)
                {
                    return BadRequest("no transaction ID was returned");
                }
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Purchase bundles error {ex.Message}");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not record purchase transaction");
            }
        }

      

    }
}
