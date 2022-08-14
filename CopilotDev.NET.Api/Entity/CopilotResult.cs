using System;
using System.Text.Json.Serialization;

namespace CopilotDev.NET.Api.Entity
{
    public class CopilotResult
    {
        /// <summary>
        /// Request and response id.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the machine learning model being used.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// LocalDate when this result was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Available result choices.
        /// </summary>
        [JsonPropertyName("choices")]
        public CopilotChoice[] Choices { get; set; }
    }
}
