using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GHApp.Contracts;
using GHApp.Contracts.Dto;

namespace GHApp.Service
{
    public interface IRepoWatcher : IDisposable
    {
        IObservable<Commit> NewCommits { get; }
    }

    public class RepoWatcher : IRepoWatcher
    {
        private readonly TimeSpan _defaultRefreshSpan = TimeSpan.FromSeconds(10);
        private readonly Hashtable _lookup = new Hashtable();
        private readonly Subject<Commit> _newCommitsSubject = new Subject<Commit>();
        private readonly IDisposable _timerDisposal;

        public RepoWatcher(IGithubBrowser browser, Repo repoToWatch, IScheduler backgroundScheduler)
        {
            _timerDisposal = Observable
                .Interval(_defaultRefreshSpan, backgroundScheduler)
                .SelectMany(_ => browser.GetCommits(repoToWatch))
                .Subscribe(OnCommitsArrived);
        }

        public IObservable<Commit> NewCommits => _newCommitsSubject.AsObservable();

        public void Dispose()
        {
            _timerDisposal.Dispose();
        }

        private void OnCommitsArrived(IEnumerable<Commit> commits)
        {
            var newCommits = commits.Where(c => !_lookup.Contains(c.Sha)).ToList();
            newCommits.ForEach(nc => _lookup.Add(nc.Sha, nc));
            newCommits.ForEach(nc => _newCommitsSubject.OnNext(nc));
        }
    }
}