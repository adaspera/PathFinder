using System.Linq.Expressions;

namespace PathFinder.Server.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    public T GetById<TId>(TId id);
    public Task<T?> GetByIdAsync<TId>(TId id);
    public IEnumerable<T> GetAll();
    public Task<IEnumerable<T>> GetAllAsync();
    public IEnumerable<T> GetWhere(Expression<Func<T, bool>> expression);
    public Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);
    public void Add(T entity);
    public Task AddAsync(T entity);
    public void AddRange(IEnumerable<T> entities);
    public Task AddRangeAsync(IEnumerable<T> entities);
    public void Remove(T entity);
    public void Update(T entity);
    public void BulkUpdate(IEnumerable<T> entity);
    public int SaveChanges();
    public Task<int> SaveChangesAsync();
}