using GitHub.Api;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<GitHubSettings>()
    .BindConfiguration(GitHubSettings.ConfigurationSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddTransient<GitHubAuthenticationHandler>();

builder.Services.AddHttpClient<GitHubService>((sp, httpClient) =>
{
    var settings = sp.GetRequiredService<IOptions<GitHubSettings>>().Value;
    httpClient.BaseAddress = new Uri(settings.BaseAddress);

}).AddHttpMessageHandler<GitHubAuthenticationHandler>();

builder.Services.AddRefitClient<IGitHubApi>()
    .ConfigureHttpClient((sp, httpClient) =>
    {
        var settings = sp.GetRequiredService<IOptions<GitHubSettings>>().Value;
        httpClient.BaseAddress = new Uri(settings.BaseAddress);
    }).AddHttpMessageHandler<GitHubAuthenticationHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/users/{username}", async (IGitHubApi service, string username) =>
{
    var user = await service.GetByUserNameAsync(username);

    return Results.Ok(user);
});

app.MapPatch("/users/bio", async (IGitHubApi service, UpdateBioRequest request) =>
{
    var response = await service.UpdateBioAsync(request);
    response.EnsureSuccessStatusCode();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();
