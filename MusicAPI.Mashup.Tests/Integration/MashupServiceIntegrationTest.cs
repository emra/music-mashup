namespace MusicAPI.Mashup.Tests.Integration
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Repositories;
    using MusicAPI.Mashup.Services;

    using NUnit.Framework;

    [TestFixture]
    public class MashupServiceIntegrationTest
    {
        [Test]
        public async Task ShouldGetNirvanaFromMashup()
        {
            // Arrange
            const string NirvanaMbid = "5b11f4ce-a62d-471e-81fc-a69a8278c7da";
            const string NevermindCoverArt = "http://coverartarchive.org/release/a146429a-cedc-3ab0-9e41-1aaf5f6cdc2d/3012495605.jpg";
            const string NirvanaWikipediaExtract =
                "<p><b>Nirvana</b> was an American grunge band formed by singer and guitarist Kurt Cobain";

            const string UserAgent = "Music mashup API";

            var musicBrainzClient = new HttpClient { BaseAddress = new Uri("http://musicbrainz.org") };
            musicBrainzClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var wikipediaClient = new HttpClient { BaseAddress = new Uri("https://en.wikipedia.org") };
            wikipediaClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var coverArtArchiveClient = new HttpClient { BaseAddress = new Uri("http://coverartarchive.org/") };
            coverArtArchiveClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var systemUnderTest = new MashupService(
                new MusicBrainzRepository(musicBrainzClient),
                new WikipediaRepository(wikipediaClient),
                new CoverArtArchiveRepository(coverArtArchiveClient));

            // Act
            var result = await systemUnderTest.GetMashup(NirvanaMbid);

            // Assert
            Assert.That(result.Mbid.Equals(NirvanaMbid));
            Assert.That(result.Albums.Any(album => album.Images.Contains(NevermindCoverArt)));
            Assert.That(result.Description.Contains(NirvanaWikipediaExtract));
        }
    }
}
