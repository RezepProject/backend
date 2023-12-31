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

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        config = builder.Configuration;

        builder.Services.AddDbContext<DataContext>(options
                                                       => options
                                                          .UseNpgsql(builder.Configuration["ConnectionString"])
                                                          .UseSnakeCaseNamingConvention());

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] + ""))
            };
        });

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = @"JWT Authorization header using the Bearer scheme. <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br />
                      Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }

        // TODO: change before production
        app.UseCors(b => b
                         .WithOrigins("http://localhost:44398", "http://localhost:5260")
                         .AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Use(async (context, next) =>
        {
            var code = await AuthenticationUtils.AuthorizeUser(app, context);
            if (code == 301) await next(context);
            else context.Response.StatusCode = code;
        });

        app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}