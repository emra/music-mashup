namespace MusicAPI.Mashup.Services
{
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.Mashup;

    public interface IMashupService
    {
        Task<ArtistInformation> GetMashup(string mbid);
    }
}
