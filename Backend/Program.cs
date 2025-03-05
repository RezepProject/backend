using System.Text;
using backend.Controllers.Validators;
using backend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using dotenv.net;
using FluentValidation;

namespace backend
{
    public class Program
    {
        public static IConfiguration config;
        public static bool devMode;

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();
            
            builder.Services.AddValidatorsFromAssemblyContaining<CreateAnswerValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateAnswerValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateBackgroundImageValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ConfigValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ConfigUserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ChangeConfigUserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionCategoryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateTaskValidator>();
            
            config = builder.Configuration;

            // Check for unit test mode using appsettings
            var isUnitTest = config["AppSettings:UnitTestMode"]?.ToLower() == "true";

            if (isUnitTest)
            {
                Console.WriteLine("Running in unit test mode");
            }

            // Get the connection string based on the mode
            var connectionString = isUnitTest
                ? config.GetConnectionString("TestConnection")
                : config.GetConnectionString("DefaultConnection");

            // Print connection string components for debugging (except the password for security reasons)
            var user = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Username="))?.Split("=")[1];
            var password = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Password="))?.Split("=")[1];
            var port = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Port="))?.Split("=")[1];

            Console.WriteLine($"Connecting to database with user: {user}, password: {password}, port: {port}");

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(connectionString) // Use the selected connection string
                       .UseSnakeCaseNamingConvention());

            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
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
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Conditionally add JWT authentication based on test mode
            if (!isUnitTest)
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = SecretsProvider.Instance.JwtIssuer,
                            ValidAudience = SecretsProvider.Instance.JwtAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretsProvider.Instance.JwtKey))
                        };
                    });

                builder.Services.AddAuthorization(options =>
                {
                    // You can add authorization policies here if necessary
                    // For example: options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin"));
                });
            }

            var app = builder.Build();

            // Only apply migrations if not in unit test mode
            if (!isUnitTest)
            {
                using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    await context.Database.MigrateAsync(); // Apply all pending migrations
                }
            }

            // TODO: change before production
            app.UseCors(b => b
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                devMode = true;
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.MapControllers();

            AiUtil.GetInstance(); // Assuming this initializes a single instance of a utility or configuration
            await app.RunAsync();
        }
    }
}
