using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Sam sam = new Sam();
            sam.FindCandidates();

            sw.Stop();
            Console.WriteLine("Program took {0} seconds", sw.Elapsed.TotalSeconds);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
