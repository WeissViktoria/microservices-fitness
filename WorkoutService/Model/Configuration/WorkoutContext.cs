using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class WorkoutContext : DbContext
{
    public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { }
    
    public DbSet<Workout> Workouts { get; set; }
}