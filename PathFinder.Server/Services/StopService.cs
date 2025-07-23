using PathFinder.Server.Models;
using PathFinder.Server.Repositories.Interfaces;
using PathFinder.Server.Services.Interfaces;
using Serilog;

namespace PathFinder.Server.Services;

public class StopService : IStopService
{
    private readonly IStopRepository _stopRepository;
    private readonly MobilityDbService _mobilityDbService;
    
    public StopService(IStopRepository stopRepository, MobilityDbService mobilityDbService)
    {
        _stopRepository = stopRepository;
        _mobilityDbService = mobilityDbService;
    }
    
    public async Task<List<Stop>> GetAllAsync(string feedId)
    {
        var stops = await _stopRepository.GetByFeedIdAsync(feedId);

        //TODO add expiration date
        if (!stops.Any())
        {
            await _mobilityDbService.DownloadGtfsFeedAsync(feedId);
            var stopsFresh = await _stopRepository.GetByFeedIdAsync(feedId);
            
            if (!stopsFresh.Any())
            {
                Log.Error("No stops found for feed {feedId}", feedId);
            }
            return stopsFresh;
        }
        
        return stops;
    }
}