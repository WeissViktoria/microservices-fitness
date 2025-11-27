using System.ComponentModel.DataAnnotations;

namespace RestApi.Dtos;

public class CreateRecommendationDto
{
    [Required]
    public int ParticipantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RecommendationType { get; set; } = string.Empty; // z.B. "Training", "Ernährung"

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}

public class ReadRecommendationDto
{
    public int RecommendationId { get; set; }
    public int ParticipantId { get; set; }
    public string RecommendationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime DateGenerated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateRecommendationDto
{
    [Required]
    [MaxLength(50)]
    public string RecommendationType { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}