using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace WpfClient
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly Subject<string> _subject;

        public ViewModelBase()
        {
            _subject = new Subject<string>();
            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h, h => PropertyChanged -= h
                )
                .Where(p => p.EventArgs.PropertyName != "Property")
                .Subscribe(p => _subject.OnNext(p.EventArgs.PropertyName));
        }

        public IObservable<string> PropertyChangedStream => _subject.AsObservable();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}