using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) {}

    public DbSet<Workout> Workouts { get; set; }
}
