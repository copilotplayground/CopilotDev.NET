using System.Text.Json.Serialization;

namespace CopilotDev.NET.Api.Entity
{
    public class CopilotParameters
    {
        /// <summary>
        /// Text context for the returned suggestions.
        /// Support for placeholders is currently unknown. 
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// Amount of tokens, which should be returned by copilot.
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 30;

        /// <summary>
        /// Unknown effect. Known values are '0.0f', '0.2f', '0.4f' or '0.8f'. Parameter N seems to be related.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; } = 0.2f;

        /// <summary>
        /// Unknown effect. Only value '1' seems to return something.
        /// </summary>
        [JsonPropertyName("top_p")]
        public int TopP { get; set; } = 1;

        /// <summary>
        /// Unknown effect. Seems to be related to temperature.
        /// </summary>
        [JsonPropertyName("n")]
        public string N { get; set; }

        /// <summary>
        /// Unknown effect. 
        /// </summary>
        [JsonPropertyName("logprobs")]
        public int LogProbs { get; set; } = 1;

        /// <summary>
        /// Should the response be streamed. Only true is allowed.
        /// </summary>
        [JsonPropertyName("stream")]
        public bool IsStreamEnabled { get; set; } = true;

        /// <summary>
        /// Detailed effects are not known. Seems to be related to multi line line breaks for suggestions.
        /// </summary>
        [JsonPropertyName("stops")]
        public string[] Stops { get; set; } = new string[] { "\n" };

        /// <summary>
        /// Unknown effects. One known experimental feature is 'nextLineIndent'.
        /// </summary>
        [JsonPropertyName("experimentalFeatures")]
        public string[] ExperimentalFeatures { get; set; } = { };

        /// <summary>
        /// Extra data (may be used for experimental features) which is not known. Serialized, it is a new json object.
        /// </summary>
        [JsonPropertyName("extra")]
        public object Extra { get; set; }

        /// <summary>
        /// Header value with unknown effects. Known values are 'copilot-ghost' or 'copilot-panel'.
        /// </summary>
        public string OpenAiIntent { get; set; } = "copilot-ghost";
    }
}
