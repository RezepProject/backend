using Shared;

const string CorsPolicyName = "AllowOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddCors(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicyName);

app.MapPost("samples", (SampleRequest request) =>
{
    // in real app would be e.g. in a middleware
    var validator = new SampleRequest.Validator();
    var validationResult = validator.Validate(request);

    if (!validationResult.IsValid)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.Run();



static void AddCors(IServiceCollection services)
{
    var clientOrigin = "Http://localhost:5006";

    services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
    {
        builder.WithOrigins(clientOrigin)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    }));
}