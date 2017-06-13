namespace MusicAPI.Mashup.Models.Wikipedia
{
    using Newtonsoft.Json;

    public class PageIdObject
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("extract")]
        public string Extract { get; set; }
    }
}