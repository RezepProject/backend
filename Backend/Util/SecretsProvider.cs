namespace backend.Util;

using Microsoft.Extensions.Configuration;

public class SecretsProvider
{
    private static readonly Lazy<SecretsProvider> _instance = new(() => new SecretsProvider());

    public static SecretsProvider Instance => _instance.Value;

    private readonly IConfiguration _configuration;

    // Secrets als Properties
    public string MailHost => _configuration["Mail:Host"] ?? Environment.GetEnvironmentVariable("MAIL_HOST");

    public int MailPort => int.TryParse(_configuration["Mail:Port"], out var port) ? port :
        int.TryParse(Environment.GetEnvironmentVariable("MAIL_PORT"), out var envPort) ? envPort : 0;

    public string MailAddress => _configuration["Mail:Address"] ?? Environment.GetEnvironmentVariable("MAIL_ADDRESS");
    public string MailKey => _configuration["Mail:Key"] ?? Environment.GetEnvironmentVariable("MAIL_KEY");
    public string JwtKey => _configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY");
    public string JwtIssuer => _configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER");
    public string JwtAudience => _configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    public string OpenAiKey => _configuration["OpenAi:Key"] ?? Environment.GetEnvironmentVariable("OPENAI_KEY");

    public string MistralAiKey =>
        _configuration["MistralAi:Key"] ?? Environment.GetEnvironmentVariable("MISTRALAI_KEY");

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
    }
}