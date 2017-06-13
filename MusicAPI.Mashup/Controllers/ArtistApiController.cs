namespace MusicAPI.Mashup.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using MusicAPI.Mashup.Models.Mashup;
    using MusicAPI.Mashup.Services;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ArtistApiController : ApiController
    {
        private readonly IMashupService mashupService;

        public ArtistApiController(IMashupService mashupService)
        {
            this.mashupService = mashupService;
        }

        [HttpGet]
        [Route("artist/{mbid}")]
        public async Task<HttpResponseMessage> Get(string mbid)
        {
            try
            {
                var artistInformation = await this.mashupService.GetMashup(mbid);

                var serializeSettings =
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

                var json = JsonConvert.SerializeObject(
                    new { artist = new List<ArtistInformation> { artistInformation } },
                    serializeSettings);

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return response;
            }
            catch (HttpResponseException exception)
            {
                // log exception
                throw;
            }
            catch (Exception exception)
            {
                // log exception
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
