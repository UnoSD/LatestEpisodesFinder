using System.IO;
using CommandLine;
using static LatestEpisodesFinder.HomePathProvider;

namespace LatestEpisodesFinder
{
    public class BaseOptions
    {
        [Option(HelpText = "Database to store series information.")]
        public string DatabaseFile { get; set; } = Path.Combine(HomePath.Value, ".lef-series.db");
    }
}