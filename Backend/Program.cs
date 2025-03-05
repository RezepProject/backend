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

            // Add FluentValidation validators from assemblies
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
            builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleValidator>();
            

            // Set configuration and check unit test mode
            config = builder.Configuration;
            var isUnitTest = config["AppSettings:UnitTestMode"]?.ToLower() == "true";
            if (isUnitTest)
            {
                Console.WriteLine("Running in unit test mode");
            }

            // Get the connection string based on the mode
            var connectionString = isUnitTest
                ? config.GetConnectionString("TestConnection")
                : config.GetConnectionString("DefaultConnection");

            // Debugging: Print connection string components (excluding password for security reasons)
            PrintConnectionStringDetails(connectionString);

            // Add DbContext and configure routing
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(connectionString) // Use the selected connection string
                       .UseSnakeCaseNamingConvention());
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();

            // Set up Swagger/OpenAPI documentation
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
                ConfigureJwtAuthentication(builder);
            }

            var app = builder.Build();

            // Apply migrations if not in unit test mode
            if (!isUnitTest)
            {
                await ApplyDatabaseMigrations(app);
            }

            // Configure CORS
            ConfigureCors(app);

            // Set up the HTTP request pipeline
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

        // Helper method to print connection string details for debugging
        private static void PrintConnectionStringDetails(string connectionString)
        {
            var user = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Username="))?.Split("=")[1];
            var password = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Password="))?.Split("=")[1];
            var port = connectionString.Split(";").FirstOrDefault(x => x.StartsWith("Port="))?.Split("=")[1];

            Console.WriteLine($"Connecting to database with user: {user}, password: {password}, port: {port}");
        }

        // Helper method to configure JWT authentication
        private static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
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
                // Add authorization policies here if needed
            });
        }

        // Helper method to apply database migrations
        private static async Task ApplyDatabaseMigrations(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                await using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync(); // Apply all pending migrations
            }
        }

        // Helper method to configure CORS
        private static void ConfigureCors(WebApplication app)
        {
            app.UseCors(b => b
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
        }
    }
}
