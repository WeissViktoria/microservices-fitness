using System.ComponentModel.DataAnnotations;

namespace WebGUI.Models.Workouts;

public class WorkoutFormModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ParticipantId { get; set; } = 1;

    [Required]
    [StringLength(100)]
    public string ExerciseType { get; set; } = string.Empty;

    [Range(1, 300)]
    public int Duration { get; set; } = 30;

    [Range(0, 1000)]
    public int Repetitions { get; set; } = 10;

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(50)]
    public string Intensity { get; set; } = "Mittel";
}

public class WorkoutUpdateFormModel
{
    [Required]
    public int WorkoutId { get; set; }

    [Required]
    public int ParticipantId { get; set; }

    [Range(1, 300)]
    public int Duration { get; set; }

    [StringLength(50)]
    public string Intensity { get; set; } = "Mittel";
}

public class WorkoutListItemViewModel
{
    public int WorkoutId { get; set; }
    public DateTime Date { get; set; }
    public string ExerciseType { get; set; } = string.Empty;
    public string Intensity { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Calories { get; set; }
}

public class WorkoutManagementViewModel
{
    public int ParticipantId { get; set; } = 1;
    public string? Message { get; set; }
    public string? Error { get; set; }
    public WorkoutFormModel CreateForm { get; set; } = new();
    public List<WorkoutListItemViewModel> Workouts { get; set; } = [];
}

