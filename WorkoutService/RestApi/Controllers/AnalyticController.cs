using Domain.Interfaces;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;
using RestApi.Controllers;


namespace RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticController 
    : AController<History, AnalyticsRecords.CreateAnalyticsDto, AnalyticsRecords.ReadAnalyticsDto, AnalyticsRecords.UpdateAnalyticsDto>
{
    private readonly IRepositoryAsync<History> _repository;
    private readonly AnalyticService _analyticService;

    public AnalyticController(
        IRepositoryAsync<History> repository,
        AnalyticService analyticService,
        ILogger<AController<History, 
            AnalyticsRecords.CreateAnalyticsDto, 
            AnalyticsRecords.ReadAnalyticsDto, 
            AnalyticsRecords.UpdateAnalyticsDto>> logger)
        : base(repository, logger)
    {
        _repository = repository;
        _analyticService = analyticService;
    }

      [HttpGet("participant/{participantId}/analysis")]
    public async Task<ActionResult<AnalyticsRecords.ReadAnalyticsDto>> GetAnalysisAsync(
        int participantId,
        [FromQuery] DateTime periodStart,
        [FromQuery] DateTime periodEnd)
    {
        try
        {
            var analysis = await _analyticService.GetAnalysisAsync(participantId, periodStart, periodEnd);
            var dto = new AnalyticsRecords.ReadAnalyticsDto(
                analysis.AnalyticsId,
                analysis.ParticipantId,
                analysis.PeriodStart,
                analysis.PeriodEnd,
                analysis.TotalWorkouts,
                analysis.AverageDuration,
                analysis.TotalCalories,
                analysis.BestPerformance
            );
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogWarning(ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting analysis for participant {ParticipantId}", participantId);
            return StatusCode(500, new { message = "An error occurred while retrieving analysis" });
        }
    }

      [HttpGet("participant/{participantId}/history")]
    public async Task<ActionResult<List<AnalyticsRecords.ReadAnalyticsDto>>> GetHistoricalAnalysisAsync(
        int participantId,
        [FromQuery] DateTime? periodStart = null,
        [FromQuery] DateTime? periodEnd = null)
    {
        try
        {
            var analyses = await _analyticService.GetHistoricalAnalysisAsync(participantId, periodStart, periodEnd);
            var dtos = analyses.Select(a => new AnalyticsRecords.ReadAnalyticsDto(
                a.AnalyticsId,
                a.ParticipantId,
                a.PeriodStart,
                a.PeriodEnd,
                a.TotalWorkouts,
                a.AverageDuration,
                a.TotalCalories,
                a.BestPerformance
            )).ToList();
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting historical analysis for participant {ParticipantId}", participantId);
            return StatusCode(500, new { message = "An error occurred while retrieving historical analysis" });
        }
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<AnalyticsRecords.ReadAnalyticsDto>> CalculateAnalysisAsync(
        [FromBody] AnalyticsRecords.CreateAnalyticsDto createDto)
    {
        try
        {
            var analysis = await _analyticService.CalculateAndSaveAnalysisAsync(
                createDto.ParticipantId,
                createDto.PeriodStart,
                createDto.PeriodEnd);

            var dto = new AnalyticsRecords.ReadAnalyticsDto(
                analysis.AnalyticsId,
                analysis.ParticipantId,
                analysis.PeriodStart,
                analysis.PeriodEnd,
                analysis.TotalWorkouts,
                analysis.AverageDuration,
                analysis.TotalCalories,
                analysis.BestPerformance
            );
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogWarning(ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error calculating analysis");
            return StatusCode(500, new { message = "An error occurred while calculating analysis" });
        }
    }

    [HttpGet("participant/{participantId}/trends")]
    public async Task<ActionResult<Dictionary<string, object>>> GetTrendAnalysisAsync(
        int participantId,
        [FromQuery] DateTime period1Start,
        [FromQuery] DateTime period1End,
        [FromQuery] DateTime period2Start,
        [FromQuery] DateTime period2End)
    {
        try
        {
            var trends = await _analyticService.GetTrendAnalysisAsync(
                participantId,
                period1Start,
                period1End,
                period2Start,
                period2End);
            return Ok(trends);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting trend analysis for participant {ParticipantId}", participantId);
            return StatusCode(500, new { message = "An error occurred while retrieving trend analysis" });
        }
    }
}
