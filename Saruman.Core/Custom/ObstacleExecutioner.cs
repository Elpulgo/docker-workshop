using Microsoft.Extensions.DependencyInjection;

namespace Saruman.Core.Custom;

public static class ObstacleExecutioner
{
    public static async Task SprinkleObstacles(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var obstacles = scope.ServiceProvider.GetRequiredService<IEnumerable<IObstacle>>();

        foreach (var obstacle in obstacles.OrderBy(o => o.Priority()))
        {
            await obstacle.Execute();
        }
    }
}