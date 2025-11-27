using Domain.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

public abstract class AController<TEntity, TCreateDto, TReadDto ,TUpdateDto> : ControllerBase
    where TEntity : class
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
{
    protected readonly IRepositoryAsync<TEntity> Repository;
    protected readonly ILogger<AController<TEntity, TCreateDto, TReadDto, TUpdateDto>> Logger;

    public AController(IRepositoryAsync<TEntity> repository,
        ILogger<AController<TEntity, TCreateDto, TReadDto, TUpdateDto>> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto record)
    {
        var entity = record.Adapt<TEntity>();
        var data = await Repository.CreateAsync(entity);
        return Ok(data.Adapt<TReadDto>());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TReadDto>> ReadAsync(int id)
    {
        TEntity? data = await Repository.ReadAsync(id);

        if (data is null)
        {
            Logger.LogInformation($"Invalid Request: Entity not present - {id}");
            return NotFound();
        }
        
        Logger.LogInformation($"Sending Entity: {id}");
        return Ok(data.Adapt<TReadDto>());
    }

    [HttpGet]
    public async Task<ActionResult<List<TReadDto>>> ReadAllAsync()
    {
        var data = await Repository.ReadAllAsync();
        var dtos = data.Select(entity => entity.Adapt<TReadDto>()).ToList();
        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, TUpdateDto record)
    {
        TEntity? data = await Repository.ReadAsync(id);

        if (data is null)
        {
            Logger.LogInformation($"Invalid Request: Entity not present - {id}");
            return NotFound();
        }
        await Repository.UpdateAsync(record.Adapt<TEntity>());
        Logger.LogInformation($"Update Entity: {id}");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        TEntity? data = await Repository.ReadAsync(id);
        
        if(data is null)
            return NotFound();
        
        await Repository.DeleteAsync(data);
        Logger.LogInformation($"Delete Entity: {id}");

        return NoContent();
    }
    
    
}