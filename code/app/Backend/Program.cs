using System;

namespace Backend
{
    internal class Program
    {
        internal static void Main()
        {
            var publisher = new Producer(TimeSpan.FromSeconds(5));
            Console.ReadKey();
            Console.WriteLine("Start");
            publisher.Start();
            Console.ReadKey();
            publisher.Stop();
            Console.WriteLine("Stop");
            Console.ReadKey();
        }
    }
}
