using System;
using System.Linq;
using System.Threading.Tasks;
using static LatestEpisodesFinder.Storage;

namespace LatestEpisodesFinder
{
    static class Deleter
    {
        internal static async Task DeleteSerieStored(string dbFile, string searchTerm)
        {
            var matchingShows =
                GetAll<Series>(dbFile, series => series.Name
                                                               .ToLowerInvariant()
                                                               .Contains(searchTerm.Trim()
                                                                                   .ToLowerInvariant()));
            
            await Enumerable.Range(0, matchingShows.Count)
                            .Select(index => (index, show: matchingShows.Skip(index).First()))
                            .Select(t => $"[{t.index}] - {t.show.Name}")
                            .Select(Console.Out.WriteLineAsync)
                            .WhenAll();

            await Console.Out.WriteLineAsync();
            await Console.Out.WriteAsync("Pick a series from the options or [q]uit: ");
            var choice = await Console.In.ReadLineAsync();

            if (choice == "q")
                return;

            if (!int.TryParse(choice, out var showIndex))
            {
                await Console.Out.WriteLineAsync($"{choice} is not a choice.");
                return;
            }

            var show = matchingShows.Skip(showIndex).First();

            var success = Delete<Series>(dbFile, series => series.Id == show.Id) == 1;

            await Console.Out.WriteLineAsync();

            var output = success ? "Series deleted from the database." : "Failed to delete series.";
            
            await Console.Out.WriteLineAsync(output);
        }
    }
}