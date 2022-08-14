using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CopilotDev.NET.Api.Entity
{
    public class CopilotLogProbs
    {
        [JsonPropertyName("tokens")]
        public string[] Tokens { get; set; }

        [JsonPropertyName("token_logprobs")]
        public double[] TokenLogProbs { get; set; }

        [JsonPropertyName("top_logprobs")]
        public Dictionary<string, object>[] TopLogProbs { get; set; }

        [JsonPropertyName("text_offset")]
        public int[] TextOffSet { get; set; }
    }
}
