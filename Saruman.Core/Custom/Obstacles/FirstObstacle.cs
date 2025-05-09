using Microsoft.Extensions.Configuration;
using Saruman.Core.Helpers;

namespace Saruman.Core.Custom.Obstacles;

public class FirstObstacle(IConfiguration configuration) : IObstacle
{
    public Task Execute()
    {
        var frodoValue = configuration["lotr:frodo"];

        if (frodoValue is null)
        {
            throw new InvalidOperationException("Configuration 'lotr:frodo' is required.");
        }

        LogHelper.WrapLogOutput(() =>
        {
            Console.WriteLine(
                "Gandalf: You have walked through shadow and fire and emerged stronger. The world is changed by such deeds, though few will ever know.");
            Console.WriteLine($"You have now passed obstacle no. {Priority()}!");
        });

        return Task.CompletedTask;
    }

    public int Priority() => 1;
}