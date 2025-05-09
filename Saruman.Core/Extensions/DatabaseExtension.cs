using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Saruman.Core.Extensions;

public static class DatabaseExtension
{
    public static async Task InitializeMiddleEarthDatabaseAsync(this IConfiguration configuration)
    {
        var useDb = configuration["lotr:usedb"];

        if (useDb is null)
        {
            return;
        }

        var fullConnStr = configuration["lotr:dbconnection"];

        if (string.IsNullOrWhiteSpace(fullConnStr))
        {
            throw new MiddleEarthDatabaseException("Missing 'lotr:dbconnection' in configuration.");
        }

        var builder = new SqlConnectionStringBuilder(fullConnStr);
        var initialCatalog = builder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(initialCatalog))
        {
            throw new MiddleEarthDatabaseException(
                "The connection string must specify InitialCatalog (database name).");
        }

        await CreateDatabaseIfNotExists(builder, initialCatalog);
        await using var dbConn = await CreateTableIfNotExists(builder, initialCatalog);
        await SeedIfApplicable(dbConn);
    }

    private static async Task CreateDatabaseIfNotExists(SqlConnectionStringBuilder builder, string initialCatalog)
    {
        // Connect to master to ensure DB exists
        builder.InitialCatalog = "master";
        var masterConnStr = builder.ConnectionString;

        await using var masterConn = new SqlConnection(masterConnStr);

        var counter = 0;
        const int maxCount = 10;
        var isDbReady = false;
        while (!isDbReady && counter < maxCount)
        {
            try
            {
                await masterConn.OpenAsync();
                isDbReady = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Middle Earth db is not up yet, wait a bit and try again.. Try: {counter}/{maxCount}");
                
                await Task.Delay(1500);
            }

            counter++;
        }

        if (!isDbReady)
        {
            Console.WriteLine($"Tried connecting to Middle Earth db {maxCount} times. Will not stand for this..");
            throw new Exception("I'm tired..");
        }

        var checkDbCmd = $"IF DB_ID(N'{initialCatalog}') IS NULL CREATE DATABASE [{initialCatalog}];";
        await using var cmd = new SqlCommand(checkDbCmd, masterConn);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task<SqlConnection> CreateTableIfNotExists(SqlConnectionStringBuilder builder,
        string initialCatalog)
    {
        SqlConnection? dbConn = null;
        try
        {
            builder.InitialCatalog = initialCatalog;
            var dbConnStr = builder.ConnectionString;

            dbConn = new SqlConnection(dbConnStr);
            await dbConn.OpenAsync();

            const string createTableCmd = @"
            IF OBJECT_ID(N'dbo.Characters', N'U') IS NULL
            CREATE TABLE dbo.Characters (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(100),
                Race NVARCHAR(50),
                Age INT,
                IsGood BIT,
                AppearsInBook NVARCHAR(100)
            );";

            await using var cmd = new SqlCommand(createTableCmd, dbConn);
            await cmd.ExecuteNonQueryAsync();

            return dbConn;
        }
        catch
        {
            if (dbConn != null)
            {
                await dbConn.DisposeAsync();
            }

            throw;
        }
    }

    private static async Task SeedIfApplicable(SqlConnection dbConn)
    {
        var countCmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Characters", dbConn);
        var count = (int)(await countCmd.ExecuteScalarAsync() ?? 0);
        if (count > 0)
        {
            return;
        }

        const string seedCmd = @"
            INSERT INTO dbo.Characters (Name, Race, Age, IsGood, AppearsInBook)
            VALUES
           ('Frodo Baggins', 'Hobbit', 50, 1, 'The Fellowship of the Ring'),
           ('Gandalf', 'Maia', 2019, 1, 'The Fellowship of the Ring'),
           ('Sauron', 'Maia', 3000, 0, 'The Silmarillion'),
           ('Aragorn', 'Human', 87, 1, 'The Two Towers'),
           ('Gollum', 'Hobbit', 589, 0, 'The Two Towers'),
           ('Legolas', 'Elf', 2931, 1, 'The Two Towers'),
           ('Gimli', 'Dwarf', 139, 1, 'The Fellowship of the Ring'),
           ('Boromir', 'Human', 41, 1, 'The Fellowship of the Ring'),
           ('Samwise Gamgee', 'Hobbit', 38, 1, 'The Fellowship of the Ring'),
           ('Meriadoc Brandybuck', 'Hobbit', 36, 1, 'The Fellowship of the Ring'),
           ('Peregrin Took', 'Hobbit', 28, 1, 'The Two Towers'),
           ('Saruman', 'Maia', 2019, 0, 'The Two Towers'),
           ('Elrond', 'Elf', 6520, 1, 'The Fellowship of the Ring'),
           ('Galadriel', 'Elf', 8372, 1, 'The Fellowship of the Ring'),
           ('Bilbo Baggins', 'Hobbit', 111, 1, 'The Hobbit'),
           ('Théoden', 'Human', 71, 1, 'The Two Towers'),
           ('Éomer', 'Human', 29, 1, 'The Return of the King'),
           ('Éowyn', 'Human', 24, 1, 'The Return of the King'),
           ('Denethor', 'Human', 89, 0, 'The Return of the King'),
           ('Faramir', 'Human', 36, 1, 'The Two Towers'),
           ('Treebeard', 'Ent', 17000, 1, 'The Two Towers'),
           ('Gríma Wormtongue', 'Human', 45, 0, 'The Two Towers'),
           ('Beregond', 'Human', 40, 1, 'The Return of the King'),
           ('Isildur', 'Human', 234, 1, 'The Silmarillion'),
           ('Anárion', 'Human', 200, 1, 'The Silmarillion'),
           ('Gil-galad', 'Elf', 3000, 1, 'The Silmarillion'),
           ('Celebrimbor', 'Elf', 2000, 1, 'The Silmarillion'),
           ('Finrod Felagund', 'Elf', 5000, 1, 'The Silmarillion'),
           ('Melkor (Morgoth)', 'Ainur', 99999, 0, 'The Silmarillion'),
           ('Ungoliant', 'Unknown', 9999, 0, 'The Silmarillion'),
           ('Beren', 'Human', 60, 1, 'The Silmarillion'),
           ('Lúthien', 'Half-Elf', 3000, 1, 'The Silmarillion'),
           ('Húrin', 'Human', 60, 1, 'The Silmarillion'),
           ('Túrin Turambar', 'Human', 35, 1, 'The Silmarillion'),
           ('Glorfindel', 'Elf', 6000, 1, 'The Fellowship of the Ring'),
           ('Arwen', 'Half-Elf', 2778, 1, 'The Return of the King'),
           ('Tom Bombadil', 'Unknown', 99999, 1, 'The Fellowship of the Ring'),
           ('Goldberry', 'Unknown', 9999, 1, 'The Fellowship of the Ring'),
           ('Bard the Bowman', 'Human', 35, 1, 'The Hobbit'),
           ('Thranduil', 'Elf', 4000, 1, 'The Hobbit'),
           ('Dáin Ironfoot', 'Dwarf', 252, 1, 'The Hobbit'),
           ('Azog', 'Orc', 140, 0, 'The Hobbit'),
           ('Bolg', 'Orc', 120, 0, 'The Hobbit'),
           ('Radagast', 'Maia', 2019, 1, 'The Hobbit'),
           ('Beorn', 'Skin-changer', 100, 1, 'The Hobbit'),
           ('Smaug', 'Dragon', 600, 0, 'The Hobbit'),
           ('Shelob', 'Spider', 10000, 0, 'The Two Towers'),
           ('Círdan', 'Elf', 10000, 1, 'The Silmarillion'),
           ('Gothmog', 'Balrog', 9999, 0, 'The Silmarillion');";

        await using var cmd = new SqlCommand(seedCmd, dbConn);
        await cmd.ExecuteNonQueryAsync();
    }

    public static bool IsDbInUse(this IConfiguration configuration) => configuration["lotr:usedb"] is not null;
}

public class MiddleEarthDatabaseException(string message) : Exception(message);