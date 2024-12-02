namespace backend.Util;

using Microsoft.Extensions.Configuration;

public class SecretsProvider
{
    private static readonly Lazy<SecretsProvider> _instance = 
        new Lazy<SecretsProvider>(() => new SecretsProvider());

    public static SecretsProvider Instance => _instance.Value;

    private readonly IConfiguration _configuration;

    // Secrets als Properties
    public string MailHost => _configuration["Mail:Host"];
    public int MailPort => int.TryParse(_configuration["Mail:Port"], out var port) ? port : 0;
    public string MailAddress => _configuration["Mail:Address"];
    public string MailKey => _configuration["Mail:Key"];
    public string JwtKey => _configuration["Jwt:Key"];
    public string JwtIssuer => _configuration["Jwt:Issuer"];
    public string JwtAudience => _configuration["Jwt:Audience"];
    public string OpenAiKey => _configuration["OpenAi:Key"];
    public string MistralAiKey => _configuration["MistralAi:Key"];

    private SecretsProvider()
    {
        // Konfigurationsquellen laden
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
        builder.AddEnvironmentVariables();

        var kubernetesSecretPath = "/etc/secrets";
        if (Directory.Exists(kubernetesSecretPath))
        {
            builder.AddKeyPerFile(kubernetesSecretPath, optional: true);
        }

        _configuration = builder.Build();
        
        // Secrets anzeigen
        Console.WriteLine($"Mail Host: {MailHost}");
        Console.WriteLine($"Mail Port: {MailPort}");
        Console.WriteLine($"Mail Address: {MailAddress}");
        Console.WriteLine($"Mail Key: {MailKey}");
        Console.WriteLine($"JWT Key: {JwtKey}");
        Console.WriteLine($"JWT Issuer: {JwtIssuer}");
        Console.WriteLine($"JWT Audience: {JwtAudience}");
        Console.WriteLine($"OpenAI Key: {OpenAiKey}");
        Console.WriteLine($"MistralAI Key: {MistralAiKey}");

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        
        // Access MIt Environment
        Console.WriteLine($"MAIL_HOST: {Environment.GetEnvironmentVariable("MAIL_HOST")}");
        Console.WriteLine($"MAIL_PORT: {Environment.GetEnvironmentVariable("MAIL_PORT")}");
        Console.WriteLine($"MAIL_ADDRESS: {Environment.GetEnvironmentVariable("MAIL_ADDRESS")}");
        Console.WriteLine($"MAIL_KEY: {Environment.GetEnvironmentVariable("MAIL_KEY")}");
        Console.WriteLine($"JWT_KEY: {Environment.GetEnvironmentVariable("JWT_KEY")}");
        Console.WriteLine($"JWT_ISSUER: {Environment.GetEnvironmentVariable("JWT_ISSUER")}");
        Console.WriteLine($"JWT_AUDIENCE: {Environment.GetEnvironmentVariable("JWT_AUDIENCE")}");
        Console.WriteLine($"OPENAI_KEY: {Environment.GetEnvironmentVariable("OPENAI_KEY")}");
        Console.WriteLine($"MISTRALAI_KEY: {Environment.GetEnvironmentVariable("MISTRALAI_KEY")}");
    }
}
