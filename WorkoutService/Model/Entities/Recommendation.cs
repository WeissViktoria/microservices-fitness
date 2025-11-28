namespace Model.Entities;

public class Recommendation
{
    public int RecommendationId { get; set; }
    public int ParticipantId { get; set; }
    public string RecommendationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime DateGenerated { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

