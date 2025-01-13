using System.Text;
using backend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace backend;

public static class Program
{
    public static IConfiguration config;
    public static bool devMode;

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
        config = builder.Configuration;

        // Datenbankkontext konfigurieren
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(config["DB_CONNECTION_STRING"])
                   .UseSnakeCaseNamingConvention());

        // Routing und Controller
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();

        // CORS konfigurieren
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Swagger für API-Dokumentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Type         = SecuritySchemeType.ApiKey,
                Scheme       = "Bearer",
                BearerFormat = "JWT",
                In           = ParameterLocation.Header,
                Description  = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                               "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                               "Example: \"Bearer {token}\""
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // JWT-Authentifizierung
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = SecretsProvider.Instance.JwtIssuer,
                    ValidAudience            = SecretsProvider.Instance.JwtAudience,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretsProvider.Instance.JwtKey))
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Datenbankmigrationen
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }

        // CORS aktivieren
        app.UseCors("AllowAll");

        // Entwicklungsmodus erkennen
        if (app.Environment.IsDevelopment())
        {
            devMode = true;
        }

        // Swagger aktivieren
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        // Authentifizierung und Autorisierung
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // AI-Utility initialisieren
        AiUtil.GetInstance();

        await app.RunAsync();
    }
}
