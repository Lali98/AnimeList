using AnimeList.Commands;
using AnimeList.Data;
using AnimeList.Mapping;
using AnimeList.Service;
using AnimeList.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddScoped<MalApiService>();
builder.Services.AddHttpClient<MalApiService>(client =>
{
    var malClientId = builder.Configuration["MyAnimeList:ClientId"];
    client.BaseAddress = new Uri("https://api.myanimelist.net/v2/");
    client.DefaultRequestHeaders.Add("User-Agent", "AnimeList/1.0");
    client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", malClientId);
});

builder.Services.AddScoped<AnimeImportService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();

builder.Services.AddScoped<ImportCommand>();
builder.Services.AddScoped<UpdateScoreCommand>();

builder.Services.AddScoped<AnimeMapper>();

var app = builder.Build();

if (args.Length > 0)
{
    using var scope = app.Services.CreateScope();

    switch(args[0].ToLower())
    {
        case "import":
            var importCommand = scope.ServiceProvider.GetRequiredService<ImportCommand>();
            await importCommand.ExecuteAsync(args);
            break;
        case "update":
            var updateCommand = scope.ServiceProvider.GetRequiredService<UpdateScoreCommand>();
            await updateCommand.ExecuteAsync(args);
            break;
        default:
            Console.WriteLine("Ismeretlen parancs.");
            break;
    }
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference("/docs");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
