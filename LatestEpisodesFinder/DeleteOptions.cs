using CommandLine;

namespace LatestEpisodesFinder
{
    [Verb("delete", HelpText = "Remove series from the database.")]
    class DeleteOptions : BaseOptions
    {
        [Value(0, Required = true, HelpText = "Search term of the series to remove.")]
        public string Term { get; set; }
    }
}