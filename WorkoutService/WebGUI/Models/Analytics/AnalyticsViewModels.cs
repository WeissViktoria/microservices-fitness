namespace WebGUI.Models.Analytics;

public class AnalyticsRequestModel
{
    public int ParticipantId { get; set; } = 1;
    public DateTime PeriodStart { get; set; } = DateTime.Today.AddDays(-7);
    public DateTime PeriodEnd { get; set; } = DateTime.Today;
}

public class AnalyticsSummaryViewModel
{
    public int ParticipantId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TotalWorkouts { get; set; }
    public double AverageDuration { get; set; }
    public double TotalCalories { get; set; }
    public string BestPerformance { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class AnalyticsHistoryItemViewModel
{
    public int AnalyticsId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TotalWorkouts { get; set; }
    public double AverageDuration { get; set; }
    public double TotalCalories { get; set; }
    public string BestPerformance { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class WorkoutListItemViewModel
{
    public DateTime Date { get; set; }
    public string ExerciseType { get; set; } = string.Empty;
    public string Intensity { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Calories { get; set; }
}

public class AnalyticsPageViewModel
{
    public AnalyticsRequestModel Request { get; set; } = new();
    public AnalyticsSummaryViewModel? Summary { get; set; }
    public List<AnalyticsHistoryItemViewModel> History { get; set; } = [];
    public List<WorkoutListItemViewModel> RecentWorkouts { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public bool HasData => Summary != null || History.Count > 0;
}

