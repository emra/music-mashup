namespace MusicAPI.Mashup.Tests
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class FakeHttpContent : HttpContent
    {
        public string Content { get; set; }

        public FakeHttpContent(string content)
        {
            this.Content = content;
        }

        protected async override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(this.Content);
            await stream.WriteAsync(byteArray, 0, this.Content.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = this.Content.Length;
            return true;
        }
    }
}
