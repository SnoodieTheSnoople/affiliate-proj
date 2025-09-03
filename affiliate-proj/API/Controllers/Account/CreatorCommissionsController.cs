using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
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
    public async Task<IActionResult> SetCommissionRate([FromBody] CreateCommissionRateDTO createCommissionRateDTO)
    {
        try
        {
            return Ok(await _commissionRatesService.SetCommissionRateAsync(createCommissionRateDTO));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-commission-rates")]
    public async Task<IActionResult> GetCommissionRatesAsync([FromQuery] Guid creatorId, [FromQuery] Guid storeId,
        [FromQuery] Guid rateId)
    {
        try
        {
            if (creatorId == Guid.Empty && storeId == Guid.Empty && rateId == Guid.Empty)
                return BadRequest();
            
            
            if (creatorId != Guid.Empty && storeId == Guid.Empty && rateId == Guid.Empty)
                return Ok(await _commissionRatesService.GetCommissionRatesAsync(creatorId, 'c'));
            
            if (creatorId == Guid.Empty && storeId != Guid.Empty && rateId == Guid.Empty)
                return Ok(await _commissionRatesService.GetCommissionRatesAsync(storeId, 's'));

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-commission-rate")]
    public async Task<IActionResult> GetCommissionRateByRateIdAsync([FromQuery] Guid rateId)
    {
        try
        {
            if (rateId == Guid.Empty)
                return BadRequest();
            
            return Ok(await _commissionRatesService.GetCommissionRateByRateIdAsync(rateId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("update-commission-rate")]
    public async Task<IActionResult> UpdateCommissionRateAsync([FromBody] CommissionRateDTO commissionRateDTO)
    {
        try
        {
            if (commissionRateDTO == null)
                return BadRequest();

            if (commissionRateDTO.Rate == null)
                return BadRequest();

            if (commissionRateDTO.RateId ==  Guid.Empty)
                return BadRequest();

            return Ok(await _commissionRatesService.UpdateCommissionRateAsync(commissionRateDTO));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("accept-commission-rate")]
    public async Task<IActionResult> AcceptCommissionRateAsync([FromBody] CommissionRateDTO commissionRateDTO)
    {
        try
        {
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        throw new NotImplementedException();
    }

    [HttpDelete("delete-commission-rate")]
    public async Task<IActionResult> DeleteCommissionRate(Guid rateId)
    {
        try
        {
            if (rateId == Guid.Empty)
                return BadRequest("ID empty");

            var checkCommissionRateNull = await _commissionRatesService.DeleteCommissionRateAsync(rateId);

            if (checkCommissionRateNull != null)
                return BadRequest();
            
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}