using System.Text.Json.Serialization;

namespace CopilotDev.NET.Api.Entity
{
    public class CopilotChoice
    {
        /// <summary>
        /// Text Result.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Unknown Index value.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Unknown value.
        /// </summary>
        [JsonPropertyName("finish_reason")]
        public object FinishReason { get; set; }

        /// <summary>
        /// Unknown values.
        /// </summary>
        [JsonPropertyName("logprobs")]
        public CopilotLogProbs LogProbs { get; set; }
    }
}
