using Microsoft.Extensions.Configuration;
using System.IO;

namespace FunctionBlobTrigger
{
    public class ConfigRoot
    {
        public IConfigurationRoot Config { get; }
        public ConfigRoot()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secret.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
