using System.ComponentModel.DataAnnotations;

namespace Model.Entities;

public class Workout
{
    [Key]
    public int WorkoutId { get; set; }
    public int ParticipantId { get; set; }
    public string ExerciseType { get; set; }
    public int Duration { get; set; }
    public int Repetitions { get; set; }
    public string Intensity { get; set; }
    public DateTime Date { get; set; }
    public int CaloriesBurned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
