namespace GitHub.Api;

public sealed class GitHubService(HttpClient httpClient)
{
    public async Task<GitHubUser?> GetByUserNameAsync(string username)
    {
        var user = await httpClient.GetFromJsonAsync<GitHubUser>($"users/{username}");

        return user;
    }
}
