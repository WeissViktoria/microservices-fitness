namespace Model.Entities;

public class History
{
    public int AnalyticsId { get; set; }
    public int ParticipantId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TotalWorkouts { get; set; }
    public double AverageDuration { get; set; }
    public double TotalCalories { get; set; }
    public string? BestPerformance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
