using System.Text;
using backend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using dotenv.net;
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
        
        Console.WriteLine(Program.config["OpenAi:Key"]);
        Console.WriteLine(Environment.GetEnvironmentVariable("OPENAI_KEY"));
        Console.WriteLine($"Connection String: {builder.Configuration.GetConnectionString("DefaultConnection")}");
        
        var mailHost = Environment.GetEnvironmentVariable("MAIL_HOST");
        var mailPort = Environment.GetEnvironmentVariable("MAIL_PORT");
        var mailAddress = Environment.GetEnvironmentVariable("MAIL_ADDRESS");
        var mailKey = Environment.GetEnvironmentVariable("MAIL_KEY");
        
        mailHost = config["Mail:Host"];
        
        Console.WriteLine($"Mail Host: {mailHost}");
        Console.WriteLine($"Mail Port: {mailPort}");
        Console.WriteLine($"Mail Address: {mailAddress}");
        Console.WriteLine($"Mail Key: {mailKey}");
        
        builder.Services.AddDbContext<DataContext>(options
            => options
                .UseNpgsql(config["DB_CONNECTION_STRING"])
                .UseSnakeCaseNamingConvention());

        builder.Configuration.AddJsonFile("secrets.json",
            optional: true,
            reloadOnChange: true);

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = builder.Configuration["Jwt:Issuer"],
                    ValidAudience            = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            //options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin"));
        });

        var app = builder.Build();

        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }

        // TODO: change before production
        app.UseCors(b => b
            .WithOrigins("http://localhost:44398", "http://localhost:5260", "http://localhost:8080", "http://localhost:5003")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            devMode = true;
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        AiUtil.GetInstance();

        await app.RunAsync();
    }
}