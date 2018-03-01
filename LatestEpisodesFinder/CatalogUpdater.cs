using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp.Enums;
using static LatestEpisodesFinder.Storage;
using static LatestEpisodesFinder.TraktClientFactory;

namespace LatestEpisodesFinder
{
    static class CatalogUpdater
    {
        internal static async Task UpdateEndedOrCanceledShowsStatus(string dbFile)
        {
            var shows = GetAll<Series>(dbFile).Select(e => (series: e,
                                                            traktShow: TraktClient.GetShowAsync(e.Id.ToString())))
                                              .ToList();

            await shows.Select(e => e.traktShow)
                       .WhenAll();

            var series = shows.Select(show => new Series
            {
                Id = show.series.Id,
                Name = show.series.Name,
                IsRunning = show.traktShow.Result.Status != TraktShowStatus.Canceled &&
                            show.traktShow.Result.Status != TraktShowStatus.Ended
            });
            
            UpdateAll(dbFile, series);
        }
    }
}