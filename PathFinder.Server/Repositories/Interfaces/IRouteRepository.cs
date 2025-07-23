namespace PathFinder.Server.Repositories.Interfaces;

public interface IRouteRepository : IBaseRepository<Models.Route>
{
    Task<List<Models.Route>> GetByFeedIdAsync(string feedId);
}