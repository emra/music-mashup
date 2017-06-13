namespace MusicAPI.Mashup.Models.MusicBrainz
{
    using Newtonsoft.Json;

    public class MusicBrainzResponseModel
    {
        [JsonProperty("id")]
        public string Mbid { get; set; }

        [JsonProperty("relations")]
        public Relation[] Relations { get; set; }

        [JsonProperty("release-groups")]
        public RealeaseGroup[] ReleaseGroups { get; set; }
    }
}
