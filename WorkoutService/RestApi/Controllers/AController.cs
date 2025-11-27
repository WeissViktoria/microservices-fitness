using Domain.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[ApiController]
public abstract class AController<TEntity, TCreateDto, TReadDto, TUpdateDto> : ControllerBase
    where TEntity : class
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
        var saved = await Repository.CreateAsync(entity);
        return Ok(saved.Adapt<TReadDto>());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TReadDto>> ReadAsync(int id)
    {
        var existing = await Repository.ReadAsync(id);
        if (existing is null)
            return NotFound();

        return Ok(existing.Adapt<TReadDto>());
    }

    [HttpGet]
    public async Task<ActionResult<List<TReadDto>>> ReadAllAsync()
    {
        var data = await Repository.ReadAllAsync();
        return Ok(data.Select(e => e.Adapt<TReadDto>()).ToList());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, TUpdateDto record)
    {
        var existing = await Repository.ReadAsync(id);
        if (existing is null)
            return NotFound();

        record.Adapt(existing);
        await Repository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var existing = await Repository.ReadAsync(id);
        if (existing is null)
            return NotFound();

        await Repository.DeleteAsync(existing);
        return NoContent();
    }
}