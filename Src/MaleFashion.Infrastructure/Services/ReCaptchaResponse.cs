using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MaleFashion.Infrastructure.Services
{
    public class ReCaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonPropertyName("error-codes")]
        public string[] ErrorCodes { get; set; } = [];
    }
}
