using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using WebGUI.Models.Recommendations;

namespace WebGUI.Controllers;

public class RecommendationController : Controller
{
    private readonly ILogger<RecommendationController> _logger;
    private readonly RecommendationService _recommendationService;
    private readonly RecommendationDbContext _recommendationContext;

    public RecommendationController(
        ILogger<RecommendationController> logger,
        RecommendationService recommendationService,
        RecommendationDbContext recommendationContext)
    {
        _logger = logger;
        _recommendationService = recommendationService;
        _recommendationContext = recommendationContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int participantId = 1, bool isPremium = false)
    {
        var request = new RecommendationRequestModel
        {
            ParticipantId = participantId,
            IsPremium = isPremium
        };

        var model = await BuildRecommendationViewModelAsync(request);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Generate(RecommendationRequestModel request)
    {
        if (request.ParticipantId <= 0)
        {
            ModelState.AddModelError("ParticipantId", "Teilnehmer-ID muss größer als 0 sein.");
            var model = await BuildRecommendationViewModelAsync(request);
            return View("Index", model);
        }

        try
        {
            // Generate new recommendations
            var recommendations = await _recommendationService.GenerateRecommendationsAsync(
                request.ParticipantId,
                request.IsPremium);

            // Redirect to show the generated recommendations
            return RedirectToAction("Index", new
            {
                participantId = request.ParticipantId,
                isPremium = request.IsPremium
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate recommendations for participant {ParticipantId}", request.ParticipantId);
            var model = await BuildRecommendationViewModelAsync(request);
            model.ErrorMessage = "Beim Generieren der Recommendation ist ein Fehler aufgetreten: " + ex.Message;
            return View("Index", model);
        }
    }

    private async Task<RecommendationPageViewModel> BuildRecommendationViewModelAsync(RecommendationRequestModel request)
    {
        var viewModel = new RecommendationPageViewModel { Request = request };

        try
        {
            var recommendations = await _recommendationService.GetRecommendationsAsync(
                request.ParticipantId,
                request.IsPremium);

            viewModel.Recommendations = recommendations
                .Select(r => new RecommendationItemViewModel
                {
                    RecommendationId = r.RecommendationId,
                    ParticipantId = r.ParticipantId,
                    RecommendationType = r.RecommendationType,
                    Description = r.Description,
                    DateGenerated = r.DateGenerated,
                    IsPremium = r.Description.Contains("PREMIUM:")
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load recommendations for participant {ParticipantId}", request.ParticipantId);
            viewModel.ErrorMessage = "Beim Laden der Recommendation ist ein Fehler aufgetreten.";
        }

        return viewModel;
    }
}

