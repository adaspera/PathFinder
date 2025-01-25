namespace PathFinder.Server.Services;


public class MobilityDbService
{
    private readonly HttpClient _httpClient;

    public MobilityDbService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<object?> GetAllFeedsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<object>("feeds");
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException("Error fetching feeds from MobilityDb API", ex);
        }
    }

    public async Task<object?> GetFeedInfoAsync(string feedId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<object>($"feeds/{feedId}");
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions, e.g., log errors
            throw new ApplicationException($"Error fetching feed {feedId} from MobilityDb API", ex);
        }
    }
    
    public async Task<object?> GetMetadata()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<object>("v1/metadata");
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException($"Error fetching metadata from MobilityDb API", ex);
        }
    }

}
