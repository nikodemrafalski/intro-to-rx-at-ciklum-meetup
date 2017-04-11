using System;
using System.Reactive;
using System.Reactive.Subjects;
using Console = System.Console;

namespace Demos
{
    public class Subjects
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
                Console.WriteLine("Observer {0} - value:  {1}", _name, value);
            }

            protected override void OnErrorCore(Exception error)
            {
                Console.WriteLine("Observer {0} - error:  {1}", _name, error);
            }

            protected override void OnCompletedCore()
            {
                Console.WriteLine("Observer {0} - completed:", _name);
            }
        }

        public static void Subject()
        {
            Console.WriteLine("----------SUBJECT");
            var subject = new Subject<int>();
            subject.Subscribe(new Observer("A"));
            subject.OnNext(1);
            subject.OnNext(11);
            subject.OnNext(111);
            subject.Subscribe(new Observer("B"));
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
            subject.OnNext(4);
            Console.ReadKey();
        }

        public static void BehaviorSubject()
        {
            Console.WriteLine("----------BEHAVIOR SUBJECT");
            var subject = new BehaviorSubject<int>(10000);
            subject.Subscribe(new Observer("A"));
            subject.OnNext(1);
            subject.OnNext(11);
            subject.OnNext(111);
            subject.Subscribe(new Observer("B"));
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
            subject.OnNext(4);
            Console.ReadKey();
        }

        public static void ReplaySubject()
        {
            Console.WriteLine("----------REPLAY SUBJECT(2)");
            var subject = new ReplaySubject<int>(2);
            subject.Subscribe(new Observer("A"));
            subject.OnNext(1);
            subject.OnNext(11);
            subject.OnNext(111);
            subject.Subscribe(new Observer("B"));
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
            subject.OnNext(4);
            Console.ReadKey();
        }

        public static void AsyncSubject()
        {
            Console.WriteLine("----------ASYNC SUBJECT(2)");
            var subject = new AsyncSubject<int>();
            subject.Subscribe(new Observer("A"));
            subject.OnNext(1);
            subject.OnNext(11);
            subject.OnNext(111);
            subject.Subscribe(new Observer("B"));
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted(); // important! (subject.OnError(new Exception()));
            subject.OnNext(4);
            Console.ReadKey();
        }
    }
}