namespace MusicAPI.Mashup.Models.CoverArtArchive
{
    using Newtonsoft.Json;

    public class Image
    {
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
    }
}