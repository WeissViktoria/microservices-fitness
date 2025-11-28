using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;

namespace RestApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly WorkoutService _workoutService;
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(WorkoutService workoutService, ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ReadWorkoutDto>> CreateAsync([FromBody] CreateWorkoutDto dto)
    {
        var workout = await _workoutService.CreateAsync(dto.ToCreateModel());
        _logger.LogInformation("Created workout {WorkoutId} for participant {ParticipantId}", workout.WorkoutId, workout.ParticipantId);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = workout.WorkoutId }, workout.ToReadDto());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadWorkoutDto>> GetByIdAsync(int id)
    {
        var workout = await _workoutService.GetByIdAsync(id);

        if (workout is null)
        {
            return NotFound();
        }

        return Ok(workout.ToReadDto());
    }

    [HttpGet]
    public async Task<ActionResult<List<ReadWorkoutDto>>> GetAllAsync([FromQuery] int? participantId)
    {
        List<Workout> workouts = participantId.HasValue
            ? await _workoutService.GetByParticipantAsync(participantId.Value)
            : await _workoutService.GetAllAsync();

        return Ok(workouts.Select(w => w.ToReadDto()).ToList());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReadWorkoutDto>> UpdateAsync(int id, [FromBody] UpdateWorkoutDto dto)
    {
        var updated = await _workoutService.UpdateAsync(id, dto.ToUpdateModel());
        _logger.LogInformation("Updated workout {WorkoutId}", updated.WorkoutId);
        return Ok(updated.ToReadDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _workoutService.DeleteAsync(id);
        _logger.LogInformation("Deleted workout {WorkoutId}", id);
        return NoContent();
    }
}

internal static class WorkoutDtoMapper
{
    public static WorkoutCreateModel ToCreateModel(this CreateWorkoutDto dto) =>
        new(dto.ParticipantId, dto.ExerciseType, dto.Duration, dto.Repetitions, dto.Date, dto.Intensity);

    public static WorkoutUpdateModel ToUpdateModel(this UpdateWorkoutDto dto) =>
        new(dto.ExerciseType, dto.Duration, dto.Repetitions, dto.Date, dto.Intensity);

    public static ReadWorkoutDto ToReadDto(this Workout workout) => new()
    {
        WorkoutId = workout.WorkoutId,
        ParticipantId = workout.ParticipantId,
        ExerciseType = workout.ExerciseType,
        Duration = workout.Duration,
        Repetitions = workout.Repetitions,
        Intensity = workout.Intensity,
        Date = workout.Date,
        CaloriesBurned = workout.CaloriesBurned,
        CreatedAt = workout.CreatedAt,
        UpdatedAt = workout.UpdatedAt
    };
}