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
        return await _httpClient.GetFromJsonAsync<object>("feeds");
    }

    public async Task<object?> GetFeedInfoAsync(string feedId)
    {
        return await _httpClient.GetFromJsonAsync<object>($"feeds/{feedId}");
    }
    
    public async Task<object?> GetMetadata()
    {
        return await _httpClient.GetFromJsonAsync<object>("v1/metadata");
    }

}
