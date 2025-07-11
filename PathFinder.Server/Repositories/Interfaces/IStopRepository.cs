using PathFinder.Server.Models;

namespace PathFinder.Server.Repositories.Interfaces;

public interface IStopRepository : IBaseRepository<Stop>
{
    Task<List<Stop>> GetByFeedIdAsync(string feedId);
}