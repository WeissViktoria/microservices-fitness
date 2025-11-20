using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;
using RestApi.Controllers;


namespace RestApi.Controllers;

[ApiController]
[Route("[analytic]")]
public class AnalyticController2 
    : AController<History, AnalyticsRecords.CreateAnalyticsDto, AnalyticsRecords.ReadAnalyticsDto, AnalyticsRecords.UpdateAnalyticsDto>
{
    private readonly IRepositoryAsync<History> _repository;

    public AnalyticController2(
        IRepositoryAsync<History> repository,
        ILogger<AController<History, 
            AnalyticsRecords.CreateAnalyticsDto, 
            AnalyticsRecords.ReadAnalyticsDto, 
            AnalyticsRecords.UpdateAnalyticsDto>> logger)
        : base(repository, logger)
    {
        _repository = repository;
    }
}
