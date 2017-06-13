namespace MusicAPI.Mashup.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Moq;

    using MusicAPI.Mashup.Models.MusicBrainz;
    using MusicAPI.Mashup.Models.Wikipedia;
    using MusicAPI.Mashup.Repositories;
    using MusicAPI.Mashup.Services;

    using NUnit.Framework;

    public class MashupServiceTests
    {
        private Mock<IMusicBrainzRepository> musicBrainzRepository;
        private Mock<IWikipediaRepository> wikipediaRepository;
        private Mock<ICoverArtArchiveRepository> coverArtRepository;

        [Test]
        public async Task ShouldCallMusicBrainzWithMbid()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_1", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_1", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_1");

            // Assert
            this.musicBrainzRepository.Verify(m => m.GetArtistFromMusicBrainz("mbid_1"), Times.Once);
        }

        [Test]
        public async Task ShouldCallCoverArtArchiveWithAlbumMbid()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_2", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_2", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_2");

            // Assert
            this.coverArtRepository.Verify(c => c.GetCoverArt(releaseGroups), Times.Once);
        }

        [Test]
        public async Task ShouldCallWikipediaWithRelations()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_3", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_3", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_3");

            // Assert
            this.wikipediaRepository.Verify(c => c.GetWikipediaBiography(relations), Times.Once);
        }

        [Test]
        public async Task ShouldCacheMusicBrainzResponse()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_4", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_4", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_4");
            await systemUnderTest.GetMashup("mbid_4");

            // Assert
            this.musicBrainzRepository.Verify(c => c.GetArtistFromMusicBrainz("mbid_4"), Times.Exactly(1));
        }

        [Test]
        public async Task ShouldCacheWikipediaResponse()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_5", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_5", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_5");
            await systemUnderTest.GetMashup("mbid_5");

            // Assert
            this.wikipediaRepository.Verify(c => c.GetWikipediaBiography(relations),Times.Exactly(1));
        }

        [Test]
        public async Task ShouldCacheCoverArtResponse()
        {
            // Arrange
            var releaseGroups = new[] { new RealeaseGroup { Mbid = "mbid_6", PrimaryType = "album", Title = "test" } };
            var relations = new[] { new Relation { Type = "wikipedia" } };
            var coverArts = new Dictionary<string, string[]> { { "mbid_6", new[] { "länk" } } };

            this.SetUpDependencies(releaseGroups, relations, coverArts);

            var systemUnderTest = new MashupService(
                this.musicBrainzRepository.Object,
                this.wikipediaRepository.Object,
                this.coverArtRepository.Object);

            // Act
            await systemUnderTest.GetMashup("mbid_6");
            await systemUnderTest.GetMashup("mbid_6");

            // Assert
            this.coverArtRepository.Verify(c => c.GetCoverArt(releaseGroups),Times.Exactly(1));
        }

        private void SetUpDependencies(RealeaseGroup[] releaseGroups, Relation[] relations, Dictionary<string, string[]> coverArts)
        {
            this.musicBrainzRepository = new Mock<IMusicBrainzRepository>();
            var musicBrainzResponse = new MusicBrainzResponseModel { ReleaseGroups = releaseGroups, Relations = relations };

            this.musicBrainzRepository.Setup(m => m.GetArtistFromMusicBrainz(It.IsAny<string>()))
                .ReturnsAsync(musicBrainzResponse);

            this.wikipediaRepository = new Mock<IWikipediaRepository>();

            var pages = new Dictionary<string, PageIdObject> { { "key", new PageIdObject() } };
            var wikiResponse = new WikipediaBiographyResponseModel { Queries = new Query { Pages = pages } };

            this.wikipediaRepository.Setup(w => w.GetWikipediaBiography(It.IsAny<IEnumerable<Relation>>()))
                .ReturnsAsync(wikiResponse);

            this.coverArtRepository = new Mock<ICoverArtArchiveRepository>();
            this.coverArtRepository.Setup(c => c.GetCoverArt(It.IsAny<IEnumerable<RealeaseGroup>>())).ReturnsAsync(coverArts);
        }
    }
}
