using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Subjects.Subject();
            Subjects.BehaviorSubject();
            Subjects.ReplaySubject();
            Subjects.AsyncSubject();

            Schedulers.CurrThreadScheduler();
            Schedulers.CurrThreadPlusNewThreadScheduler();
            Schedulers.CurrThreadPlusThreadPoolScheduler();
            Schedulers.NewThreadPlusCurrScheduler();
            Schedulers.ImmediateSchedulerSubOb();
            Schedulers.ImmediateSchedulerSubThreadPoolOb();
            Schedulers.ImmediateSchedulerEventLoopObs();
        }
    }
}
