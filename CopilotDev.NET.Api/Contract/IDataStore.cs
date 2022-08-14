using System.Threading.Tasks;

namespace CopilotDev.NET.Api.Contract
{
    /// <summary>
    /// Implementations of this interface are used to store and retrieved created tokens when
    /// using the Github Copilot Api. Examples of such tokens are the device code and access token.
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Retrieves the previously stored JSON string from this data store.
        /// </summary>
        /// <returns>JSON string or null if not found.</returns>
        Task<string> GetAsync();

        /// <summary>
        /// Stores the given <paramref name="data"/> into this data store.
        /// </summary>
        /// <param name="data">Not null JSON string.</param>
        /// <returns><see cref="Task"/></returns>
        Task SaveAsync(string data);
    }
}
