using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GHApp.Service
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string _login;
        private readonly string _token;

        public HttpClientFactory()
        {
            _login = ConfigurationManager.AppSettings["githubLogin"];
            _token = ConfigurationManager.AppSettings["githubToken"];
        }

        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _login, _token));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Add("User-Agent", "Rx-Demo-App");
            return client;
        }
    }
}