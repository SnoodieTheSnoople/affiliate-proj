using affiliate_proj.Application.Interfaces.Account.Rates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CreatorCommissionsController : ControllerBase
{
    private readonly ICommissionRatesService _commissionRatesService;

    public CreatorCommissionsController(ICommissionRatesService commissionRatesService)
    {
        _commissionRatesService = commissionRatesService;
    }

    [HttpPost("set-commission-rate")]
    public async Task<IActionResult> SetCommissionRate()
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

    [HttpDelete("delete-commission-rate")]
    public async Task<IActionResult> DeleteCommissionRate()
    {
        throw new NotImplementedException();
    }
}