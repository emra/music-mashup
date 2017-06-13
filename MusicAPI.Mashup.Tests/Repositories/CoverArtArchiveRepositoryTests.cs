namespace MusicAPI.Mashup.Tests.Repositories
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MusicAPI.Mashup.Models.MusicBrainz;
    using MusicAPI.Mashup.Repositories;

    using NUnit.Framework;

    [TestFixture]
    public class CoverArtArchiveRepositoryTests
    {
        [Test]
        public async Task ShouldReturnEmptyArrayWhenImageWasNotFound()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            var messageHandler = new FakeHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://test.se") };

            var systemUnderTest = new CoverArtArchiveRepository(httpClient);

            // Act
            var result = await systemUnderTest.GetCoverArt(new[] { new RealeaseGroup { Mbid = "mbid" } });

            // Assert
            Assert.That(result["mbid"].Length.Equals(0));
        }
    }
}
