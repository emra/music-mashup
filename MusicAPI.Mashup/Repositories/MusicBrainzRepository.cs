namespace MusicAPI.Mashup.Repositories
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using MusicAPI.Mashup.Models.MusicBrainz;

    using Newtonsoft.Json;

    public class MusicBrainzRepository : IMusicBrainzRepository
    {
        private readonly HttpClient client;

        public MusicBrainzRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<MusicBrainzResponseModel> GetArtistFromMusicBrainz(string mbid)
        {
            var response = await this.MakeRequest(mbid);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MusicBrainzResponseModel>(stringResult);
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                try
                {
                    response = await DelayedRetry(3, () => this.MakeRequest(mbid));

                    var stringResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MusicBrainzResponseModel>(stringResult);
                }
                catch (HttpResponseException)
                {
                    throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
                }
            }

            throw new HttpResponseException(response.StatusCode);
        }

        private async Task<HttpResponseMessage> MakeRequest(string mbid)
        {
            return await this.client.GetAsync($"/ws/2/artist/{mbid}?&fmt=json&inc=url-rels+release-groups");
        }

        private static async Task<HttpResponseMessage> DelayedRetry(int maximumAttempts, Func<Task<HttpResponseMessage>> operation)
        {
            var random = new Random();
            var randomSeconds = random.NextDouble() * (1.5 - 1.0) + 1.0;
            var delay = TimeSpan.FromSeconds(randomSeconds);

            var attempts = 0;

            for (var i = 0; i < maximumAttempts; i++)
            {
                var response = await operation();

                try
                {
                    attempts++;
                    response.EnsureSuccessStatusCode();
                    return response;
                }
                catch
                {
                    if (attempts == maximumAttempts)
                    {
                        throw;
                    }

                    Task.Delay(delay).Wait();
                }
            }

            throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
        }
    }
}