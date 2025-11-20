using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;

namespace RestApi.Controllers;


[ApiController]
[Route("[controller]")]
public class WorkoutController : AController<Workout, CreateWorkoutDto, 
    ReadWorkoutDto, UpdateWorkoutDto>
{
    public WorkoutController(IRepositoryAsync<Workout> repository,
        ILogger<AController<Workout, CreateWorkoutDto, ReadWorkoutDto, UpdateWorkoutDto>> logger) : base(repository, logger)
    {
        
    }
}