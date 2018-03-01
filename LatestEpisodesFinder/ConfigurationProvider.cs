using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static LatestEpisodesFinder.HomePathProvider;

namespace LatestEpisodesFinder
{
    static class ConfigurationProvider
    {
        const string ConfigurationFilename = ".lef-config.json";

        static readonly Lazy<string> ConfigPath =
            new Lazy<string>(() => Path.Combine(HomePath.Value, ConfigurationFilename));

        internal static Lazy<IConfigurationRoot> Configuration =>
            new Lazy<IConfigurationRoot>(() =>
                new ConfigurationBuilder().AddInMemoryCollection(GetConfigFromFile())
                                          // This turd just doesn't work, sick of it.
                                          // Using InMemoryCollection from file.
                                          .SetBasePath(HomePath.Value)
                                          .AddJsonFile(ConfigurationFilename, true)
                                          .Build());

        static IEnumerable<KeyValuePair<string, string>> GetConfigFromFile() =>
            File.Exists(ConfigPath.Value) ?
            JsonConvert.DeserializeObject<Dictionary<string, string>>
                (File.ReadAllText(ConfigPath.Value)) :
            Enumerable.Empty<KeyValuePair<string, string>>();

        internal static bool IsConfigurationValid() =>
            new[]
            {
                Configuration.Value[TraktClientFactory.ConfigurationKeyClientId],
                Configuration.Value[TraktClientFactory.ConfigurationKeyClientSecret]
            }.All(config => config != null);

        internal static async Task SetupConfigurationAsync()
        {
            await Console.Out.WriteLineAsync("Configuration not found, setting it up...");
            await Console.Out.WriteLineAsync("Register here (https://trakt.tv/) to obtain credentials");
            await Console.Out.WriteLineAsync();

            await Console.Out.WriteAsync("Insert Trakt client ID: ");

            var clientId = await Console.In.ReadLineAsync();

            await Console.Out.WriteAsync("Insert Trakt client secret: ");

            var clientSecret = await Console.In.ReadLineAsync();

            var configuration = JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                [TraktClientFactory.ConfigurationKeyClientId] = clientId,
                [TraktClientFactory.ConfigurationKeyClientSecret] = clientSecret
            }, Formatting.Indented);

            using (var writer = new StreamWriter(new FileInfo(ConfigPath.Value).Open(FileMode.Create)))
                await writer.WriteAsync(configuration);

            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync($"Configuration saved in {HomePath.Value}, edit .lef-config.json to change.");
        }
    }
}