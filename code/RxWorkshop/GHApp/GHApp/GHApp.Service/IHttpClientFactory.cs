using System.Net.Http;

namespace GHApp.Service
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}