using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebGUI.Models.Workouts;

namespace WebGUI.Controllers;

public class WorkoutController : Controller
{
    private readonly WorkoutService _workoutService;
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(WorkoutService workoutService, ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int participantId = 1)
    {
        var model = await BuildPageModelAsync(participantId);
        if (TempData.TryGetValue("WorkoutMessage", out var message))
        {
            model.Message = message?.ToString();
        }
        if (TempData.TryGetValue("WorkoutError", out var error))
        {
            model.Error = error?.ToString();
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(Prefix = "CreateForm")] WorkoutFormModel form)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildPageModelAsync(form.ParticipantId, form);
            invalidModel.Error = "Bitte Eingaben prüfen.";
            return View("Index", invalidModel);
        }

        try
        {
            var createModel = new WorkoutCreateModel(
                form.ParticipantId,
                form.ExerciseType,
                form.Duration,
                form.Repetitions,
                form.Date,
                form.Intensity);

            await _workoutService.CreateAsync(createModel);
            TempData["WorkoutMessage"] = "Workout erfolgreich gespeichert.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create workout for participant {ParticipantId}", form.ParticipantId);
            TempData["WorkoutError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { participantId = form.ParticipantId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(WorkoutUpdateFormModel form)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildPageModelAsync(form.ParticipantId);
            invalidModel.Error = "Ungültige Werte für Dauer oder Intensität.";
            return View("Index", invalidModel);
        }

        try
        {
            var updateModel = new WorkoutUpdateModel(
                Duration: form.Duration,
                Intensity: form.Intensity);

            await _workoutService.UpdateAsync(form.WorkoutId, updateModel);
            TempData["WorkoutMessage"] = $"Workout #{form.WorkoutId} aktualisiert.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update workout {WorkoutId}", form.WorkoutId);
            TempData["WorkoutError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { participantId = form.ParticipantId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int participantId)
    {
        try
        {
            await _workoutService.DeleteAsync(id);
            TempData["WorkoutMessage"] = $"Workout #{id} gelöscht.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete workout {WorkoutId}", id);
            TempData["WorkoutError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { participantId });
    }

    private async Task<WorkoutManagementViewModel> BuildPageModelAsync(int participantId, WorkoutFormModel? formOverride = null)
    {
        var workouts = await _workoutService.GetByParticipantAsync(participantId);

        var viewModel = new WorkoutManagementViewModel
        {
            ParticipantId = participantId,
            Workouts = workouts
                .OrderByDescending(w => w.Date)
                .Select(w => new WorkoutListItemViewModel
                {
                    WorkoutId = w.WorkoutId,
                    Date = w.Date,
                    ExerciseType = w.ExerciseType,
                    Intensity = w.Intensity,
                    Duration = w.Duration,
                    Calories = w.CaloriesBurned
                })
                .ToList(),
            CreateForm = formOverride ?? new WorkoutFormModel
            {
                ParticipantId = participantId,
                Date = DateTime.Today
            }
        };

        return viewModel;
    }
}

