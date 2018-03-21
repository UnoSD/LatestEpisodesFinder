using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using static LatestEpisodesFinder.Storage;
using static LatestEpisodesFinder.TraktClientFactory;

namespace LatestEpisodesFinder
{
    static class Finder
    {
        internal static async Task FindLatestEpisodeByDateAsync(DateTime fromDate, string dbFile)
        {
            string FormatEpisode(TraktEpisode e, string seriesName) => 
                $"{TimeZoneInfo.ConvertTimeFromUtc(e.FirstAired.Value, TimeZoneInfo.Local)} - {seriesName} - {e.SeasonNumber} - {e.Title}";

            var seasons = await GetAll<Series>(dbFile, s => s.IsRunning).Select(GetLastSeason)
                                                                        .WhenAll();

            await seasons.Select(s => (s.name, episodes: GetLatestEpisodes(s.season, fromDate)))
                         .Where(s => s.episodes.Any())
                         .SelectMany(s => s.episodes.Select(e => FormatEpisode(e, s.name)))
                         .Select(Console.Out.WriteLineAsync)
                         .WhenAll();
        }
        
        internal static Task ListSeriesStored(string dbFile) => 
            GetAll<Series>(dbFile).Select(s => $"{s.Name} - {(s.IsRunning ? "Running" : "Not running")}")
                                  .Select(Console.Out.WriteLineAsync)
                                  .WhenAll();

        static TraktSeason LastSeason(Task<IEnumerable<TraktSeason>> task) => 
            task.Result
                .Where(season => season.FirstAired.HasValue)
                .OrderBy(season => season.Number ?? 0)
                .Last();

        static async Task<(string name, TraktSeason season)> GetLastSeason(Series series) => 
            (series.Name, 
             await TraktClient.GetSeasonsAsync(series.Id.ToString())
                              .ContinueWith(LastSeason));

        static IReadOnlyCollection<TraktEpisode> GetLatestEpisodes(TraktSeason season, DateTime fromDate) =>
            season.Episodes
                  .Where(e => e.FirstAired >= fromDate &&
                              e.FirstAired <= DateTime.UtcNow)
                  .ToList();
    }
}