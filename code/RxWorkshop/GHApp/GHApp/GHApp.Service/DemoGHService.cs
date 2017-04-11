using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using Newtonsoft.Json;

namespace GHApp.Service
{
    public class DemoGHService : IGithubBrowser
    {
        private readonly IHttpClientFactory _clientFactory = new HttpClientFactory();
        public IObservable<User> GetUser(string username)
        {
            return WrapHttpGet<User>(new Uri($"https://api.github.com/users/{username}"));
        }

        private IObservable<TResult> WrapHttpGet<TResult>(Uri uri)
        {
            return
                Observable
                    .Using(
                        _clientFactory.CreateHttpClient,
                        client => Observable.FromAsync(() => client.GetStringAsync(uri)))
                    .Select(JsonConvert.DeserializeObject<TResult>);
        }

        public IObservable<IEnumerable<Repo>> GetRepos(User user)
        {
            throw new NotImplementedException();
        }

        public IObservable<IEnumerable<Commit>> GetCommits(Repo repo)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> StopWatchingRepo(Repo repo)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> StartWatchingRepo(Repo repo)
        {
            throw new NotImplementedException();
        }

        public IObservable<Commit> NewCommitsFeed { get; }
    }
}