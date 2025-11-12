using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class AnalyticsContext: DbContext
{
    public AnalyticsContext(DbContextOptions<WorkoutContext> options) : base(options) { }
    
    public DbSet<Analytics> Analytics { get; set; }
}