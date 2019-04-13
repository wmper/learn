using Microsoft.Extensions.Configuration;

namespace Example.IHostService
{
    public class ConfigurationManager
    {
        /// <summary>
        /// App's static IConfiguration
        /// </summary>
        public static IConfiguration Configuration { get; set; }
    }
}
