namespace CopilotDev.NET.Api.Entity
{
    public class CopilotDeviceAuthenticationData
    {
        /// <summary>
        /// Url to enter the device code.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// UserCode to enter on the given Url.
        /// </summary>
        public string UserCode { get; set; }
    }
}
