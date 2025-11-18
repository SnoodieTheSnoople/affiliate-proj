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
}