using Microsoft.Extensions.Configuration;
using Saruman.Core.Helpers;

namespace Saruman.Core.Custom.Obstacles;

public class ThirdObstacle(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IObstacle
{
    private const string BookApiUrl = "https://the-one-api.dev/v2/bok";

    public async Task Execute()
    {
        var urlToUse = configuration["lotr:bookapiurl"] ?? BookApiUrl;

        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync(urlToUse);

        if (!response.IsSuccessStatusCode)
        {
            throw new RemoteBookException(
                $"Failed to get url from BookApi. Url: '{urlToUse}'. Is the env variable 'lotr:bookapiurl' set correct perhaps?");
        }

        var content = await response.Content.ReadAsStringAsync();

        LogHelper.WrapLogOutput(() =>
        {
            Console.WriteLine(
                "Gandalf: Well done, my friends. Though the road was dark and full of peril, you did not falter. You have shown courage greater than sword or spell. Let this be a time of peace—for now.");
            Console.WriteLine($"The ancient books read: '{content}'");
            Console.WriteLine($"You have now passed obstacle no. {Priority()}!");
        });
    }

    public int Priority() => 3;
}

public class RemoteBookException(string message) : Exception(message);