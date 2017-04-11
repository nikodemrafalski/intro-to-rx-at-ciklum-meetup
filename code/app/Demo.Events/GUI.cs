using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using static Demos2.Blocks;

namespace Demos2
{
    public partial class GUI : Form
    {
        private IDisposable _subscription = Disposable.Empty;
        public GUI()
        {
            InitializeComponent();
            //_subscription = Observable
            //    .FromEventPattern<MouseEventHandler, MouseEventArgs>(
            //        handler => MouseMove += handler,
            //        handler => MouseMove -= handler)
            //    .Subscribe(e => Console.WriteLine($@"X = {e.EventArgs.X}, Y = {e.EventArgs.Y}"));

            // select
            IObservable<MouseEventArgs> mouseMoveData = Observable
            		.FromEventPattern<MouseEventHandler, MouseEventArgs>(
            		handler => MouseMove += handler,
            			handler => MouseMove -= handler)
            		.Select(x => x.EventArgs);

            //mouseMoveData.Subscribe(e => Console.WriteLine($@"X = {e.X}, Y = {e.Y}"));

            // button down missing
            //_subscription = mouseMoveData.Subscribe(e => Console.WriteLine($@"X = {e.X}, Y = {e.Y}, Buttons = {e.Button}"));

            IObservable<MouseEventArgs> mouseDownData = Observable
               .FromEventPattern<MouseEventHandler, MouseEventArgs>(
                   handler => MouseDown += handler,
                   handler => MouseDown -= handler)
               .Select(x => x.EventArgs);

            // now moves missing
            //_subscription = mouseDownData.Subscribe(e => Console.WriteLine($@"X = {e.X}, Y = {e.Y}, Buttons = {e.Button}"));

            //merge
            //_subscription = mouseMoveData
            //   .Merge(mouseDownData)
            //   .Subscribe(e => Console.WriteLine($@"X = {e.X}, Y = {e.Y}, Buttons = {e.Button}"));

            //throttling
            //http://reactivex.io/documentation/operators/debounce.html
            //_subscription = mouseMoveData

            //buffering - tripple clicks
            //http://reactivex.io/documentation/operators/buffer.html
            _subscription = mouseDownData
                .Buffer(TimeSpan.FromMilliseconds(750))
                .Where(buffer => buffer.Count > 2)
                .Select(_ => Unit.Default)
                .Subscribe(_ => Console.WriteLine(@"Tripple click"));
        }

        private void OnGetRandomButtonCLick(object sender, EventArgs e)
        {
            Observable
                .Create<string>(o =>
                {
                    Thread.Sleep(2000);
                    o.OnNext(Guid.NewGuid().ToString());
                    Thread.Sleep(500);
                    o.OnNext(Guid.NewGuid().ToString());
                    o.OnCompleted();
                    return Disposable.Empty;
                })
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .Do(x => Console.WriteLine(x))
                .Select(x => x.ToUpper())
                .ObserveOn(new ControlScheduler(_randomNumberBox))
                .Subscribe(value => _randomNumberBox.Text = value);

            //with perflogging, timeout, schedulers

            //Here say a bit more on schedulers and show separate schedulers demo.
        }

        private void OnUnsubscribeButtonClick(object sender, EventArgs e)
        {
            _subscription.Dispose();
        }
    }
}


//int i = 0;
//Observable
//.Create<int>(obs =>
//{
//i++;
//if (i< 3)
//{
//    obs.OnError(new Exception(i.ToString()));
//}
//else
//{
//    obs.OnNext(i);
//    obs.OnCompleted();
//}
//return Disposable.Empty;
//})
//.Do(Console.WriteLine, e => Console.WriteLine(e))
//.Retry(2)
//.Subscribe(Console.WriteLine, e => Console.WriteLine(e));

//Console.ReadKey();