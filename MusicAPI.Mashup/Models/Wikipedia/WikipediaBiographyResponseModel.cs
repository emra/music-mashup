namespace MusicAPI.Mashup.Models.Wikipedia
{
    using Newtonsoft.Json;

    public class WikipediaBiographyResponseModel
    {
        [JsonProperty("query")]
        public Query Queries { get; set; }
    }
}