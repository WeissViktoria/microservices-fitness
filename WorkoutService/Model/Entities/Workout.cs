using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

public class Workout
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WorkoutId { get; set; }

    [Required]
    public int ParticipantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ExerciseType { get; set; } = null!;

    [Required]
    public int Duration { get; set; }   // Dauer in Minuten

    [Required]
    public int Repetitions { get; set; }

    [Required]
    [MaxLength(50)]
    public string Intensity { get; set; } = null!; // z.B. "Hoch", "Mittel", "Niedrig"

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int CaloriesBurned { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}
