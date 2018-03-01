using System.Diagnostics;

namespace LatestEpisodesFinder
{
    [DebuggerDisplay("{" + nameof(Id) + "} - {" + nameof(Name) + "}")]
    class Series
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
    }
}