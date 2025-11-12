using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class RecommendationContext : DbContext
{
    public RecommendationContext(DbContextOptions<WorkoutContext> options) : base(options) { }
    
    public DbSet<Recommendation> Recommendation { get; set; }
}