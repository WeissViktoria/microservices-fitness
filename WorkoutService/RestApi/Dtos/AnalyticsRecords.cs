using Domain.Interfaces;

namespace RestApi.Dtos;

public class AnalyticsRecords 
{
    public record ReadAnalyticsDto(
        int AnalyticsId,
        int ParticipantId,
        DateTime PeriodStart,
        DateTime PeriodEnd,
        int TotalWorkouts,
        double AverageDuration,
        double TotalCalories,
        string? BestPerformance = null);
    public record CreateAnalyticsDto(
        int ParticipantId,
        DateTime PeriodStart,
        DateTime PeriodEnd);

    public record UpdateAnalyticsDto(
        int AnalyticsId,
        int TotalWorkouts,
        double AverageDuration,
        double TotalCalories);
}