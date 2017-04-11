using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace Demos
{
    public class Schedulers
    {
        private class Observer : ObserverBase<int>
        {
            private readonly string _name;

            public Observer(string name)
            {
                _name = name;
            }

            protected override void OnNextCore(int value)
            {
                Console.WriteLine("Observer {0} - Thread {1} - value:  {2}",
                    _name, Thread.CurrentThread.ManagedThreadId, value);
            }

            protected override void OnErrorCore(Exception error)
            {
                Console.WriteLine("Observer {0} - Thread {1} - error:  {2}",
                    _name, Thread.CurrentThread.ManagedThreadId, error);
            }

            protected override void OnCompletedCore()
            {
                Console.WriteLine("Observer {0} - Thread {1} - completed:",
                    _name, Thread.CurrentThread.ManagedThreadId);
            }
        }

        private static IObservable<int> CreateObservable()
        {
            IObservable<int> observable = Observable.Create<int>(observer =>
            {
                Console.WriteLine("Producing value 1 - Thread {0}", Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(1);
                Console.WriteLine("Producing value 2 - Thread {0}", Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(2);
                Console.WriteLine("Produced all values - Thread {0}", Thread.CurrentThread.ManagedThreadId);
                observer.OnCompleted();
                return
                    Disposable.Create(
                        () =>
                            Console.WriteLine("Disposing subscription - Thread {0}",
                                Thread.CurrentThread.ManagedThreadId));
            });

            return observable;
        }

        public static void CurrThreadScheduler()
        {
            Console.WriteLine("-----CURRENT THREAD SCHEDULER");
            SchedulersDemo(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance);
            Console.WriteLine();
        }

        public static void CurrThreadPlusNewThreadScheduler()
        {
            Console.WriteLine("-----CURRENT THREAD + Observe on new thread SCHEDULER");
            SchedulersDemo(CurrentThreadScheduler.Instance, NewThreadScheduler.Default);
            Console.WriteLine();
        }

        public static void CurrThreadPlusThreadPoolScheduler()
        {
            Console.WriteLine("-----CURRENT THREAD + Observe on  thread pool SCHEDULER");
            SchedulersDemo(CurrentThreadScheduler.Instance, ThreadPoolScheduler.Instance);
            Console.WriteLine();
        }

        public static void NewThreadPlusCurrScheduler()
        {
            Console.WriteLine("-----NEW THREAD + curr thread pool SCHEDULER");
            SchedulersDemo(NewThreadScheduler.Default, CurrentThreadScheduler.Instance);
            Console.WriteLine();
        }

        public static void NewThreadPlusImmediateScheduler()
        {
            Console.WriteLine("-----NEW THREAD + immediate thread pool SCHEDULER");
            SchedulersDemo(NewThreadScheduler.Default, ImmediateScheduler.Instance);;
            Console.WriteLine();
        }

        public static void ImmediateSchedulerSubOb()
        {
            Console.WriteLine("-----IMMEDIATE BOTH");
            SchedulersDemo(ImmediateScheduler.Instance, ImmediateScheduler.Instance);
            Console.WriteLine();
        }

        public static void ImmediateSchedulerSubThreadPoolOb()
        {
            Console.WriteLine("-----IMMEDIATE PROD + Task pool consumer");
            SchedulersDemo(ImmediateScheduler.Instance, TaskPoolScheduler.Default);
            Console.WriteLine();
        }

        public static void ImmediateSchedulerEventLoopObs()
        {
            var eventLoop = new EventLoopScheduler(ts => new Thread(ts) { IsBackground = true });
            Console.WriteLine("-----IMMEDIATE PROD + EventLooop");
            SchedulersDemo(ImmediateScheduler.Instance, eventLoop);
            Console.WriteLine();
        }

        public static void SchedulersDemo(IScheduler subscribeOn, IScheduler observeOn)
        {
            Console.WriteLine("Before sub - Thread : {0}", Thread.CurrentThread.ManagedThreadId);
            IObservable<int> obs = CreateObservable()
                .SubscribeOn(subscribeOn)
                .ObserveOn(observeOn);

            var sub1 = obs.Subscribe(new Observer("A"));
            var sub2 = obs.Subscribe(new Observer("B"));

            Console.ReadKey();
            sub1.Dispose();
            sub2.Dispose();
            Console.ReadKey();
        }
    }
}