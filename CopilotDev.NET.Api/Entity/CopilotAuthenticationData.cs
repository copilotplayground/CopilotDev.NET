using System;

namespace CopilotDev.NET.Api.Entity
{
    internal class CopilotAuthenticationData
    {
        public string AccessToken { get; set; }

        public DateTime AccessTokenValidTo { get; set; }

        public string GithubToken { get; set; }
    }
}
