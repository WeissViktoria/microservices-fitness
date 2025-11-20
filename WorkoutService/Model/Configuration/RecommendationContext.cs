using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class RecommendationDbContext : DbContext
{
    public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options) : base(options) {}

    public DbSet<Recommendation> Recommendations { get; set; }
}
