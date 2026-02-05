using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.Shopify.Conversion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Test.CommissionAttribution
{
    [Route("api/test/[controller]")]
    [ApiController]
    public class EarnedCommissionController : ControllerBase
    {
        private readonly IEarnedCommissionService _earnedCommissionService;
        private readonly ILogger<EarnedCommissionController> _logger;

        public EarnedCommissionController(IEarnedCommissionService earnedCommissionService, ILogger<EarnedCommissionController> logger)
        {
            _earnedCommissionService = earnedCommissionService;
            _logger = logger;
        }

        [HttpPost("calculate-attributed-commission")]
        public async Task<ActionResult> CalculateAttributedCommissionAsync([FromBody] ConversionDTO conversionDto)
        {
            try
            {
                _logger.LogInformation("Calculating Attributed Commission");
                await _earnedCommissionService.CalculateAttributedCommissionAsync(conversionDto);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}
