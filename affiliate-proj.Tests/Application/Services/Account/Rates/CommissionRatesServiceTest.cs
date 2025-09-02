using System;
using System.Threading.Tasks;
using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Application.Services.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
using JetBrains.Annotations;
using Moq;
using NuGet.ContentModel;
using Xunit;

namespace affiliate_proj.Tests.Application.Services.Account.Rates;

[TestSubject(typeof(CommissionRatesService))]
public class CommissionRatesServiceTest
{
    private readonly Mock<ICommissionRatesRepository> _commissionRatesRepository;
    private readonly CommissionRatesService _commissionRatesService;

    public CommissionRatesServiceTest()
    {
        _commissionRatesRepository = new Mock<ICommissionRatesRepository>();
        _commissionRatesService = new CommissionRatesService(_commissionRatesRepository.Object);
    }

    [Fact]
    public async Task SetCommissionRateAsync_Throws_WhenDtoIsNull()
    {
        CreateCommissionRateDTO dto = null;
        await Assert.ThrowsAsync<ArgumentNullException>(() => _commissionRatesService.SetCommissionRateAsync(dto));
    }

    [Fact]
    [NotNull]
    public async Task SetCommissionRateAsync_Throws_WhenDtoHasRateGreaterThan100()
    {
        CreateCommissionRateDTO dto = new CreateCommissionRateDTO()
        {
            CreatorId = Guid.NewGuid(),
            StoreId = Guid.NewGuid(),
            Rate = 150,
        };
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _commissionRatesService.SetCommissionRateAsync(dto));
    }
}