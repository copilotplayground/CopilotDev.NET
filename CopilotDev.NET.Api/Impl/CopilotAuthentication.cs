using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CopilotDev.NET.Api.Contract;
using CopilotDev.NET.Api.Entity;
using CopilotDev.NET.Api.Extension;

namespace CopilotDev.NET.Api.Impl
{
    public class CopilotAuthentication : ICopilotAuthentication
    {
        private readonly CopilotConfiguration _configuration;
        private readonly IDataStore _dataStore;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of the Copilot Authentication flow.
        /// </summary>
        /// <param name="configuration">Configuration for the API.</param>
        /// <param name="dataStore">Storage where to store tokens.</param>
        /// <param name="httpClient">HttpClient to perform web requests.</param>
        public CopilotAuthentication(CopilotConfiguration configuration, IDataStore dataStore, HttpClient httpClient)
        {
            _configuration = configuration;
            _dataStore = dataStore;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets called when the authentication flow requires user input.
        /// </summary>
        public event Action<CopilotDeviceAuthenticationData> OnEnterDeviceCode;

        /// <summary>
        /// Gets the access authenticationData, which can be used to authenticate http requests to the copilot http api.
        /// </summary>
        /// <returns>Access Token.</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            var rawToken = await _dataStore.GetAsync();
            var authenticationData = new CopilotAuthenticationData();

            if (rawToken != null)
            {
                authenticationData = JsonSerializer.Deserialize<CopilotAuthenticationData>(rawToken);
            }

            if (authenticationData != null && authenticationData.AccessToken != null && DateTime.Now < authenticationData.AccessTokenValidTo)
            {
                return authenticationData.AccessToken;
            }

            _httpClient.AddOrReplaceHeader("User-Agent", _configuration.UserAgent);
            _httpClient.AddOrReplaceHeader("Accept", "application/json");

            authenticationData.GithubToken = await GetSessionToken(authenticationData);
            _httpClient.AddOrReplaceHeader("Authorization", $"token {authenticationData.GithubToken}");
            var response = await _httpClient.GetAsync("https://api.github.com/copilot_internal/v2/token");
            var jsonResult = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(jsonResult);
            authenticationData.AccessToken = jsonObject.RootElement.GetProperty("token").GetString();
            authenticationData.AccessTokenValidTo =
                DateTimeOffset.FromUnixTimeSeconds((long) jsonObject.RootElement.GetProperty("expires_at").GetInt64())
                    .LocalDateTime;
            await _dataStore.SaveAsync(JsonSerializer.Serialize(authenticationData));

            return authenticationData.AccessToken;
        }

        /// <summary>
        /// Gets a session authenticationData, which identifies the current device session with Github.
        /// </summary>
        /// <returns>Valid Session Token.</returns>
        private async Task<string> GetSessionToken(CopilotAuthenticationData authenticationData)
        {
            if (authenticationData.GithubToken != null)
            {
                return authenticationData.GithubToken;
            }

            var tokens = await GetDeviceToken();
            var deviceToken = tokens[0];
            var userCode = tokens[1];

            OnEnterDeviceCode?.Invoke(new CopilotDeviceAuthenticationData
            {
                Url = "https://github.com/login/device",
                UserCode = userCode
            });

            // Wait until the user has entered the device authenticationData in Github.
            while (true)
            {
                await Task.Delay(5000);
                var response = await _httpClient.PostAsync(
                    $"https://github.com/login/oauth/access_token?client_id={_configuration.GithubAppId}&device_code={deviceToken}&grant_type={_configuration.GithubGrantType}",
                    new StringContent(""));
                var data = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonDocument.Parse(data);

                if (!jsonObject.RootElement.TryGetProperty("error", out _))
                {
                    authenticationData.GithubToken = jsonObject.RootElement.GetProperty("access_token").GetString();
                    return authenticationData.GithubToken;
                }
            }
        }

        /// <summary>
        /// Gets a new device code identifying this device.
        /// </summary>
        /// <returns>Device Token.</returns>
        private async Task<string[]> GetDeviceToken()
        {
            var response = await _httpClient.PostAsync(
                $"https://github.com/login/device/code?client_id={_configuration.GithubAppId}&scope=read:user",
                new StringContent(""));
            var data = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(data);
            var deviceToken = jsonObject.RootElement.GetProperty("device_code").GetString();
            var userCode = jsonObject.RootElement.GetProperty("user_code").GetString();
            return new[] {deviceToken, userCode};
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}