using System.Diagnostics;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using WebGUI.Models;
using WebGUI.Models.Analytics;

namespace WebGUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AnalyticService _analyticService;
    private readonly WorkoutDbContext _workoutDbContext;

    public HomeController(ILogger<HomeController> logger, AnalyticService analyticService, WorkoutDbContext workoutDbContext)
    {
        _logger = logger;
        _analyticService = analyticService;
        _workoutDbContext = workoutDbContext;
    }

    public async Task<IActionResult> Index()
    {
        var defaultRequest = new AnalyticsRequestModel();
        var model = await BuildAnalyticsViewModelAsync(defaultRequest);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Analytics(int participantId = 1, DateTime? periodStart = null, DateTime? periodEnd = null)
    {
        var request = new AnalyticsRequestModel
        {
            ParticipantId = participantId,
            PeriodStart = periodStart ?? DateTime.Today.AddDays(-7),
            PeriodEnd = periodEnd ?? DateTime.Today
        };

        var model = await BuildAnalyticsViewModelAsync(request);
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<AnalyticsPageViewModel> BuildAnalyticsViewModelAsync(AnalyticsRequestModel request)
    {
        var viewModel = new AnalyticsPageViewModel { Request = request };

        try
        {
            var summary = await _analyticService.GetAnalysisAsync(
                request.ParticipantId,
                request.PeriodStart,
                request.PeriodEnd);

            viewModel.Summary = new AnalyticsSummaryViewModel
            {
                ParticipantId = summary.ParticipantId,
                PeriodStart = summary.PeriodStart,
                PeriodEnd = summary.PeriodEnd,
                TotalWorkouts = summary.TotalWorkouts,
                AverageDuration = summary.AverageDuration,
                TotalCalories = summary.TotalCalories,
                BestPerformance = summary.BestPerformance ?? "n/a",
                GeneratedAt = summary.UpdatedAt
            };

            var history = await _analyticService.GetHistoricalAnalysisAsync(
                request.ParticipantId,
                request.PeriodStart.AddMonths(-3),
                request.PeriodEnd);

            viewModel.History = history
                .OrderByDescending(h => h.PeriodEnd)
                .Select(h => new AnalyticsHistoryItemViewModel
                {
                    AnalyticsId = h.AnalyticsId,
                    PeriodStart = h.PeriodStart,
                    PeriodEnd = h.PeriodEnd,
                    TotalWorkouts = h.TotalWorkouts,
                    AverageDuration = h.AverageDuration,
                    TotalCalories = h.TotalCalories,
                    BestPerformance = h.BestPerformance ?? "n/a",
                    UpdatedAt = h.UpdatedAt
                })
                .ToList();

            viewModel.RecentWorkouts = await _workoutDbContext.Workouts
                .Where(w => w.ParticipantId == request.ParticipantId
                         && w.Date >= request.PeriodStart.AddMonths(-1)
                         && w.Date <= request.PeriodEnd)
                .OrderByDescending(w => w.Date)
                .Take(5)
                .Select(w => new WorkoutListItemViewModel
                {
                    Date = w.Date,
                    ExerciseType = w.ExerciseType,
                    Intensity = w.Intensity,
                    Duration = w.Duration,
                    Calories = w.CaloriesBurned
                })
                .ToListAsync();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "No analytics data for participant {ParticipantId}", request.ParticipantId);
            viewModel.ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build analytics view");
            viewModel.ErrorMessage = "Beim Laden der Analytics-Daten ist ein Fehler aufgetreten.";
        }

        return viewModel;
    }
}