using System.Net.Http.Headers;
using PathFinder.Server.Services;

namespace PathFinder.Server.Utils;

public class TokenRefreshHandler : DelegatingHandler
{
    private readonly MobilityDbTokenService _tokenService;

    public TokenRefreshHandler(MobilityDbTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}
