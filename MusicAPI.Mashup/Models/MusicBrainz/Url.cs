namespace MusicAPI.Mashup.Models.MusicBrainz
{
    using Newtonsoft.Json;

    public class Url
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }
    }
}