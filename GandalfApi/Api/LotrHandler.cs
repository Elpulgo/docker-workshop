using Saruman.Core.Custom.LazyObstacles;
using Saruman.Core.Extensions;
using Saruman.Core.Models;
using Saruman.Core.Repository;

namespace GandalfApi.Api;

public class LotrHandler(IConfiguration configuration, LotrRepository lotrRepository)
{
    public static IList<string> GetSomeHobbits()
    {
        var hobbits = new List<string>
        {
            "Frodo Baggins",
            "Samwise Gamgee",
            "Meriadoc Brandybuck (Merry)",
            "Peregrin Took (Pippin)",
            "Bilbo Baggins",
            "Rosie Cotton",
            "Lobelia Sackville-Baggins",
            "Hamfast Gamgee (Gaffer)",
            "Fatty Bolger (Fredegar Bolger)",
            "Paladin Took (Pippin’s father)"
        };

        return hobbits;
    }

    public static async Task<UrukModel> GetUrukName(string input) =>
        await UrukObstacle.Execute(input);

    public async Task<IResult> GetCharacters()
    {
        if (!configuration.IsDbInUse())
        {
            return Results.BadRequest("Need to use a Middle earth database for this endpoint :( Hint: Add configuration lotr:usedb");
        }

        var characters = await lotrRepository.GetAllCharacters();
        return Results.Ok(characters);
    }

    public async Task<IResult> CreateCharacter(Character character)
    {
        if (!configuration.IsDbInUse())
        {
            return Results.BadRequest("Need to use a Middle earth database for this endpoint :( Hint: Add configuration lotr:usedb");
        }

        await lotrRepository.CreateCharacter(character);
        return Results.Ok();
    }
}