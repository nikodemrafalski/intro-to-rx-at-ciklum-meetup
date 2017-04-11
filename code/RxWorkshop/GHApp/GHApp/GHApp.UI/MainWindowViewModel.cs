using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using GHApp.Communication;
using GHApp.Contracts.Dto;
using GHApp.Contracts.Notifications;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IService<FavQuery, FavResponse> _favService;
        private readonly IService<RepoQuery, RepoResponse> _repoService;
        private readonly ITopic<RepoNotification> _repoTopic;
        private readonly IService<UserQuery, UserResponse> _userService;

        private ICommand _addToFavouritesCommand;

        private ICommand _findUserCommand;

        private ICommand _getReposCommand;

        private string _property;

        private string _searchText;

        public MainWindowViewModel(IService<UserQuery, UserResponse> userService,
            IService<RepoQuery, RepoResponse> repoService, IService<FavQuery, FavResponse> favService,
            ITopic<RepoNotification> repoTopic)
        {
            _userService = userService;
            _repoService = repoService;
            _favService = favService;
            _repoTopic = repoTopic;

            _repoTopic.Messages
                .Select(n => n.Commit)
                .ObserveOnDispatcher()
                .Subscribe(Commits.Add);

            PropertyChangedStream.Where(p => p != "Property").Subscribe(p => Property = p);
        }

        public ObservableCollection<Commit> Commits { get; set; } = new ObservableCollection<Commit>();

        public ObservableCollection<User> UserSearchResults { get; set; } = new ObservableCollection<User>();

        public ObservableCollection<Repo> RepoSearchResults { get; set; } = new ObservableCollection<Repo>();

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string Property
        {
            get => _property;
            set
            {
                _property = value;
                OnPropertyChanged();
            }
        }

        public ICommand FindUserCommand => _findUserCommand ?? (_findUserCommand = new ReactiveCommand(FindUser));

        public ICommand GetReposCommand => _getReposCommand ?? (_getReposCommand = new ReactiveCommand(GetRepos));

        public ICommand AddToFavouritesCommand => _addToFavouritesCommand ??
                                                  (_addToFavouritesCommand = new ReactiveCommand(AddToFavourites));

        private void FindUser(object parameter)
        {
            UserSearchResults.Clear();
            var user = parameter == null ? null : parameter.ToString();
            if (user != null)
                _userService.Query(new UserQuery(user))
                    .ObserveOnDispatcher()
                    .Subscribe(res => { res.Users.ForEach(UserSearchResults.Add); });
        }

        private void GetRepos(object parameter)
        {
            RepoSearchResults.Clear();
            var user = parameter as User;
            if (user != null)
                _repoService.Query(new RepoQuery(user))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe(res => { res.Repos.ForEach(RepoSearchResults.Add); });
        }

        private void AddToFavourites(object parameter)
        {
            var repo = parameter as Repo;
            if (repo != null)
                _favService.Query(new FavQuery(repo))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe();
        }
    }
}