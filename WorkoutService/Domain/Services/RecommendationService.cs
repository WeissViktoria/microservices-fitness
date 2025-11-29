using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace Domain.Services;

public class RecommendationService
{
    private readonly RecommendationDbContext _recommendationContext;
    private readonly WorkoutDbContext _workoutContext;
    private readonly AnalyticsDbContext _analyticsContext;
    private readonly RecommendationRepository _repository;

    public RecommendationService(
        RecommendationDbContext recommendationContext,
        WorkoutDbContext workoutContext,
        AnalyticsDbContext analyticsContext)
    {
        _recommendationContext = recommendationContext;
        _workoutContext = workoutContext;
        _analyticsContext = analyticsContext;
        _repository = new RecommendationRepository(recommendationContext);
    }

    public async Task<List<Recommendation>> GenerateRecommendationsAsync(int participantId, bool isPremium = false)
    {
        var recommendations = new List<Recommendation>();

        // Get recent workouts (last 30 days)
        var recentWorkouts = await _workoutContext.Workouts
            .Where(w => w.ParticipantId == participantId
                     && w.Date >= DateTime.UtcNow.AddDays(-30))
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        // Get latest analytics
        var latestAnalytics = await _analyticsContext.History
            .Where(h => h.ParticipantId == participantId)
            .OrderByDescending(h => h.CreatedAt)
            .FirstOrDefaultAsync();

        // Generate training recommendations based on workout data
        if (recentWorkouts.Any())
        {
            var avgDuration = recentWorkouts.Average(w => w.Duration);
            var totalCalories = recentWorkouts.Sum(w => w.CaloriesBurned);
            var workoutCount = recentWorkouts.Count;
            var mostCommonExercise = recentWorkouts
                .GroupBy(w => w.ExerciseType)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "";

            // Training recommendation based on average duration
            if (avgDuration < 30)
            {
                recommendations.Add(new Recommendation
                {
                    ParticipantId = participantId,
                    RecommendationType = "Training",
                    Description = $"Ihre durchschnittliche Trainingsdauer liegt bei {avgDuration:F0} Minuten. " +
                                 "Erhöhen Sie die Dauer schrittweise auf 45-60 Minuten für bessere Ergebnisse.",
                    DateGenerated = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else if (avgDuration > 60)
            {
                recommendations.Add(new Recommendation
                {
                    ParticipantId = participantId,
                    RecommendationType = "Training",
                    Description = $"Sie trainieren bereits {avgDuration:F0} Minuten im Durchschnitt. " +
                                 "Achten Sie auf ausreichende Regenerationszeiten zwischen den Einheiten.",
                    DateGenerated = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Training recommendation based on exercise variety
            var uniqueExercises = recentWorkouts.Select(w => w.ExerciseType).Distinct().Count();
            if (uniqueExercises < 3 && workoutCount >= 5)
            {
                recommendations.Add(new Recommendation
                {
                    ParticipantId = participantId,
                    RecommendationType = "Training",
                    Description = $"Sie haben {uniqueExercises} verschiedene Übungstypen in den letzten Workouts. " +
                                 "Erweitern Sie Ihr Training um mehr Abwechslung (z.B. Krafttraining, Cardio, Mobility) für bessere Ergebnisse.",
                    DateGenerated = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Nutrition recommendation based on calories burned
            if (totalCalories > 0)
            {
                var avgCaloriesPerWorkout = totalCalories / workoutCount;
                recommendations.Add(new Recommendation
                {
                    ParticipantId = participantId,
                    RecommendationType = "Ernährung",
                    Description = $"Sie verbrennen durchschnittlich {avgCaloriesPerWorkout:F0} kcal pro Training. " +
                                 "Achten Sie auf eine ausgewogene Ernährung mit ausreichend Protein (ca. 1,5-2g pro kg Körpergewicht) zur optimalen Regeneration.",
                    DateGenerated = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Premium recommendations
            if (isPremium)
            {
                if (latestAnalytics != null)
                {
                    recommendations.Add(new Recommendation
                    {
                        ParticipantId = participantId,
                        RecommendationType = "Training",
                        Description = $"PREMIUM: Basierend auf Ihrer Bestleistung ({latestAnalytics.BestPerformance}) " +
                                     "empfehlen wir ein progressives Intervalltraining mit 30/90s Intervallen zur Steigerung der Ausdauer.",
                        DateGenerated = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

                    recommendations.Add(new Recommendation
                    {
                        ParticipantId = participantId,
                        RecommendationType = "Ernährung",
                        Description = "PREMIUM: 30g Protein innerhalb von 30 Minuten nach Morning-Workouts zur optimalen Regeneration. " +
                                     "Kombinieren Sie Whey-Protein mit schnellen Kohlenhydraten für maximale Effektivität.",
                        DateGenerated = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

                    if (workoutCount >= 10)
                    {
                        recommendations.Add(new Recommendation
                        {
                            ParticipantId = participantId,
                            RecommendationType = "Training",
                            Description = "PREMIUM: Zwei Mobility-Sessions pro Woche halten Ihre Intensität hoch und senken das Verletzungsrisiko erheblich. " +
                                         "Integrieren Sie Yoga oder Stretching in Ihren Trainingsplan.",
                            DateGenerated = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
            }
        }
        else
        {
            // No workouts found - general recommendation
            recommendations.Add(new Recommendation
            {
                ParticipantId = participantId,
                RecommendationType = "Training",
                Description = "Es wurden noch keine Workouts für Sie erfasst. " +
                             "Starten Sie mit 2-3 Trainingseinheiten pro Woche à 30-45 Minuten für einen gesunden Einstieg.",
                DateGenerated = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Save recommendations to database
        foreach (var recommendation in recommendations)
        {
            await _repository.CreateAsync(recommendation);
        }

        return recommendations;
    }

    public async Task<List<Recommendation>> GetRecommendationsAsync(int participantId, bool includePremium = false)
    {
        var query = _recommendationContext.Recommendations
            .Where(r => r.ParticipantId == participantId);

        if (!includePremium)
        {
            query = query.Where(r => !r.Description.Contains("PREMIUM:"));
        }

        return await query
            .OrderByDescending(r => r.DateGenerated)
            .ToListAsync();
    }
}

