using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CopilotDev.NET.Api.Entity;

namespace CopilotDev.NET.Api
{
    /// <summary>
    /// Root Access to the Copilot Api.
    /// </summary>
    public interface ICopilotApi : IDisposable
    {
        /// <summary>
        /// Gets the returned tokens from the Copilot AI programmer.
        /// Uses meaningful default parameters along with the given prompt. For a customization, use <see cref="GetCompletionsAsync"/> instead.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns>A list of strings containing the text values of the returned tokens.</returns>
        Task<List<string>> GetStringCompletionsAsync(string prompt);

        /// <summary>
        /// Gets the returned tokens from the Copilot AI programmer.
        /// Uses the Copilot Parameter data to customize the result.
        /// </summary>
        /// <param name="parameters"><see cref="CopilotParameters"/> parameters</param>
        /// <returns>A list of returned copilot tokens.</returns>
        Task<List<CopilotResult>> GetCompletionsAsync(CopilotParameters parameters);

        /// <summary>
        /// Gets the returned result from the CopilotAI programmer.
        /// Takes a raw string which is sent per http request and returns the raw result. Useful for testing unknown parameters or results.
        /// e.g.
        /// </summary>
        /// <param name="rawContent">Raw parameters.</param>
        /// <returns>Raw return result.</returns>
        Task<string> GetRawCompletionsAsync(string rawContent);
    }
}