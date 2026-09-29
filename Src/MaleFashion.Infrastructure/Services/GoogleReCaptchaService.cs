using MaleFashion.Application.Contracts.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MaleFashion.Infrastructure.Services
{
    public class GoogleReCaptchaService : IGoogleReCaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleReCaptchaOptions _options;

        public GoogleReCaptchaService(
            HttpClient httpClient,
            IOptions<GoogleReCaptchaOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<bool> VerifyAsync(
            string token,
            string expectedAction,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["secret"] = _options.SecretKey,
                    ["response"] = token
                });

            var response = await _httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var json = await response.Content.ReadAsStringAsync(
                cancellationToken);

            var result = JsonSerializer.Deserialize<ReCaptchaResponse>(
                json);

            if (result == null)
            {
                return false;
            }

            return result.Success
                   && result.Action == expectedAction
                   && result.Score >= _options.MinimumScore;
        }
    }

}
