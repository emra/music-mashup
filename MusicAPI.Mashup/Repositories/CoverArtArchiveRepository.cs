namespace MusicAPI.Mashup.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.CoverArtArchive;
    using MusicAPI.Mashup.Models.MusicBrainz;

    using Newtonsoft.Json;

    public class CoverArtArchiveRepository : ICoverArtArchiveRepository
    {
        private readonly HttpClient client;

        public CoverArtArchiveRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Dictionary<string, string[]>> GetCoverArt(IEnumerable<RealeaseGroup> albums)
        {
            var coverArts = new Dictionary<string, string[]>();

            foreach (var album in albums)
            {
                var response = await this.client.GetAsync($"/release-group/{album.Mbid}");

                CoverArtResponseModel coverArtInfo;
                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    coverArtInfo = JsonConvert.DeserializeObject<CoverArtResponseModel>(stringResult);
                }
                else
                {
                    coverArtInfo = new CoverArtResponseModel { Images = new Image[0] };
                }

                coverArts.Add(album.Mbid, coverArtInfo.Images.Select(i => i.ImageUrl).ToArray());
            }

            return coverArts;
        }
    }
}