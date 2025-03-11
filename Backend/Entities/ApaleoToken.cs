using System.Text.Json.Serialization;

namespace backend.Entities;

public class ApaleoToken
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}