using Saruman.Core.Helpers;

namespace Saruman.Core.Custom.Obstacles;

public class SecondObstacle : IObstacle
{
    public async Task Execute()
    {
        var content = await File.ReadAllTextAsync("/foo/bar/foobar.txt");

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new AncientScrollException("The ancient scroll of 'foobar.txt' has no content? :(");
        }

        LogHelper.WrapLogOutput(() =>
        {
            Console.WriteLine(
                "Gandalf: The task is done, though the echoes of it may linger. Rest now, for not all storms are meant to be weathered twice.");
            Console.WriteLine($"The content of the ancient scroll reads: '{content}'");

            Console.WriteLine($"You have now passed obstacle no. {Priority()}!");
        });
    }

    public int Priority() => 2;
}

public class AncientScrollException(string message) : Exception(message);
