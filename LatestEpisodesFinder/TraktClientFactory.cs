using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Exceptions;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Requests.Params;
using static LatestEpisodesFinder.ConfigurationProvider;

namespace LatestEpisodesFinder
{
    static class TraktClientFactory
    {
        internal const string ConfigurationKeyClientId = "ClientId";
        internal const string ConfigurationKeyClientSecret = "ClientSecret";

        static readonly TraktExtendedInfo Info = new TraktExtendedInfo
        {
            Episodes = true,
            Full = true,
            Images = false,
            Metadata = true,
            NoSeasons = false
        };

        static readonly Lazy<TraktClient> Client =
            new Lazy<TraktClient>(() => 
                new TraktClient(Configuration.Value[ConfigurationKeyClientId], 
                                Configuration.Value[ConfigurationKeyClientSecret]));

        internal static TraktClient TraktClient => Client.Value;

        internal static async Task<TraktShow> GetShowAsync(this TraktClient client, string serie)
        {
            try
            {
                return await client.Shows.GetShowAsync(serie, Info);
            }
            catch (TraktShowNotFoundException)
            {
                return new TraktShow { Ids = new TraktShowIds { Slug = serie, Trakt = 0 } };
            }
        }

        internal static Task<IEnumerable<TraktSeason>> GetSeasonsAsync(this TraktClient client, string serie) =>
            client.Seasons.GetAllSeasonsAsync(serie, Info);
    }
}