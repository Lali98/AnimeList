using AnimeList.Commands;
using AnimeList.Data;
using AnimeList.Service;
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

builder.Services.AddScoped<MalApiService>();
builder.Services.AddScoped<AnimeImportService>();

builder.Services.AddScoped<ImportCommand>();

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
