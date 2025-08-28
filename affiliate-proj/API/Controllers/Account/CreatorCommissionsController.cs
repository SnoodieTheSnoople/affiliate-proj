using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CreatorCommissionsController
{
    [HttpPost("set-commission-rate")]
    public async Task<IActionResult> SetCommissionRate()
    {
        throw new NotImplementedException();
    }

    [HttpGet("get-commission-rate")]
    public async Task<IActionResult> GetCommissionRate()
    {
        throw new NotImplementedException();
    }

    [HttpPut("update-commission-rate")]
    public async Task<IActionResult> UpdateCommissionRate()
    {
        throw new NotImplementedException();
    }
}