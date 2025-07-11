using Microsoft.EntityFrameworkCore;
using PathFinder.Server.Data;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories.Interfaces;

namespace PathFinder.Server.Repositories;

public class StopRepository : BaseRepository<Stop>, IStopRepository
{
    AppDbContext _appDbContext;
    public StopRepository(AppDbContext db) : base(db)
    {
        _appDbContext = db;
    }

    public async Task<List<Stop>> GetByFeedIdAsync(string feedId)
    {
        return await _appDbContext.Stops.Where(s => s.FeedId == feedId).ToListAsync();
    }
}