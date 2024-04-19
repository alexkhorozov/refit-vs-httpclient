using Refit;

namespace GitHub.Api;

public interface IGitHubApi
{
    [Get("/users/{username}")]
    Task<GitHubUser?> GetByUserNameAsync(string username);

    [Patch("/user")]
    Task<HttpResponseMessage> UpdateBioAsync([Body] UpdateBioRequest request);
}

public class UpdateBioRequest
{
    public string Bio { get; init; }
}