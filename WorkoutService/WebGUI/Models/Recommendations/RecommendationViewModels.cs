namespace WebGUI.Models.Recommendations;

public class RecommendationRequestModel
{
    public int ParticipantId { get; set; } = 1;
    public bool IsPremium { get; set; } = false;
}

public class RecommendationItemViewModel
{
    public int RecommendationId { get; set; }
    public int ParticipantId { get; set; }
    public string RecommendationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateGenerated { get; set; }
    public bool IsPremium { get; set; }
}

public class RecommendationPageViewModel
{
    public RecommendationRequestModel Request { get; set; } = new();
    public List<RecommendationItemViewModel> Recommendations { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public bool HasRecommendations => Recommendations.Any();
}

