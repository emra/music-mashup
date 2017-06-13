namespace MusicAPI.Mashup.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Logic;
    using MusicAPI.Mashup.Models.Mashup;
    using MusicAPI.Mashup.Repositories;

    public class MashupService : IMashupService
    {
        private readonly IMusicBrainzRepository musicBrainzRepository;

        private readonly IWikipediaRepository wikipediaRepository;

        private readonly ICoverArtArchiveRepository coverArtArchiveRepository;

        public MashupService(
            IMusicBrainzRepository musicBrainzRepository,
            IWikipediaRepository wikipediaRepository,
            ICoverArtArchiveRepository coverArtArchiveRepository)
        {
            this.musicBrainzRepository = musicBrainzRepository;
            this.wikipediaRepository = wikipediaRepository;
            this.coverArtArchiveRepository = coverArtArchiveRepository;
        }

        public async Task<ArtistInformation> GetMashup(string mbid)
        {
            const int CacheTimeInMinutes = 60;

            var musicBrainzArtist = await Cache.GetObjectFromCache(
                                        $"musicBrainz_{mbid}",
                                        CacheTimeInMinutes,
                                        () => this.musicBrainzRepository.GetArtistFromMusicBrainz(mbid));

            var albums =
                musicBrainzArtist.ReleaseGroups.Where(r => r.PrimaryType.Equals("album", StringComparison.OrdinalIgnoreCase));

            var coverArtsTask = Cache.GetObjectFromCache(
                $"coverArts_{mbid}",
                CacheTimeInMinutes,
                () => this.coverArtArchiveRepository.GetCoverArt(albums));

            var wikiRelations =
                musicBrainzArtist.Relations.Where(r => r.Type.Equals("wikipedia", StringComparison.OrdinalIgnoreCase));

            var wikiBiographyTask = Cache.GetObjectFromCache(
                $"wikiBiography_{mbid}",
                CacheTimeInMinutes,
                () => this.wikipediaRepository.GetWikipediaBiography(wikiRelations));

            var wikiBiography = await wikiBiographyTask;
            var coverArts = await coverArtsTask;

            var albumResult = albums.Select(a => new Album { Title = a.Title, Id = a.Mbid, Images = coverArts[a.Mbid] });

            var result = new ArtistInformation
                             {
                                 Mbid = mbid,
                                 Description =
                                     wikiBiography.Queries.Pages.FirstOrDefault().Value.Extract,
                                 Albums = albumResult.ToArray()
                             };

            return result;
        }
    }
}