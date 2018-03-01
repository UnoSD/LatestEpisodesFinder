using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatestEpisodesFinder
{
    static class Extensions
    {
        internal static Task<T> AsTask<T>(this T value) => Task.FromResult(value);

        internal static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

        internal static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAll(tasks);
    }
}