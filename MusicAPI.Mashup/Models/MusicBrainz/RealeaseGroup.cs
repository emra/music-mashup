namespace MusicAPI.Mashup.Models.MusicBrainz
{
    using Newtonsoft.Json;

    public class RealeaseGroup
    {
        [JsonProperty("id")]
        public string Mbid { get; set; }

        [JsonProperty("primary-type")]
        public string PrimaryType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}