using CommandLine;

namespace LatestEpisodesFinder
{
    [Verb("add", HelpText = "Add new series to the database.")]
    class AddOptions : BaseOptions
    {
        [Value(0, Required = true, HelpText = "Name of the series to add.")]
        public string Name { get; set; }
    }
}