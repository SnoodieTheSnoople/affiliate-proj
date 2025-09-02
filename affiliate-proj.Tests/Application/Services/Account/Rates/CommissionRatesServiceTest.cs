using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Application.Services.Account.Rates;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace affiliate_proj.Tests.Application.Services.Account.Rates;

[TestSubject(typeof(CommissionRatesService))]
public class CommissionRatesServiceTest
{
    private readonly Mock<ICommissionRatesRepository> _commissionRatesRepository;
    private readonly CommissionRatesService _commissionRatesService;

    public CommissionRatesServiceTest(Mock<ICommissionRatesRepository> commissionRatesRepository)
    {
        _commissionRatesRepository = commissionRatesRepository;
        _commissionRatesService = new CommissionRatesService(_commissionRatesRepository.Object);
    }

    [Fact]
    public void METHOD()
    {
        
    }
}