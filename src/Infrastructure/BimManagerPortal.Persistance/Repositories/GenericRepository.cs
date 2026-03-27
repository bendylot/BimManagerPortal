using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Common;
using BimManagerPortal.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BimManagerPortal.Persistance.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
{
    private readonly ApplicationDbContext _dbContext;
 
    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
 
    public IQueryable<T> Entities => _dbContext.Set<T>();
 
    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }
 
    public async Task UpdateAsync(T entity)
    {
        var exist = await _dbContext.Set<T>().FindAsync(entity.Id)
            ?? throw new KeyNotFoundException($"Запись с id '{entity.Id}' не найдена.");
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
    }
 
    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
 
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext
            .Set<T>()
            .ToListAsync();
    }
 
    public async Task<T?> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return default;

        return await _dbContext.Set<T>().FindAsync(guid);
    }
}