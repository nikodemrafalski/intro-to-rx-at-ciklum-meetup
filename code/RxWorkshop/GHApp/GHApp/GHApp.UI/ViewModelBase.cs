using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace GHApp.UI
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly Subject<string> _subject;

        public ViewModelBase()
        {
            _subject = new Subject<string>();
            Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h
                )
                .Subscribe(p => _subject.OnNext(p.EventArgs.PropertyName));
        }

        public IObservable<string> PropertyChangedStream => _subject.AsObservable();

        public event PropertyChangedEventHandler PropertyChanged;

        private void PublishPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}