namespace MusicAPI.Mashup.Models.Mashup
{
    public class ArtistInformation
    {
        public string Mbid { get; set; }

        public string Description { get; set; }

        public Album[] Albums { get; set; }
    }
}