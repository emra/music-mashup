namespace MusicAPI.Mashup.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.MusicBrainz;
    using MusicAPI.Mashup.Models.Wikipedia;

    using Newtonsoft.Json;

    public class WikipediaRepository : IWikipediaRepository
    {
        private readonly HttpClient client;

        public WikipediaRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<WikipediaBiographyResponseModel> GetWikipediaBiography(IEnumerable<Relation> wikiRelations)
        {
            var result = new List<WikipediaBiographyResponseModel>();

            foreach (var relation in wikiRelations)
            {
                var searchTitle = ExtractWikipediaSearchTitle(relation);

                var response = await this.client.GetAsync(
                                   $"/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles={searchTitle}");

                var stringResult = await response.Content.ReadAsStringAsync();
                var wikipediaBiography = JsonConvert.DeserializeObject<WikipediaBiographyResponseModel>(stringResult);

                result.Add(wikipediaBiography);
            }

            return result.FirstOrDefault();
        }

        private static string ExtractWikipediaSearchTitle(Relation relation)
        {
            var startindex = relation.Url.Resource.LastIndexOf("/", StringComparison.Ordinal) + 1;
            var endindex = relation.Url.Resource.Length - 1;
            var length = endindex - startindex + 1;

            var wikipediaSearchTitle = relation.Url.Resource.Substring(startindex, length);

            return wikipediaSearchTitle;
        }
    }
}