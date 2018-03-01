using System;

namespace LatestEpisodesFinder
{
    class HomePathProvider
    {
        internal static readonly Lazy<string> HomePath = new Lazy<string>(() =>
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }
}