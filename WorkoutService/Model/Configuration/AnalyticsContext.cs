using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) {}

    public DbSet<History> History { get; set; }
}
