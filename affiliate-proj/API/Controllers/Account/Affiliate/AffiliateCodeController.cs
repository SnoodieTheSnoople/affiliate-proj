using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
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
    public async Task<IActionResult> SetAffiliateCode()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("get-affiliate-code")]
    public async Task<IActionResult> GetAffiliateCode()
    {
        throw new NotImplementedException();
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