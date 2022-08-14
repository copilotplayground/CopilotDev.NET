using System;
using System.Threading.Tasks;
using CopilotDev.NET.Api.Entity;

namespace CopilotDev.NET.Api.Contract
{
    public interface ICopilotAuthentication : IDisposable
    {
        /// <summary>
        /// Gets called when the authentication flow requires user input.
        /// </summary>
        event Action<CopilotDeviceAuthenticationData> OnEnterDeviceCode;

        /// <summary>
        /// Gets the access token, which can be used to authenticate http requests to the copilot http api.
        /// </summary>
        /// <returns>Access Token.</returns>
        Task<string> GetAccessTokenAsync();
    }
}
