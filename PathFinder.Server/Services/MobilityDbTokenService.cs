using System.Text.Json.Serialization;
using PathFinder.Server.Models.DTOs;
using Serilog;

namespace PathFinder.Server.Services;

public class MobilityDbTokenService
{
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private string? _accessToken;
    private string _refreshToken;
    private DateTime _expirationUtc;

    public MobilityDbTokenService(HttpClient httpClient, string refreshToken)
    {
        _refreshToken = refreshToken;
        _httpClient = httpClient;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken != null && _expirationUtc > DateTime.UtcNow)
        {
            return _accessToken;
        }
        Log.Information("Access token expired");

        await _semaphore.WaitAsync();
        try
        {
            if (_accessToken != null && _expirationUtc > DateTime.UtcNow)
            {
                return _accessToken;
            }
            
            var (newToken, expirationUtc) = await RefreshAccessTokenAsync();
            
            _accessToken = newToken;
            _expirationUtc = expirationUtc;

            return newToken;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<(string AccessToken, DateTime ExpirationUtc)> RefreshAccessTokenAsync()
    {
        Log.Information("Refreshing token");
        try
        {
            var requestBody = new { refresh_token = _refreshToken };

            var response = await _httpClient.PostAsJsonAsync("v1/tokens/access", requestBody);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<RefreshTokenResponseDTO>();
            if (responseData == null)
            {
                throw new ApplicationException("Invalid response received from the MobilityDb server.");
            }

            return (responseData.AccessToken, responseData.ExpirationDateTimeUtc);
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Error refreshing access token from MobilityDb API");
            throw;
        }
    }
}