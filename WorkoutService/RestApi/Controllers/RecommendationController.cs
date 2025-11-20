using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;

namespace RestApi.Controllers;

[ApiController]
[Route("[recommendation]")]
public class RecommendationController : AController<Recommendation, ReadRecommendationDto, 
    CreateRecommendationDto, UpdateRecommendationDto>
{
    public RecommendationController(IRepositoryAsync<Recommendation> repository,
        ILogger<AController<Recommendation, ReadRecommendationDto, 
            CreateRecommendationDto, UpdateRecommendationDto>> logger) : base(repository, logger)
    {
        
    }
}