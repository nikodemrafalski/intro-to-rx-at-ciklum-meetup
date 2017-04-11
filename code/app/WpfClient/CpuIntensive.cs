using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace WpfClient
{
    public static class BurnCpu
    {
        public static long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }

        public static IObservable<long> FindPrimeNumbers(int n)
        {
            return Observable.Create<long>(obs =>
            {
                int count = 0;
                long a = 2;
                while (count < n)
                {
                    long b = 2;
                    int prime = 1;
                    while (b * b <= a)
                    {
                        if (a % b == 0)
                        {
                            prime = 0;
                            break;
                        }
                        b++;
                    }
                    if (prime > 0)
                    {
                        count++;
                        obs.OnNext(a);
                    }
                    a++;
                }
                obs.OnNext(--a);
                obs.OnCompleted();
                return Disposable.Empty;
            });
        }
    }
}