namespace PathFinder.Server.Services.Interfaces;

public interface IGtfsService<T>
{
    Task<List<T>> GetAllAsync(string feedId);
}