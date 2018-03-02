using CommandLine;

namespace LatestEpisodesFinder
{
    [Verb("list", HelpText = "List all series in the database.")]
    public class ListOptions : BaseOptions
    {
    }
}