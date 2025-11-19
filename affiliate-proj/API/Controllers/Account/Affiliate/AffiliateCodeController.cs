using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Core.DTOs.Affiliate.Code;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account.Affiliate;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class AffiliateCodeController : ControllerBase
{
    private readonly IAffiliateCodeService _affiliateCodeService;

    public AffiliateCodeController(IAffiliateCodeService affiliateCodeService)
    {
        _affiliateCodeService = affiliateCodeService;
    }

    [HttpPost("set-affiliate-code")]
    public async Task<IActionResult> SetAffiliateCode([FromBody] CreateAffiliateCodeDTO createAffiliateCodeDto)
    {
        try
        {
            if (createAffiliateCodeDto.CreatorId == Guid.Empty || createAffiliateCodeDto.StoreId == Guid.Empty || 
                string.IsNullOrEmpty(createAffiliateCodeDto.Code) || createAffiliateCodeDto.ValidFor <= 0 ||
                createAffiliateCodeDto.ExpiryDate <= DateTime.Today)
            {
                return BadRequest("Invalid input data.");
            }

            return Ok(await _affiliateCodeService.SetAffiliateCodeAsync(createAffiliateCodeDto));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("get-affiliate-codes")]
    public async Task<IActionResult> GetAffiliateCodes()
    {
        try
        {
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("update-affiliate-code")]
    public async Task<IActionResult> UpdateAffiliateCode()
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("delete-affiliate-code")]
    public async Task<IActionResult> DeleteAffiliateCode()
    {
        throw new NotImplementedException();
    }
}