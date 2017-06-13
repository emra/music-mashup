namespace MusicAPI.Mashup.Tests.Repositories
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using MusicAPI.Mashup.Repositories;

    using NUnit.Framework;

    using Assert = NUnit.Framework.Assert;
    
    [TestFixture]
    public class MusicBrainzRepositoryTests
    {
        [Test]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadRequest)]
        public async Task ShouldThrowHttpResponseExceptionWithSameResponseAsMusicBrainzIfUnsuccessfulResponse(
            HttpStatusCode musicBrainzResponseCode)
        {
            // Arrange
            var responseMessage = new HttpResponseMessage { StatusCode = musicBrainzResponseCode };

            var messageHandler = new FakeHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://test.se") };

            var systemUnderTest = new MusicBrainzRepository(httpClient);

            // Act
            try
            {
                await systemUnderTest.GetArtistFromMusicBrainz("mbid");
            }

            // Assert
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(musicBrainzResponseCode, ex.Response.StatusCode);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        [Ignore("Not mockable with current implementation")]
        public async Task ShouldTryAgainWhenReceiving503FromMusicBrainz()
        {
            var responseMessage =
                new HttpResponseMessage { StatusCode = HttpStatusCode.ServiceUnavailable };

            var messageHandler = new FakeHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(messageHandler) { BaseAddress = new Uri("http://test.se") };

            var systemUnderTest = new MusicBrainzRepository(httpClient);

            // Act
            await systemUnderTest.GetArtistFromMusicBrainz("mbid");

            // Assert
            throw new NotImplementedException();
        }
    }
}
