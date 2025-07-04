using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PathFinder.Server.Data;
using PathFinder.Server.Repositories.Interfaces;
using Serilog;

namespace PathFinder.Server.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    protected BaseRepository(AppDbContext db)
    {
        _context = db;
        Log.Information("BaseRepository initialized for type {EntityType}", typeof(T).Name);
    }

    public T GetById<TId>(TId id)
    {
        Log.Information("Fetching entity of type {EntityType} with ID {Id}", typeof(T).Name, id);
        var entity = _context.Set<T>().Find(id);
        if (entity == null)
        {
            Log.Warning("Entity of type {EntityType} with ID {Id} not found", typeof(T).Name, id);
        }
        return entity;
    }

    public async Task<T?> GetByIdAsync<TId>(TId id)
    {
        Log.Information("Fetching entity of type {EntityType} asynchronously with ID {Id}", typeof(T).Name, id);
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            Log.Warning("Entity of type {EntityType} with ID {Id} not found", typeof(T).Name, id);
        }
        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        Log.Information("Fetching all entities of type {EntityType}", typeof(T).Name);
        return _context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        Log.Information("Fetching all entities of type {EntityType} asynchronously", typeof(T).Name);
        return await _context.Set<T>().ToListAsync();
    }

    public IEnumerable<T> GetWhere(Expression<Func<T, bool>> expression)
    {
        Log.Information("Fetching entities of type {EntityType} with a specific condition", typeof(T).Name);
        return _context.Set<T>().Where(expression).ToList();
    }

    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression)
    {
        Log.Information("Fetching entities of type {EntityType} asynchronously with a specific condition", typeof(T).Name);
        return await _context.Set<T>().Where(expression).ToListAsync();
    }

    public void Add(T entity)
    {
        Log.Information("Adding a new entity of type {EntityType}", typeof(T).Name);
        _context.Set<T>().Add(entity);
    }

    public async Task AddAsync(T entity)
    {
        Log.Information("Adding a new entity of type {EntityType} asynchronously", typeof(T).Name);
        await _context.Set<T>().AddAsync(entity);
    }
    
    public void AddRange(IEnumerable<T> entities)
    {
        Log.Information("Adding multiple entities of type {EntityType}", typeof(T).Name);
        _context.Set<T>().AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        Log.Information("Adding multiple entities of type {EntityType}", typeof(T).Name);
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        Log.Information("Removing an entity of type {EntityType}", typeof(T).Name);
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        Log.Information("Updating an entity of type {EntityType}", typeof(T).Name);
        _context.Set<T>().Update(entity);
    }

    public void BulkUpdate(IEnumerable<T> entity)
    {
        Log.Information("Bulk updating entities of type {EntityType}", typeof(T).Name);
        _context.Set<T>().UpdateRange(entity);
    }

    public int SaveChanges()
    {
        Log.Information("Saving changes to the database");
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        Log.Information("Saving changes to the database asynchronously");
        return await _context.SaveChangesAsync();
    }
}
