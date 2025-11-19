using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace RestApi.Controllers;


[ApiController]
[Route("[controller]")]
public class WorkoutController : AController<Workout>
{
    public WorkoutController(IRepositoryAsync<Workout> repository,
        ILogger<AController<Workout>> logger) : base(repository, logger)
    {
        
    }
}