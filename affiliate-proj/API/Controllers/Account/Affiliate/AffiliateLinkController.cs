using affiliate_proj.Core.DTOs.Affiliate.Link;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account.Affiliate
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffiliateLinkController : ControllerBase
    {
        [HttpPost("set-affiliate-link")]
        public async Task<IActionResult> SetAffiliateLink([FromBody] CreateAffiliateLinkDTO createAffiliateLinkDTO)
        {
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
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-affiliate-links")]
        public async Task<IActionResult> GetAffiliateLinks()
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
        
        [HttpPut("update-affiliate-link")]
        public async Task<IActionResult> UpdateAffiliateLink()
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
