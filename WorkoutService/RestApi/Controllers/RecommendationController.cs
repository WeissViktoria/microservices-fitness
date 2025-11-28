using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;

namespace RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController 
    : AController<Recommendation, CreateRecommendationDto, ReadRecommendationDto, UpdateRecommendationDto>
{
    public RecommendationController(
        IRepositoryAsync<Recommendation> repository,
        ILogger<AController<Recommendation, CreateRecommendationDto, ReadRecommendationDto, UpdateRecommendationDto>> logger
    ) : base(repository, logger)
    {
    }
}