namespace MusicAPI.Mashup.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.MusicBrainz;

    public interface ICoverArtArchiveRepository
    {
        Task<Dictionary<string, string[]>> GetCoverArt(IEnumerable<RealeaseGroup> albums);
    }
}