using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories;

public class WorkoutRepository : ARepositoryAsync<Workout>
{
    public WorkoutRepository(DbContext context) : base(context)
    {
    }
}