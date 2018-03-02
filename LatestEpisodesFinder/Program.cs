using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using static LatestEpisodesFinder.Finder;
using static LatestEpisodesFinder.ConfigurationProvider;
using static LatestEpisodesFinder.Adder;

namespace LatestEpisodesFinder
{
    class Program
    {
        static Task Main(string[] args) =>
            IsConfigurationValid() ?
            new Parser(settings =>
                {
                    settings.ParsingCulture = CultureInfo.CurrentCulture;
                    settings.IgnoreUnknownArguments = false;
                }).ParseArguments<AddOptions, FindOptions>(args)
                  .MapResult((AddOptions add) => AddSeriesToCheckAsync(add.DatabaseFile, add.Name),
                             (FindOptions find) => FindLatestEpisodeByDateAsync(find.FromDate,
                                                                                find.DatabaseFile),
                             errors => errors.Select(e => e.Tag.ToString())
                                             .Select(Console.Out.WriteLineAsync)
                                             .WhenAll()) :
            SetupConfigurationAsync();
    }
}
