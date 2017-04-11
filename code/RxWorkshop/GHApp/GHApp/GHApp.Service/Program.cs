using System;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using GHApp.Communication;
using GHApp.Contracts;
using GHApp.Contracts.Notifications;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.Service
{
    public class Program
    {
        private static IGitHubUserSearchService _userService;
        private static IGithubBrowser _githubBrowser;

        public static void Main()
        {
            //new Demos().Demo1();
            //new Demos().Demo2();
            //new Demos().Demo3();
            //new Demos().RepoWatcherComponentDemo();
            //new Demos().WatchRepoDemo();

            var serverAddress = ConfigurationManager.AppSettings.Get("ServerAddress");
            var serverPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));

            var clientAddress = ConfigurationManager.AppSettings.Get("ClientAddress");
            var clientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));

            var cs = new UdpClientServer();
            var serverChannel = new UdpCommunicationChannel(cs,
                new ChannelConfig {Address = serverAddress, Port = serverPort});
            var clientChannel = new UdpCommunicationChannel(cs,
                new ChannelConfig {Address = clientAddress, Port = clientPort});

            _userService = new GitHubUserSearchService(new HttpClientFactory());
            _githubBrowser = new GithubBrowser(new HttpClientFactory());

            var userListener = new Listener<UserQuery, UserResponse>(clientChannel, serverChannel);
            userListener.ListenObservable(GetUser);

            var repoListener = new Listener<RepoQuery, RepoResponse>(clientChannel, serverChannel);
            repoListener.ListenObservable(GetRepo);

            var favListener = new Listener<FavQuery, FavResponse>(clientChannel, serverChannel);
            favListener.ListenObservable(AddToFav);

            var publisher = new Publisher<RepoNotification>(serverChannel);
            _githubBrowser.NewCommitsFeed
                .Select(c => new RepoNotification {Commit = c})
                .Subscribe(publisher);

            Console.ReadLine();
        }

        private static IObservable<UserResponse> GetUser(UserQuery query)
        {
            Console.WriteLine("Got UserQuery({0}), Id = {1}", query.Name, query.Id);
            return _userService.FindUser(query.Name)
                .Select(u => new UserResponse(query.Id, u.ToList()));
        }

        private static IObservable<RepoResponse> GetRepo(RepoQuery query)
        {
            Console.WriteLine("Got RepoQuery({0}), Id = {1}", query.User.Name, query.Id);
            return _githubBrowser.GetRepos(query.User)
                .Select(r => new RepoResponse(query.Id, r.ToList()));
        }

        private static IObservable<FavResponse> AddToFav(FavQuery query)
        {
            Console.WriteLine("Got FavQuery({0}), Id = {1}", query.Repo.Name, query.Id);
            return _githubBrowser.StartWatchingRepo(query.Repo)
                .Select(u => new FavResponse(query.Id));
        }
    }
}