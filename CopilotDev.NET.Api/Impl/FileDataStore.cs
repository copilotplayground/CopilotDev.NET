using System.IO;
using System.Threading.Tasks;
using CopilotDev.NET.Api.Contract;

namespace CopilotDev.NET.Api.Impl
{
    /// <summary>
    /// Stores and retrieves Github Copilot Tokens from a file.
    /// </summary>
    public class FileDataStore : IDataStore
    {
        private readonly string _filePath;

        /// <summary>
        /// Creates a new <see cref="FileDataStore"/> with the file being the given <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">Path to the file where the token should be stored. Needs to be in an existing directory.</param>
        public FileDataStore(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Retrieves the previously stored JSON string from this data store.
        /// </summary>
        /// <returns>JSON string or null if not found.</returns>
        public async Task<string> GetAsync()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            var result = await Task.Run(() => File.ReadAllText(_filePath));
            return result;
        }

        /// <summary>
        /// Stores the given <paramref name="data"/> into this data store.
        /// </summary>
        /// <param name="data">Not null JSON string.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task SaveAsync(string data)
        {
            await Task.Run(() => File.WriteAllText(_filePath, data));
        }
    }
}
