using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

public class History
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // Primärschlüssel, automatisch generiert

    [Required]
    public int AnalyticsId { get; set; }

    [Required]
    public int ParticipantId { get; set; }

    [Required]
    public DateTime PeriodStart { get; set; }

    [Required]
    public DateTime PeriodEnd { get; set; }

    [Required]
    public int TotalWorkouts { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public double AverageDuration { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public double TotalCalories { get; set; }

    [MaxLength(255)]
    public string? BestPerformance { get; set; }  // optional, max 255 Zeichen

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}
