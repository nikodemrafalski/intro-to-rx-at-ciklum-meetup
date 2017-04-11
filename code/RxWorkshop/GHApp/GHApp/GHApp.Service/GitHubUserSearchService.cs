using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using Newtonsoft.Json;

namespace GHApp.Service
{
    public class GitHubUserSearchService : IGitHubUserSearchService
    {
        private readonly IHttpClientFactory _factory;

        public GitHubUserSearchService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IObservable<IEnumerable<User>> FindUser(string userNamePart)
        {
            var queryString = "https://api.github.com/search/users?q={0}&page={1}&per_page=20";

            return Observable.Using(_factory.CreateHttpClient, client =>
            {
                return Observable.Create<IEnumerable<User>>(async (o, token) =>
                {
                    var readRecords = 0;
                    var currentPage = 0;
                    var hasMoreData = true;

                    while (!token.IsCancellationRequested && hasMoreData)
                    {
                        var request = string.Format(queryString, userNamePart, currentPage);
                        var resultString = await client.GetStringAsync(request);

                        var result = JsonConvert.DeserializeObject<SearchResult>(resultString);
                        readRecords += result.Users.Length;
                        ++currentPage;
                        hasMoreData = readRecords < result.TotalCount;
                        o.OnNext(result.Users);
                    }
                    Console.WriteLine("Exiting - cancelation {0}", token.IsCancellationRequested);
                    o.OnCompleted();
                });
            });
        }
    }
}