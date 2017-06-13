namespace MusicAPI.Mashup.Tests.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Moq;

    using MusicAPI.Mashup.Controllers;
    using MusicAPI.Mashup.Services;

    using NUnit.Framework;

    [TestFixture]
    public class ArtistApiControllerTests
    {
        [Test]
        public async Task ShouldGetMashupFromServiceForMbid()
        {
            // Arrange
            var mashupService = new Mock<IMashupService>();
            var systemUnderTest = new ArtistApiController(mashupService.Object);

            // Act
            await systemUnderTest.Get("5b11f4ce-a62d-471e-81fc-a69a8278c7da");

            // Assert
            mashupService.Verify(m => m.GetMashup("5b11f4ce-a62d-471e-81fc-a69a8278c7da"), Times.Once);
        }

        [Test]
        public async Task ShouldReturnStatusCode404ForMbidMissingInMusicBrainz()
        {
            // Arrange
            var mashupService = new Mock<IMashupService>();
            mashupService.Setup(m => m.GetMashup(It.IsAny<string>()))
                .Throws(new HttpResponseException(HttpStatusCode.NotFound));

            var systemUnderTest = new ArtistApiController(mashupService.Object);

            // Act
            try
            {
                await systemUnderTest.Get("missing_mbid");
            }

            // Assert
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task ShouldReturnStatusCode500IfNotHttpResponseException()
        {
            // Arrange
            var mashupService = new Mock<IMashupService>();
            mashupService.Setup(m => m.GetMashup(It.IsAny<string>()))
                .Throws(new Exception());

            var systemUnderTest = new ArtistApiController(mashupService.Object);

            // Act
            var response = await systemUnderTest.Get("1234");

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.InternalServerError);
        }
    }
}
