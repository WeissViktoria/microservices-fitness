using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories;

public class AnalyticRepository: ARepositoryAsync<Analytics>
{
    public AnalyticRepository(DbContext context) : base(context)
    {
    }
}