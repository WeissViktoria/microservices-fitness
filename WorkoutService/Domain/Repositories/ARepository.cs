using Domain.Interfaces;

namespace Domain.Repositories;

public abstract class ARepository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
{
    
}