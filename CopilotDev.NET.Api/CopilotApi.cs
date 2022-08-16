using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CopilotDev.NET.Api.Contract;
using CopilotDev.NET.Api.Entity;
using CopilotDev.NET.Api.Extension;

namespace CopilotDev.NET.Api
{
    public class CopilotApi : ICopilotApi
    {
        private readonly ICopilotAuthentication _copilotAuthentication;
        private readonly CopilotConfiguration _configuration;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of the copilot api with dependencies.
        /// </summary>
        /// <param name="configuration"><see cref="CopilotConfiguration"/> configuration.</param>
        /// <param name="authentication">Handles the authentication flow.</param>
        /// <param name="httpClient">HttpClient for performing web requests.</param>
        public CopilotApi(CopilotConfiguration configuration, ICopilotAuthentication authentication,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _copilotAuthentication = authentication;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the returned tokens from the Copilot AI programmer.
        /// Uses meaningful default parameters along with the given prompt. For a customization, use <see cref="ICopilotApi.GetCompletionsAsync"/> instead.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns>A list of strings containing the text values of the returned tokens.</returns>
        public async Task<List<string>> GetStringCompletionsAsync(string prompt)
        {
            var copilotParameters = new CopilotParameters
            {
                Prompt = prompt
            };
            var tokens = await GetCompletionsAsync(copilotParameters);
            return tokens.Select(e => e.Choices[0].Text).ToList();
        }

        /// <summary>
        /// Gets the returned tokens from the Copilot AI programmer.
        /// Uses the Copilot Parameter data to customize the result.
        /// </summary>
        /// <param name="parameters"><see cref="CopilotParameters"/> parameters</param>
        /// <returns>A list of returned copilot tokens.</returns>
        public async Task<List<CopilotResult>> GetCompletionsAsync(CopilotParameters parameters)
        {
            _httpClient.AddOrReplaceHeader("OpenAI-Intent", parameters.OpenAiIntent);
            var rawResult = await GetRawCompletionsAsync(JsonSerializer.Serialize(parameters));
            var lines = rawResult.Split('\n').Where(e => e != "").ToList();
            lines.RemoveAt(lines.Count - 1);
            var results = new List<CopilotResult>();

            foreach (var line in lines)
            {
                var content = line.Replace("data: ", "");
                var resultObject = JsonSerializer.Deserialize<CopilotResult>(content);
                var jsonObject = JsonDocument.Parse(content);
                resultObject.CreationDate = DateTimeOffset
                    .FromUnixTimeSeconds((long)jsonObject.RootElement.GetProperty("created").GetInt64())
                    .LocalDateTime;
                results.Add(resultObject);
            }

            return results;
        }

        /// <summary>
        /// Gets the returned result from the CopilotAI programmer.
        /// Takes a raw string which is sent per http request and returns the raw result. Useful for testing unknown parameters or results.
        /// </summary>
        /// <param name="rawContent">Raw parameters.</param>
        /// <returns>Raw return result.</returns>
        public async Task<string> GetRawCompletionsAsync(string rawContent)
        {
            var accessToken = await _copilotAuthentication.GetAccessTokenAsync();
            _httpClient.AddOrReplaceHeader("User-Agent", _configuration.UserAgent);
            _httpClient.AddOrReplaceHeader("Accept", "application/json");
            _httpClient.AddOrReplaceHeader("Authorization", "Bearer " + accessToken);
            var response = await _httpClient.PostAsync(
                "https://copilot-proxy.githubusercontent.com/v1/engines/copilot-codex/completions",
                new StringContent(rawContent));
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
            _copilotAuthentication?.Dispose();
        }
    }
}
