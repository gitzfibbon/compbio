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
            string output = Program.Run();

            Console.WriteLine(output);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static string Run()
        {
            StringBuilder sb = new StringBuilder();

            double secsToFindCandidates = 0;
            double secsToMotifScan = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Sam sam = new Sam();
            string info = sam.FindCandidates();
            sb.Append(info);

            secsToFindCandidates = sw.Elapsed.TotalSeconds;
            sw.Reset();

            MotifScan motifScan = new MotifScan();

            secsToMotifScan = sw.Elapsed.TotalSeconds;

            sw.Stop();

            sb.AppendLine();
            sb.AppendFormat("FindCandidates took {0} seconds", secsToFindCandidates);
            sb.AppendLine();
            sb.AppendFormat("MotifScan took {0} seconds", secsToMotifScan);
            sb.AppendLine();

            return sb.ToString();

        }
    }
}
