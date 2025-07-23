using Microsoft.EntityFrameworkCore;
using PathFinder.Server.Data;

namespace PathFinder.Server.Repositories.Interfaces;

public class RouteRepository : BaseRepository<Models.Route>, IRouteRepository
{
    private readonly AppDbContext _appDbContext;
    
    public RouteRepository(AppDbContext db) : base(db)
    {
        _appDbContext = db;
    }
    
    public async Task<List<Models.Route>> GetByFeedIdAsync(string feedId)
    {
        return await _appDbContext.Routes.Where(s => s.FeedId == feedId).ToListAsync();
    }
}