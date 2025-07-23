using PathFinder.Server.Repositories.Interfaces;
using PathFinder.Server.Services.Interfaces;
using Serilog;

namespace PathFinder.Server.Services;

public class RouteService : IRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly MobilityDbService _mobilityDbService;

    public RouteService(IRouteRepository routeRepository, MobilityDbService mobilityDbService)
    {
        _routeRepository = routeRepository;
        _mobilityDbService = mobilityDbService;
    }
    public async Task<List<Models.Route>> GetAllAsync(string feedId)
    {
        var routes = await _routeRepository.GetByFeedIdAsync(feedId);

        //TODO add expiration date
        if (!routes.Any())
        {
            await _mobilityDbService.DownloadGtfsFeedAsync(feedId);
            var routesFresh = await _routeRepository.GetByFeedIdAsync(feedId);
            
            if (!routesFresh.Any())
            {
                Log.Error("No stops found for feed {feedId}", feedId);
            }
            return routesFresh;
        }
        
        return routes;
    }
}