using System.ComponentModel.DataAnnotations;

namespace RestApi.Dtos;

public class CreateWorkoutDto
{
    [Required]
    public int ParticipantId { get; set; }

    [Required]
    [StringLength(100)]
    public string ExerciseType { get; set; } = string.Empty;

    [Range(1, 300)]
    public int Duration { get; set; }

    [Range(0, 1000)]
    public int Repetitions { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string? Intensity { get; set; }
}

public class ReadWorkoutDto
{
    public int WorkoutId { get; set; }
    public int ParticipantId { get; set; }
    public string ExerciseType { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Repetitions { get; set; }
    public string Intensity { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CaloriesBurned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateWorkoutDto
{
    [StringLength(100)]
    public string? ExerciseType { get; set; }

    [Range(1, 300)]
    public int? Duration { get; set; }

    [Range(0, 1000)]
    public int? Repetitions { get; set; }

    public DateTime? Date { get; set; }

    [StringLength(50)]
    public string? Intensity { get; set; }
}