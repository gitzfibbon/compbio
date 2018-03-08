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
            string output = Program.Run2a();

            Console.WriteLine(output);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static string Run2a()
        {
            StringBuilder sb = new StringBuilder();

            double secsToFindCandidates = 0;
            double secsToMotifScan = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Sam sam = new Sam();
            string info = sam.FindCandidates(@"data\hw5-candidates-alpha.txt");
            sb.AppendLine("------------------");
            sb.AppendLine("Info");
            sb.AppendLine("------------------");
            sb.Append(info);
            sb.AppendLine();

            secsToFindCandidates = sw.Elapsed.TotalSeconds;
            sw.Reset();

            List<MotifScan> motifScans0 = new List<MotifScan>();
            List<MotifScan> motifScans1 = new List<MotifScan>();
            foreach (Read read in sam.Candidates)
            {
                MotifScan motifScan0 = new MotifScan("WMM0", WMM.CreateWMM0(), read);
                motifScan0.Scan();
                motifScans0.Add(motifScan0);

                MotifScan motifScan1 = new MotifScan("WMM1", WMM.CreateWMM1(), read);
                motifScan1.Scan();
                motifScans1.Add(motifScan1);


               
            }

            for (int i=0; i<motifScans0.Count; i++)
            {
                sb.AppendLine("------------------");
                sb.AppendLine("Read Candidate: " + (i + 1));
                sb.AppendLine("------------------");
                sb.AppendLine(motifScans0[i].Print());
                sb.AppendLine(motifScans1[i].Print());

            }

            sb.AppendLine();
            sb.AppendLine("------------------");
            sb.AppendLine("Summary");
            sb.AppendLine("------------------");
            sb.AppendLine();
            sb.AppendLine("WMM0 Average Distance: " + CalculateAverageDistance(motifScans0));
            sb.AppendLine("WMM1 Average Distance: " + CalculateAverageDistance(motifScans1));

            secsToMotifScan = sw.Elapsed.TotalSeconds;
            sw.Stop();

            sb.AppendLine();
            sb.AppendFormat("FindCandidates took {0} seconds", secsToFindCandidates);
            sb.AppendLine();
            sb.AppendFormat("MotifScan took {0} seconds", secsToMotifScan);
            sb.AppendLine();

            return sb.ToString();

        }

        private static double CalculateAverageDistance(List<MotifScan> motifScans)
        {
            double sum = 0;
            int count = 0;

            foreach (MotifScan motifScan in motifScans)
            {
                if (motifScan.PolyALeftIndex >= 0)
                {
                    sum += motifScan.DistanceToCleavageSite;
                    count++;
                }
            }

            return sum / count;
        }
    }
}
