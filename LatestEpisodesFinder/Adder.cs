using System;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp.Enums;
using static LatestEpisodesFinder.Storage;
using static LatestEpisodesFinder.TraktClientFactory;

namespace LatestEpisodesFinder
{
    static class Adder
    {
        internal static async Task AddSeriesToCheckAsync(string dbFile, string name)
        {
            var matchingShows = await TraktClient.FindShowsAsync(name);

            await Enumerable.Range(0, matchingShows.Count)
                            .Select(index => (index, show: matchingShows.Skip(index).First()))
                            .Select(t => $"[{t.index}] - {t.show.Title} ({t.show.Year})")
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

            var entity = new Series
            {
                Id = show.Ids.Trakt,
                Name = show.Title,
                IsRunning = !show.Status.In(TraktShowStatus.Canceled, TraktShowStatus.Ended)
            };

            Save(dbFile, entity);

            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync("Series added to the database.");
        }
    }
}