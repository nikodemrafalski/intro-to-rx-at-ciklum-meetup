using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WpfClient
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ReceiverA _eventsBasedReceiverA = new ReceiverA();
        private readonly ReceiverB _rxBasedReceiverB = new ReceiverB();
        private CompositeDisposable _disposable = new CompositeDisposable();
        private string _currentHtml;
        private string _currentLink;
        private string _prime;
        private string _property;
        private IEnumerable<string> _images = new List<string>();


        public MainWindowViewModel()
        {
            _eventsBasedReceiverA.LinkReceived += (s, a) => CurrentLink = a.Link;
            _eventsBasedReceiverA.HtmlReceived += (s, a) => CurrentHtml = a.Html;

            ConnectA = new ActionCommand(_ => { _eventsBasedReceiverA.Start(); });

            // show error handling, fallbacks
            // show zipping

            _rxBasedReceiverB.LinksStream.Subscribe(link => CurrentLink = link);
            _rxBasedReceiverB.HtmlStream.Subscribe(html => CurrentHtml = html);
            _rxBasedReceiverB.HtmlStream
                .Select(html => Regex.Matches(html, "http.*png")
                    .Cast<Match>()
                    .Select(x => x.Value))
                .Do(links => File.AppendAllLines(@"c:/users/nixon/Desktop/png.txt", links))
                .Subscribe(links => Images = links);

            ConnectB = new ActionCommand(_ => { _rxBasedReceiverB.Start(); });

            PropertyChangedStream.Subscribe(pName => Property = pName);
            Slow1 = new ActionCommand(_ => DoSlow1());
            SlowThr = new ActionCommand(_ => DoSlowThr());
            SlowRx = new ActionCommand(_ => DoSlowRx());
            //Unsubscribe = new ActionCommand(_ =>
            //{
            //    _disposable.Dispose();
            //    _disposable = new CompositeDisposable();
            //});
        }

        public ICommand ConnectA { get; }
        public ICommand ConnectB { get; set; }

        public ICommand Slow1 { get; }
        public ICommand SlowThr { get; set; }
        public ICommand SlowRx { get; }

        public ICommand Unsubscribe { get; }

        public string Property
        {
            get => _property;
            set
            {
                _property = value;
                OnPropertyChanged();
            }
        }

        public string CurrentLink
        {
            get => _currentLink;
            set
            {
                _currentLink = value;
                OnPropertyChanged();
            }
        }

        public string CurrentHtml
        {
            get => _currentHtml;
            set
            {
                _currentHtml = value;
                OnPropertyChanged();
            }
        }

        public string Prime
        {
            get => _prime;
            set
            {
                _prime = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        public void DoSlow1()
        {
            Prime = BurnCpu.FindPrimeNumber(500000).ToString();
        }

        public void DoSlowThr()
        {
            // ThreadPool.QueueUserWorkItem(_ => Prime = BurnCpu.FindPrimeNumber(500000));
            new Thread(_ => Prime = BurnCpu.FindPrimeNumber(500000).ToString()).Start();
        }

        public void DoSlowRx()
        {
            BurnCpu.FindPrimeNumbers(500000)
                .SubscribeOn(NewThreadScheduler.Default)
                .Subscribe(prime => Prime = prime.ToString());
        }
    }
}