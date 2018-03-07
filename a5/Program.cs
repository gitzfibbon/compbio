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
            sb.Append(info);
            sb.AppendLine();

            secsToFindCandidates = sw.Elapsed.TotalSeconds;
            sw.Reset();

            double distanceSum0 = 0;
            int includedInDistance0Avg = 0;
            double distanceSum1 = 0;
            int includedInDistance1Avg = 0;

            int i = 0;
            foreach (Read read in sam.Candidates)
            {
                MotifScan motifScan = new MotifScan();
                motifScan.ScanAll(read);

                sb.AppendLine("Read Candidate: " + (i + 1));
                if (motifScan.PolyALeftIndex_WMM0 >= 0)
                {
                    sb.AppendLine("  WMM0 LLR: " + motifScan.LLR_WMM0);
                    sb.AppendLine("  WMM0 Poly-A Site: " + read.Sequence.Substring(motifScan.PolyALeftIndex_WMM0, MotifScan.MotifLength));
                    int distance = read.CleavageSite - motifScan.PolyALeftIndex_WMM0 - MotifScan.MotifLength;
                    distanceSum0 += distance;
                    includedInDistance0Avg++;
                    sb.AppendLine("  WMM0 Distance: " + distance);
                    sb.AppendLine("  WMM0 Poly-A Left Index: " + motifScan.PolyALeftIndex_WMM0);
                    sb.AppendLine("  WMM1 Hit Count: " + motifScan.HitCount_WMM0);
                    
                }
                else
                {
                    sb.AppendLine("  WMM0: No Poly-A Site found");
                }

                if (motifScan.PolyALeftIndex_WMM1 >= 0)
                {
                    sb.AppendLine("  WMM1 LLR: " + motifScan.LLR_WMM1);
                    sb.AppendLine("  WMM1 Poly-A Site: " + read.Sequence.Substring(motifScan.PolyALeftIndex_WMM1, MotifScan.MotifLength));
                    int distance = read.CleavageSite - motifScan.PolyALeftIndex_WMM1 - MotifScan.MotifLength;
                    distanceSum1 += distance;
                    includedInDistance1Avg++;
                    sb.AppendLine("  WMM1 Distance: " + distance);
                    sb.AppendLine("  WMM1 Poly-A Left Index: " + motifScan.PolyALeftIndex_WMM1);
                    sb.AppendLine("  WMM1 Hit Count: " + motifScan.HitCount_WMM1);
                }
                else
                {
                    sb.AppendLine("  WMM1: No Poly-A Site found");
                }

                sb.AppendLine();

                i++;
            }

            sb.AppendLine("WMM0 Average Distance: " + distanceSum0 / includedInDistance0Avg);
            sb.AppendLine("WMM1 Average Distance: " + distanceSum1 / includedInDistance1Avg);


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
