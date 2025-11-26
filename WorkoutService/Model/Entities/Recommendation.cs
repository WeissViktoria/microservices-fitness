using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

public class Recommendation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecommendationId { get; set; } 

    [Required]
    public int ParticipantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string RecommendationType { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime DateGenerated { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}
