using System.ComponentModel.DataAnnotations;

namespace Model.Entities;

public class Recommendation
{
    [Key]
    public int RecommendationId { get; set; }
    public int ParticipantId { get; set; }
    public string RecommendationType { get; set; }
    public string Description { get; set; }
    public DateTime DateGenerated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
