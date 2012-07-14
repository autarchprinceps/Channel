using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Channel
{
    static class Core
    {
        static Channel<int> ch;
        [MTAThread]
        static void Main()
        {
            r = new Random();
            ch = new Channel<int>();
            Thread t1 = new Thread(new ThreadStart(test1));
            Thread t2 = new Thread(new ThreadStart(test1));
            Thread s = new Thread(new ThreadStart(test2));
            s.Start();
            t1.Start();
            t2.Start();
            System.Console.ReadLine();
        }
        static Random r;

        static void test1()
        {
            for (int i = 10000; i >= 1000; i--)
            {
                ch.give(i);
                Thread.Sleep(r.Next(1000) + 1);
            }
        }

        static void test2()
        {
            while (true)
            {
                System.Console.WriteLine(ch.take());
            }
        }
    }
}
