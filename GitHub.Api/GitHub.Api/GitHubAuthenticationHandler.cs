using Microsoft.Extensions.Options;
namespace GitHub.Api;

public class GitHubAuthenticationHandler :DelegatingHandler
{
    private readonly GitHubSettings _settings;

    public GitHubAuthenticationHandler(IOptions<GitHubSettings> options)
    {
        _settings = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {_settings.AccessToken}");
        request.Headers.Add("User-Agent", _settings.UserAgent);

        return base.SendAsync(request, cancellationToken);
    }
}