using affiliate_proj.Application.Interfaces.CommissionAttribution;
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
    }
}
