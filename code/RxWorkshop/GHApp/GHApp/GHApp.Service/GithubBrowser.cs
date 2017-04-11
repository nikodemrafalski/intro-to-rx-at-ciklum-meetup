using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using Newtonsoft.Json;

namespace GHApp.Service
{
    public class GithubBrowser : IGithubBrowser
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Subject<Commit> _feedSubject = new Subject<Commit>();
        private readonly SerialDisposable _serial = new SerialDisposable();
        private readonly Dictionary<Repo, RepoWatcher> _watchers = new Dictionary<Repo, RepoWatcher>();

        public GithubBrowser(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IObservable<User> GetUser(string username)
        {
            var uri = new Uri(string.Format("https://api.github.com/users/{0}", username));
            return WrapHttpGet<User>(uri);
        }

        public IObservable<IEnumerable<Repo>> GetRepos(User user)
        {
            return WrapHttpGet<IEnumerable<Repo>>(user.ReposUrl);
        }

        public IObservable<IEnumerable<Commit>> GetCommits(Repo repo)
        {
            var uri = new Uri(repo.CommitsUrl.ToString().Replace("{/sha}", string.Empty));
            return WrapHttpGet<IEnumerable<Commit>>(uri);
        }

        public IObservable<Unit> StartWatchingRepo(Repo repo)
        {
            return Observable.Create<Unit>(obs =>
            {
                if (!_watchers.ContainsKey(repo))
                {
                    _watchers.Add(repo, new RepoWatcher(this, repo, TaskPoolScheduler.Default));
                    RebuildCommitsFeed();
                }

                obs.OnNext(Unit.Default);
                obs.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<Unit> StopWatchingRepo(Repo repo)
        {
            return Observable.Start(() =>
            {
                RepoWatcher watcher;
                if (_watchers.TryGetValue(repo, out watcher))
                {
                    _watchers.Remove(repo);
                    RebuildCommitsFeed();
                    watcher.Dispose();
                }
            });
        }

        public IObservable<Commit> NewCommitsFeed => _feedSubject.AsObservable();

        private IObservable<TResult> WrapHttpGet<TResult>(Uri uri)
        {
            return
                Observable
                    .Using(
                        _clientFactory.CreateHttpClient,
                        client => Observable.FromAsync(() => client.GetStringAsync(uri)))
                    .Select(JsonConvert.DeserializeObject<TResult>);
        }

        private void RebuildCommitsFeed()
        {
            _serial.Disposable = _watchers.Values
                .Select(x => x.NewCommits)
                .Merge()
                .Subscribe(_feedSubject.OnNext);
        }
    }
}