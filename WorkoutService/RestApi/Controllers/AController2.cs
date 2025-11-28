using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

public class AController2<TEntity> : ControllerBase where TEntity : class
{
    private IRepositoryAsync<TEntity> _repository;
    private ILogger<AController2<TEntity>> _logger;

    public AController2(IRepositoryAsync<TEntity> repository, ILogger<AController2<TEntity>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<TEntity>> Create(TEntity t)
    {
        await _repository.CreateAsync(t);
        _logger.LogInformation($"Created entity: {t}");
        return t;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TEntity>> Read(int id)
    {
        var data = await _repository.ReadAsync(id);
        
        if(data is null) return NotFound();
        _logger.LogInformation($"reading entity with id {id}");
        
        return Ok(data);
    }

    [HttpGet]
    public async Task<ActionResult<List<TEntity>>> ReadAll()
    {
        return Ok(await _repository.ReadAllAsync());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TEntity>> Update(int id, TEntity t)
    {
        var data = await _repository.ReadAsync(id);
        
        if(data is null) return NotFound();

        await _repository.UpdateAsync(t);
        _logger.LogInformation($"updated entity with id {id}");
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var entity = await _repository.ReadAsync(id);
        if(entity is null) return NotFound();
        await _repository.DeleteAsync(entity);
        _logger.LogInformation($"deleted entity with id {id}");
        return NoContent();
    }
}