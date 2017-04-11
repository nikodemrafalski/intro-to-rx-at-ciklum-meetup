using System;
using System.Collections.Generic;
using System.Reactive;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts
{
    public interface IGithubBrowser
    {
        IObservable<Commit> NewCommitsFeed { get; }
        IObservable<User> GetUser(string username);

        IObservable<IEnumerable<Repo>> GetRepos(User user);

        IObservable<IEnumerable<Commit>> GetCommits(Repo repo);

        IObservable<Unit> StopWatchingRepo(Repo repo);

        IObservable<Unit> StartWatchingRepo(Repo repo);
    }
}