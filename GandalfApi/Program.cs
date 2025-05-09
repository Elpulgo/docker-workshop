using GandalfApi.Api;
using Saruman.Core.Custom;
using Saruman.Core.Custom.LazyObstacles;
using Saruman.Core.Extensions;
using Saruman.Core.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<LotrHandler>();
builder.Services.AddScoped<UrukObstacle>();
builder.Services.AddScoped<LotrRepository>();

builder.Services.AddAllImplementationsOfInterface<IObstacle>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await ObstacleExecutioner.SprinkleObstacles(app.Services);
await app.Configuration.InitializeMiddleEarthDatabaseAsync();

app.MapGet("/api/hobbits", LotrHandler.GetSomeHobbits)
    .WithName("GetMeSomeHobbits")
    .WithTags("Easy")
    .WithOpenApi();

app.MapGet("/api/urukname/{initials}", async (string initials)
        => await LotrHandler.GetUrukName(initials))
    .WithName("GetUrukName From Initials")
    .WithTags("Hard")
    .WithOpenApi();

app.MapGet("/api/characters", async (LotrHandler handler)
        => await handler.GetCharacters())
    .WithTags("Hardcore")
    .WithOpenApi();

app.MapPost("/api/characters", async (LotrHandler handler, Character character)
        => await handler.CreateCharacter(character))
    .WithTags("Hardcore")
    .WithOpenApi();

await app.RunAsync();