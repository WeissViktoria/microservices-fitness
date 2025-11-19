using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace RestApi.Controllers;

[ApiController]
[Route("[analytic]")]
public class AnalyticController : AController<Analytics>
{
    public AnalyticController(IRepositoryAsync<Analytics> repository,
        ILogger<AController<Analytics>> logger) : base(repository, logger)
    {
        
    }
}