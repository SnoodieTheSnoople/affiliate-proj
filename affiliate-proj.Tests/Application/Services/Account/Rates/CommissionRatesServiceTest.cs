using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Application.Services.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;
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

    [Fact]
    public async Task SetComissionRateAsync_CallsRepo_WhenValidDto()
    {
        CreateCommissionRateDTO dto = new CreateCommissionRateDTO()
        {
            CreatorId = Guid.NewGuid(),
            StoreId = Guid.NewGuid(),
            Rate = 10,
            IsAccepted = false
        };

        var expected = new CommissionRateDTO
        {
            CreatorId = dto.CreatorId,
            StoreId = dto.StoreId,
            Rate = dto.Rate,
            IsAccepted = false
        };

        _commissionRatesRepository.Setup(r => r.SetCommissionRateAsync(It.IsAny<CommissionRate>()))
            .ReturnsAsync(expected);

        var result = await _commissionRatesService.SetCommissionRateAsync(dto);
        
        Assert.NotNull(result);
        Assert.Equal(expected.Rate, result.Rate);
        
        _commissionRatesRepository.Verify( r => 
            r.SetCommissionRateAsync(It.Is<CommissionRate>(c => 
                c.Rate == expected.Rate && 
                c.CreatorId == expected.CreatorId &&
                c.StoreId == expected.StoreId && 
                c.IsAccepted == expected.IsAccepted)), 
            Times.Once);
    }


    [Fact]
    public async Task GetCommissionRatesAsync_Throws_WhenIdEmpty()
    {
        var emptyId = Guid.Empty;
        
        await Assert.ThrowsAsync<NullReferenceException>(() => 
            _commissionRatesService.GetCommissionRatesAsync(emptyId, 't'));
    }

    [Fact]
    public async Task GetCommissionRatesAsync_ReturnsEmptyList_WhenPurposeTypeIncorrect()
    {
        var id = Guid.NewGuid();
        var incorrectPurposeType = 't';

        var expected = new List<CommissionRateDTO>();
        
        var result =  await _commissionRatesService.GetCommissionRatesAsync(id, incorrectPurposeType);
        
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }
}