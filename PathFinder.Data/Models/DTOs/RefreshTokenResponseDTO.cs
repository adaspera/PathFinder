using System.Text.Json.Serialization;

namespace PathFinder.Server.Models.DTOs;

public class RefreshTokenResponseDTO
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expiration_datetime_utc")]
    public DateTime ExpirationDateTimeUtc { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}