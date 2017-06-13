namespace MusicAPI.Mashup.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.MusicBrainz;
    using MusicAPI.Mashup.Models.Wikipedia;

    public interface IWikipediaRepository
    {
        Task<WikipediaBiographyResponseModel> GetWikipediaBiography(IEnumerable<Relation> wikiRelations);
    }
}