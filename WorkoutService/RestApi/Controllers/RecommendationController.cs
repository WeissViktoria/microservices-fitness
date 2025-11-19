using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace RestApi.Controllers;

[ApiController]
[Route("[recommendation]")]
public class RecommendationController : AController<Recommendation>
{
    public RecommendationController(IRepositoryAsync<Recommendation> repository,
        ILogger<AController<Recommendation>> logger) : base(repository, logger)
    {
        
    }
}