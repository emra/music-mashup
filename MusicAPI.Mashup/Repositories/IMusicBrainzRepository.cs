namespace MusicAPI.Mashup.Repositories
{
    using System.Threading.Tasks;
    
    using MusicAPI.Mashup.Models.MusicBrainz;

    public interface IMusicBrainzRepository
    {
        Task<MusicBrainzResponseModel> GetArtistFromMusicBrainz(string mbid);
    }
}
