using System;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Demos2
{
    public static class Blocks
    {
        public static IObservable<T> WithPerfLogging<T>(IObservable<T> sequence)
        {
            var stopwatch = new Stopwatch();
            return Observable.Defer(() =>
                {
                    stopwatch.Start();
                    return sequence;
                })
                .Finally(() =>
                {
                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.Elapsed);
                });
        }
    }
}