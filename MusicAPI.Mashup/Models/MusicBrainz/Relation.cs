namespace MusicAPI.Mashup.Models.MusicBrainz
{
    using Newtonsoft.Json;

    public class Relation
    {
        [JsonProperty("url")]
        public Url Url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}