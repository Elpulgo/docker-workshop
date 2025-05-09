using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Saruman.Core.Repository;

public record Character(
    int Id,
    string Name,
    string Race,
    int Age,
    bool IsGood,
    string AppearsInBook);

public class LotrRepository(IConfiguration configuration)
{
    private readonly string _connectionString =
        configuration["lotr:dbconnection"] ?? string.Empty;

    public async Task<List<Character>> GetAllCharacters()
    {
        var characters = new List<Character>();

        await using var conn = await OpenConnection();

        const string query = """
                             
                                         SELECT 
                                             Id, 
                                             Name, 
                                             Race, 
                                             Age, 
                                             IsGood, 
                                             AppearsInBook 
                                         FROM 
                                             dbo.Characters
                             """;

        await using var cmd = new SqlCommand(query, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var character = new Character(
                Id: reader.GetInt32(0),
                Name: reader.GetString(1),
                Race: reader.GetString(2),
                Age: reader.GetInt32(3),
                IsGood: reader.GetBoolean(4),
                AppearsInBook: reader.GetString(5)
            );

            characters.Add(character);
        }

        return characters.OrderByDescending(o => o.Id).ToList();
    }

    public async Task CreateCharacter(Character character)
    {
        await using var conn = await OpenConnection();

        const string query = """
                             
                                         INSERT INTO 
                                             dbo.Characters (Name, Race, Age, IsGood, AppearsInBook)
                                         VALUES 
                                             (@Name, @Race, @Age, @IsGood, @AppearsInBook)
                             """;

        await using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@Name", character.Name);
        cmd.Parameters.AddWithValue("@Race", character.Race);
        cmd.Parameters.AddWithValue("@Age", character.Age);
        cmd.Parameters.AddWithValue("@IsGood", character.IsGood);
        cmd.Parameters.AddWithValue("@AppearsInBook", character.AppearsInBook);

        var insertedRowCount = await cmd.ExecuteNonQueryAsync();

        if (insertedRowCount == 0)
        {
            throw new Exception("Failed to insert a new character to the database");
        }
    }

    private async Task<SqlConnection> OpenConnection()
    {
        SqlConnection? conn = null;
        try
        {
            conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }
        catch
        {
            if (conn != null)
            {
                await conn.DisposeAsync();
            }

            throw;
        }
    }
}