using System;
using CommandLine;

namespace LatestEpisodesFinder
{
    [Verb("find", HelpText = "Find episodes aired after a certain date.")]
    class FindOptions : BaseOptions
    {
        [Option]
        [Value(0)]
        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(-1);
    }
}