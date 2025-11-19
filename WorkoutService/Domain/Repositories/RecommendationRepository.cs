using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories;

public class RecommendationRepository : ARepositoryAsync<Recommendation>
{
    public RecommendationRepository(DbContext context) : base(context)
    {
    }
}