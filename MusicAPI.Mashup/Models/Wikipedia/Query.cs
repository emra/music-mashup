namespace MusicAPI.Mashup.Models.Wikipedia
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Query
    {
        [JsonProperty("pages")]
        public Dictionary<string, PageIdObject> Pages { get; set; }
    }
}