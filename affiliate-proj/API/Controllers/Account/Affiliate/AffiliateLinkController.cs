using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Core.DTOs.Affiliate.Link;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account.Affiliate
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffiliateLinkController : ControllerBase
    {
        private readonly IAffiliateLinkService _affiliateLinkService;

        public AffiliateLinkController(IAffiliateLinkService affiliateLinkService)
        {
            _affiliateLinkService = affiliateLinkService;
        }

        [HttpPost("set-affiliate-link")]
        public async Task<IActionResult> SetAffiliateLink([FromBody] CreateAffiliateLinkDTO createAffiliateLinkDTO)
        {
            // TODO: Modify to accommodate change for IsActive field
            try
            {
                if (createAffiliateLinkDTO.CreatorId == Guid.Empty ||
                    createAffiliateLinkDTO.StoreId == Guid.Empty ||
                    string.IsNullOrEmpty(createAffiliateLinkDTO.Link) ||
                    string.IsNullOrEmpty(createAffiliateLinkDTO.RefParam) ||
                    string.IsNullOrEmpty(createAffiliateLinkDTO.ProductLink))
                {
                    return BadRequest("Invalid input data.");
                }
                
                return Ok(await _affiliateLinkService.SetAffiliateLinkAsync(createAffiliateLinkDTO));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-affiliate-links")]
        public async Task<IActionResult> GetAffiliateLinks([FromBody] CreateAffiliateLinkDTO createAffiliateLinkDTO)
        {
            try
            {
                if (createAffiliateLinkDTO.CreatorId != Guid.Empty && createAffiliateLinkDTO.StoreId == Guid.Empty)
                {
                    return Ok(await _affiliateLinkService.GetAffiliateLinksByCreatorIdAsync(createAffiliateLinkDTO.CreatorId));
                }

                if (createAffiliateLinkDTO.StoreId != Guid.Empty && createAffiliateLinkDTO.CreatorId == Guid.Empty)
                {
                    return Ok(await _affiliateLinkService.GetAffiliateLinksByStoreIdAsync(createAffiliateLinkDTO.StoreId));
                }

                return BadRequest("Invalid input data.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("update-affiliate-link")]
        public async Task<IActionResult> UpdateAffiliateLink([FromBody] AffiliateLinkDTO affiliateLinkDTO)
        {
            try
            {
                if (affiliateLinkDTO.LinkId == Guid.Empty ||
                    affiliateLinkDTO.CreatorId == Guid.Empty ||
                    affiliateLinkDTO.StoreId == Guid.Empty ||
                    string.IsNullOrEmpty(affiliateLinkDTO.Link) ||
                    string.IsNullOrEmpty(affiliateLinkDTO.RefParam) ||
                    string.IsNullOrEmpty(affiliateLinkDTO.ProductLink))
                {
                    return BadRequest("Invalid input data.");
                }
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete-affiliate-link")]
        public async Task<IActionResult> DeleteAffiliateLink()
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
    }
}
