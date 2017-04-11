using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GHApp.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                    h => SearchText.TextChanged += h,
                    h => SearchText.TextChanged -= h
                )
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOnDispatcher()
                .Subscribe(p => SearchButton.Command.Execute(SearchButton.CommandParameter));
        }
    }
}