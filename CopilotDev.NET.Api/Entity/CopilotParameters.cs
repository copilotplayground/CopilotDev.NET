using System.Text.Json.Serialization;

namespace CopilotDev.NET.Api.Entity
{
    /// <summary>
    /// Explanation for parameters can be found here.
    /// https://beta.openai.com/docs/api-reference/completions
    /// </summary>
    public class CopilotParameters
    {
        /// <summary>
        /// Text context for the returned completions.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// Amount of tokens, which should be returned by copilot.
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 30;

        /// <summary>
        /// Higher values means the model will take more risks.
        /// 0.9 for more creative applications, and 0 for ones with a well-defined answer.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; } = 1;

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// </summary>
        [JsonPropertyName("top_p")]
        public float TopP { get; set; } = 1F;

        /// <summary>
        /// How many completions to generate for each prompt.
        /// You can group the related completions using the index property. e.g. completions.GroupBy(e => e.Choices[0].Index)
        /// </summary>
        [JsonPropertyName("n")]
        public int N { get; set; } = 1;

        /// <summary>
        ///  Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens.
        /// For example, if logprobs is 5, the API will return a list of the 5 most likely tokens. The maximum value for logprobs is 5
        /// </summary>
        [JsonPropertyName("logprobs")]
        public int? LogProbs { get; set; } = null;

        /// <summary>
        /// Should the response be streamed. Only true is allowed.
        /// </summary>
        [JsonPropertyName("stream")]
        public bool IsStreamEnabled { get; set; } = true;

        /// <summary>
        /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
        /// Use "\n" for one line completions. Use "\n\n" for multi line completions. 
        /// </summary>
        [JsonPropertyName("stop")]
        public string[] Stop { get; set; } = new[] {"\n"};

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