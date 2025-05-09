using Saruman.Core.Models;

namespace Saruman.Core.Custom.LazyObstacles;

public class UrukObstacle
{
    public static Task<UrukModel> Execute(string input = "Uruk")
    {
        var parts = input.Split(' ');
        var orcFirstName = parts[0][0].ToString();
        var orcLastName = parts[1][0].ToString();

        return Task.FromResult(new UrukModel
        {
            Data = GetOrcFullName(orcFirstName, orcLastName),
            Obstacle = nameof(UrukObstacle),
            GandalfQuote =
                "You did not seek glory, and yet you have earned it. That, my dear friend, is what makes a true hero."
        });
    }

    private static string GetOrcFullName(string firstName, string lastName) =>
        $"{firstName} 'The terror of Angband' {lastName}";
}