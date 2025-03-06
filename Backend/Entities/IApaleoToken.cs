﻿using System.Text.Json.Serialization;

namespace backend.Entities;

public interface IApaleoToken
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}