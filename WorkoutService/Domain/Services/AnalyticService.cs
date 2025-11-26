using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace Domain.Services;

public class AnalyticService : AnalyticRepository
{
    private readonly WorkoutDbContext _workoutContext;

    public AnalyticService(AnalyticsDbContext analyticsContext, WorkoutDbContext workoutContext) 
        : base(analyticsContext)
    {
        _workoutContext = workoutContext;
    }
    
    public async Task<History> CalculateAndSaveAnalysisAsync(int participantId, DateTime periodStart, DateTime periodEnd)
    {
        // Get all workouts for the participant in the specified period
        var workouts = await _workoutContext.Workouts
            .Where(w => w.ParticipantId == participantId 
                     && w.Date >= periodStart 
                     && w.Date <= periodEnd)
            .ToListAsync();

        if (!workouts.Any())
        {
            throw new InvalidOperationException($"No workouts found for participant {participantId} in the specified period");
        }

        // Calculate statistics
        var totalWorkouts = workouts.Count;
        var averageDuration = workouts.Average(w => w.Duration);
        var totalCalories = workouts.Sum(w => w.CaloriesBurned);
        
        // Find best performance (highest calories burned in a single workout)
        var bestWorkout = workouts.OrderByDescending(w => w.CaloriesBurned).First();
        var bestPerformance = $"{bestWorkout.ExerciseType}: {bestWorkout.CaloriesBurned} calories, {bestWorkout.Duration} min";

        // Check if analysis already exists for this period
        var existingAnalysis = await GetAnalysisByParticipantAndPeriodAsync(participantId, periodStart, periodEnd);

        if (existingAnalysis != null)
        {
            // Update existing analysis
            existingAnalysis.TotalWorkouts = totalWorkouts;
            existingAnalysis.AverageDuration = averageDuration;
            existingAnalysis.TotalCalories = totalCalories;
            existingAnalysis.BestPerformance = bestPerformance;
            existingAnalysis.UpdatedAt = DateTime.UtcNow;

            await UpdateAsync(existingAnalysis);
            return existingAnalysis;
        }
        else
        {
            // Create new analysis
            var newAnalysis = new History
            {
                ParticipantId = participantId,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                TotalWorkouts = totalWorkouts,
                AverageDuration = averageDuration,
                TotalCalories = totalCalories,
                BestPerformance = bestPerformance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await CreateAsync(newAnalysis);
        }
    }

    /// <summary>
    /// Get analysis for a participant and time period (calculates if not exists)
    /// </summary>
    public async Task<History> GetAnalysisAsync(int participantId, DateTime periodStart, DateTime periodEnd)
    {
        var existingAnalysis = await GetAnalysisByParticipantAndPeriodAsync(participantId, periodStart, periodEnd);
        
        if (existingAnalysis != null)
        {
            return existingAnalysis;
        }

        // Calculate and save if not exists
        return await CalculateAndSaveAnalysisAsync(participantId, periodStart, periodEnd);
    }

    /// <summary>
    /// Get historical analysis with aggregated values verification
    /// </summary>
    public async Task<List<History>> GetHistoricalAnalysisAsync(int participantId, DateTime? periodStart = null, DateTime? periodEnd = null)
    {
        var analyses = await GetHistoricalAnalysesAsync(participantId, periodStart, periodEnd);
        
        // Verify aggregated values by recalculating from workouts
        foreach (var analysis in analyses)
        {
            var workouts = await _workoutContext.Workouts
                .Where(w => w.ParticipantId == participantId 
                         && w.Date >= analysis.PeriodStart 
                         && w.Date <= analysis.PeriodEnd)
                .ToListAsync();

            if (workouts.Any())
            {
                var calculatedTotalWorkouts = workouts.Count;
                var calculatedAverageDuration = workouts.Average(w => w.Duration);
                var calculatedTotalCalories = workouts.Sum(w => w.CaloriesBurned);

                // Update if values don't match (data might have changed)
                if (Math.Abs(analysis.TotalWorkouts - calculatedTotalWorkouts) > 0.01 ||
                    Math.Abs(analysis.AverageDuration - calculatedAverageDuration) > 0.01 ||
                    Math.Abs(analysis.TotalCalories - calculatedTotalCalories) > 0.01)
                {
                    analysis.TotalWorkouts = calculatedTotalWorkouts;
                    analysis.AverageDuration = calculatedAverageDuration;
                    analysis.TotalCalories = calculatedTotalCalories;
                    analysis.UpdatedAt = DateTime.UtcNow;
                    await UpdateAsync(analysis);
                }
            }
        }

        return analyses;
    }

    /// <summary>
    /// Get trend analysis comparing two periods
    /// </summary>
    public async Task<Dictionary<string, object>> GetTrendAnalysisAsync(int participantId, DateTime period1Start, DateTime period1End, DateTime period2Start, DateTime period2End)
    {
        var period1Analysis = await GetAnalysisAsync(participantId, period1Start, period1End);
        var period2Analysis = await GetAnalysisAsync(participantId, period2Start, period2End);

        var trends = new Dictionary<string, object>
        {
            ["Period1"] = new
            {
                period1Analysis.TotalWorkouts,
                period1Analysis.AverageDuration,
                period1Analysis.TotalCalories
            },
            ["Period2"] = new
            {
                period2Analysis.TotalWorkouts,
                period2Analysis.AverageDuration,
                period2Analysis.TotalCalories
            },
            ["Trends"] = new
            {
                WorkoutChange = period2Analysis.TotalWorkouts - period1Analysis.TotalWorkouts,
                DurationChange = period2Analysis.AverageDuration - period1Analysis.AverageDuration,
                CaloriesChange = period2Analysis.TotalCalories - period1Analysis.TotalCalories,
                WorkoutChangePercent = period1Analysis.TotalWorkouts > 0 
                    ? ((double)(period2Analysis.TotalWorkouts - period1Analysis.TotalWorkouts) / period1Analysis.TotalWorkouts) * 100 
                    : 0,
                DurationChangePercent = period1Analysis.AverageDuration > 0 
                    ? ((period2Analysis.AverageDuration - period1Analysis.AverageDuration) / period1Analysis.AverageDuration) * 100 
                    : 0,
                CaloriesChangePercent = period1Analysis.TotalCalories > 0 
                    ? ((period2Analysis.TotalCalories - period1Analysis.TotalCalories) / period1Analysis.TotalCalories) * 100 
                    : 0
            }
        };

        return trends;
    }
}