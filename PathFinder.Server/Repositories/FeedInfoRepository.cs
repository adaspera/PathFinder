using PathFinder.Server.Data;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories.Interfaces;

namespace PathFinder.Server.Repositories;

public class FeedInfoRepository : BaseRepository<FeedInfo>, IFeedInfoRepository
{
    public FeedInfoRepository(AppDbContext db) : base(db)
    {
    }
}