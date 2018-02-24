using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    class Program
    {
        static void Main(string[] args)
        {
            Sam sam = new Sam();
            sam.FindCandidates();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
