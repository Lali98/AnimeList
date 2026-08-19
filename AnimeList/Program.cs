using AnimeList.Commands;
using AnimeList.Data;
using AnimeList.Mapping;
using AnimeList.Models;
using AnimeList.Service;
using AnimeList.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            )
        };
    });

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

//Services
builder.Services.AddScoped<AnimeImportService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IFanSubService, FanSubService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UpdateAnimeService>();

//Commands
builder.Services.AddScoped<ImportCommand>();
builder.Services.AddScoped<UpdateCommand>();

//Mapperes
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
            var updateCommand = scope.ServiceProvider.GetRequiredService<UpdateCommand>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
