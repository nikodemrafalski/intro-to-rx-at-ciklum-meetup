using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Demos1
{
    class Program
    {
        public static void Main()
        {
            IObservable<int> _source = null;

            _source = Observable.Empty<int>();
            _source = Observable.Return(1);
            _source = Observable.Repeat(1, 3);
            _source = Observable.Never<int>();
            //_source = Observable.Range(1, 100);
            //_source = Observable.Range(1, 1000000000, TaskPoolScheduler.Default);
            _source = Observable.Throw<int>(new ArgumentException());

            _source = Observable.Create<int>(observer =>
            {
                int i = 0;
                while (i - 1 < 100)
                {
                    observer.OnNext(i++);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });

            _source = Observable //StartsWith
                .Timer(TimeSpan.FromSeconds(1))
                .Select(i => (int)i);

            _source = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(i => (int) i);

            _source = Observable.Generate(
                0, // seed value
                i => i < 10, //condition 
                i => i + 1, //next value
                i => i * 2,  //map value
                i => TimeSpan.FromMilliseconds(i * 100)); //schedule int time

            // _source = Observable.Start(() => 10);


            //int x = 10;
            //_source = Observable.Return(x);
            //_source = Observable.Defer(() => Observable.Return(x));
            //x = 11;

            //_source = Observable.FromAsync(AsyncAwait);

            Console.WriteLine("Press any key to subscribe");
            Console.ReadLine();
            IDisposable subscription = _source.Subscribe(
                item => Console.WriteLine($"Got item: {item}"),
                exception => Console.WriteLine($"Got error: {exception}"),
                () => Console.WriteLine("Sequence completed."));

            Console.WriteLine("Press any key to unsubscribe");
            Console.ReadLine();
            subscription.Dispose();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static async Task<int> AsyncAwait()
        {
            return await Task.FromResult(111);
        }
    }
}