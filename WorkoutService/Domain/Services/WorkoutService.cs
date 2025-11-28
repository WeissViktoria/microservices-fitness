using Domain.Interfaces;
using Model.Entities;

namespace Domain.Services;

public class WorkoutService
{
    private readonly IRepositoryAsync<Workout> _workoutRepository;

    public WorkoutService(IRepositoryAsync<Workout> workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public Task<List<Workout>> GetAllAsync() => _workoutRepository.ReadAllAsync();

    public Task<Workout?> GetByIdAsync(int workoutId) => _workoutRepository.ReadAsync(workoutId);

    public Task<List<Workout>> GetByParticipantAsync(int participantId) =>
        _workoutRepository.ReadAsync(w => w.ParticipantId == participantId);

    public async Task<Workout> CreateAsync(WorkoutCreateModel model)
    {
        ValidateWorkout(model.ParticipantId, model.ExerciseType, model.Duration, model.Repetitions, model.Date);

        var normalizedExercise = model.ExerciseType.Trim();
        var intensity = DetermineIntensity(model.Duration, model.Repetitions, model.Intensity);

        var entity = new Workout
        {
            ParticipantId = model.ParticipantId,
            ExerciseType = normalizedExercise,
            Duration = model.Duration,
            Repetitions = model.Repetitions,
            Intensity = intensity,
            Date = model.Date,
            CaloriesBurned = CalculateCalories(model.Duration, intensity, normalizedExercise),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _workoutRepository.CreateAsync(entity);
    }

    public async Task<Workout> UpdateAsync(int workoutId, WorkoutUpdateModel model)
    {
        var entity = await _workoutRepository.ReadAsync(workoutId)
                     ?? throw new KeyNotFoundException($"Workout with id {workoutId} was not found.");

        if (!string.IsNullOrWhiteSpace(model.ExerciseType))
        {
            entity.ExerciseType = model.ExerciseType.Trim();
        }

        if (model.Duration.HasValue)
        {
            if (model.Duration.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(model.Duration), "Duration must be greater than zero.");
            }
            entity.Duration = model.Duration.Value;
        }

        if (model.Repetitions.HasValue)
        {
            if (model.Repetitions.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(model.Repetitions), "Repetitions must be zero or more.");
            }
            entity.Repetitions = model.Repetitions.Value;
        }

        if (model.Date.HasValue)
        {
            if (model.Date.Value > DateTime.UtcNow.AddDays(1))
            {
                throw new ArgumentOutOfRangeException(nameof(model.Date), "Date cannot be in the far future.");
            }
            entity.Date = model.Date.Value;
        }

        var intensity = DetermineIntensity(entity.Duration, entity.Repetitions, model.Intensity ?? entity.Intensity);
        entity.Intensity = intensity;
        entity.CaloriesBurned = CalculateCalories(entity.Duration, intensity, entity.ExerciseType);
        entity.UpdatedAt = DateTime.UtcNow;

        await _workoutRepository.UpdateAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(int workoutId)
    {
        var entity = await _workoutRepository.ReadAsync(workoutId)
                     ?? throw new KeyNotFoundException($"Workout with id {workoutId} was not found.");

        await _workoutRepository.DeleteAsync(entity);
    }

    private static void ValidateWorkout(int participantId, string exerciseType, int duration, int repetitions, DateTime date)
    {
        if (participantId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(participantId), "ParticipantId must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(exerciseType))
        {
            throw new ArgumentException("Exercise type is required.", nameof(exerciseType));
        }

        if (duration <= 0 || duration > 300)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be between 1 and 300 minutes.");
        }

        if (repetitions < 0 || repetitions > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(repetitions), "Repetitions must be between 0 and 1000.");
        }

        if (date > DateTime.UtcNow.AddDays(1))
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Date cannot be more than one day in the future.");
        }
    }

    private static string DetermineIntensity(int duration, int repetitions, string? requestedIntensity)
    {
        if (!string.IsNullOrWhiteSpace(requestedIntensity))
        {
            return requestedIntensity.Trim();
        }

        var workloadScore = duration + (int)Math.Round(repetitions * 0.5);

        return workloadScore switch
        {
            >= 120 => "Hoch",
            >= 60 => "Mittel",
            _ => "Niedrig"
        };
    }

    private static int CalculateCalories(int duration, string intensity, string exerciseType)
    {
        var normalizedIntensity = intensity.ToLowerInvariant();
        var normalizedExercise = exerciseType.ToLowerInvariant();

        var baseRate = normalizedExercise switch
        {
            _ when normalizedExercise.Contains("hiit") => 13.5,
            _ when normalizedExercise.Contains("kraft") || normalizedExercise.Contains("strength") => 9.5,
            _ when normalizedExercise.Contains("yoga") || normalizedExercise.Contains("mobility") => 5.5,
            _ when normalizedExercise.Contains("lauf") || normalizedExercise.Contains("run") => 11,
            _ => 8.0
        };

        var intensityFactor = normalizedIntensity switch
        {
            "hoch" => 1.3,
            "mittel" => 1.0,
            "niedrig" => 0.75,
            _ => 1.0
        };

        return (int)Math.Round(duration * baseRate * intensityFactor);
    }
}

public record WorkoutCreateModel(
    int ParticipantId,
    string ExerciseType,
    int Duration,
    int Repetitions,
    DateTime Date,
    string? Intensity = null);

public record WorkoutUpdateModel(
    string? ExerciseType = null,
    int? Duration = null,
    int? Repetitions = null,
    DateTime? Date = null,
    string? Intensity = null);

