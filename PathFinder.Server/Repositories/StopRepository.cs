using PathFinder.Server.Data;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories.Interfaces;

namespace PathFinder.Server.Repositories;

public class StopRepository : BaseRepository<Stop>, IStopRepository
{
    public StopRepository(AppDbContext db) : base(db)
    {
    }
}