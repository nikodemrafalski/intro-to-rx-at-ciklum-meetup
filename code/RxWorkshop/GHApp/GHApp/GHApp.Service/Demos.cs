using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GHApp.Contracts;
using GHApp.Contracts.Dto;

namespace GHApp.Service
{
    public class Demos
    {
        private readonly IGithubBrowser _githubBrowser;
        private readonly IGitHubUserSearchService _userSearchService;

        public Demos()
        {
            var factory = new HttpClientFactory();
            _githubBrowser = new GithubBrowser(factory);
            _userSearchService = new GitHubUserSearchService(factory);
        }

        public void Demo1()
        {
            _githubBrowser
                .GetUser("nikodemrafalski")
                .Subscribe(v => { }, error => { }, () => { });
            Console.ReadKey();
        }

        public void Demo2()
        {
            var someRepo = new Repo
            {
                CommitsUrl = new Uri("https://api.github.com/repos/nikodemrafalski/testrepo/commits{/sha}")
            };

            _githubBrowser
                .GetCommits(someRepo)
                .Subscribe(v => { }, error => { }, () => { });
            Console.ReadKey();
        }

        public void Demo3()
        {
            var namesToLook = new Subject<string>();

            var sub = namesToLook.Select(partialName => _userSearchService.FindUser(partialName))
                .Switch()
                .Subscribe(users => { Console.WriteLine(string.Join(",", users.Select(p => p.Login))); });

            Console.ReadKey();
            namesToLook.OnNext("a");
            Console.ReadKey();
           // sub.Dispose();
            namesToLook.OnNext("b");
            Console.ReadKey();
            namesToLook.OnNext("c");
            Console.WriteLine("done!");
            Console.ReadKey();
        }

        public void RepoWatcherComponentDemo()
        {
            var someRepo = new Repo
            {
                CommitsUrl = new Uri("https://api.github.com/repos/nikodemrafalski/testrepo/commits{/sha}")
            };

            var watcher = new RepoWatcher(_githubBrowser, someRepo, TaskPoolScheduler.Default);
            watcher.NewCommits.Subscribe(c => Console.WriteLine("new commit! " + c.Sha + " " + c.CommitInfo.Message));
            Console.ReadKey();
        }

        public void WatchRepoDemo()
        {
            var someRepo = new Repo
            {
                CommitsUrl = new Uri("https://api.github.com/repos/nikodemrafalski/testrepo/commits{/sha}")
            };

            _githubBrowser.NewCommitsFeed.Subscribe(
                c => Console.WriteLine("new commit! " + c.Sha + " " + c.CommitInfo.Message));
            Console.ReadKey();
            _githubBrowser.StartWatchingRepo(someRepo).Subscribe();
            Console.ReadKey();
            _githubBrowser.StopWatchingRepo(someRepo).Subscribe();
            Console.ReadKey();
        }
    }
}