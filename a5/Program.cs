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
            //string output = Program.Run2a();

            Console.WriteLine(output);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static string Run()
        {
            StringBuilder sb = new StringBuilder();

            Sam sam2a = new Sam();
            string fileName2a = @"data\hw5-candidates-alpha.txt";
            string info2a = sam2a.FindCandidates(fileName2a);

            sb.AppendLine("------------------");
            sb.AppendLine("Candidate Set 2a");
            sb.AppendLine("------------------");
            sb.AppendLine("File: " + fileName2a);
            sb.Append(info2a);
            sb.AppendLine();

            Meme meme2a = new Meme(sam2a, WMM.CreateWMM1());
            WMM wmm2a = meme2a.Run();

            List<MotifScan> motifScans0 = new List<MotifScan>();
            List<MotifScan> motifScans1 = new List<MotifScan>();
            List<MotifScan> motifScans2a = new List<MotifScan>();

            foreach (Read read in sam2a.Candidates)
            {
                MotifScan motifScan0 = new MotifScan("WMM0", WMM.CreateWMM0(), read);
                motifScan0.Scan();
                motifScans0.Add(motifScan0);

                MotifScan motifScan1 = new MotifScan("WMM1", WMM.CreateWMM1(), read);
                motifScan1.Scan();
                motifScans1.Add(motifScan1);

                MotifScan motifScan2a = new MotifScan("WMM2a", wmm2a, read);
                motifScan2a.Scan();
                motifScans2a.Add(motifScan2a);
            }

            sb.AppendLine("WMM0 Average Distance: " + CalculateAverageDistance(motifScans0));
            sb.AppendLine("WMM0 Relative Entropy: " + WMM.CreateWMM0().RelativeEntropy);
            sb.AppendLine("WMM0 Candidates with at least one hit: " + motifScans0.Count(ms => ms.TotalHitCount > 0));
            sb.AppendLine();
            sb.AppendLine("WMM1 Average Distance: " + CalculateAverageDistance(motifScans1));
            sb.AppendLine("WMM1 Relative Entropy: " + WMM.CreateWMM1().RelativeEntropy);
            sb.AppendLine("WMM1 Candidates with at least one hit: " + motifScans1.Count(ms => ms.TotalHitCount > 0));
            sb.AppendLine();
            sb.AppendLine("WMM2a Average Distance: " + CalculateAverageDistance(motifScans2a));
            sb.AppendLine("WMM2a Relative Entropy: " + wmm2a.RelativeEntropy);
            sb.AppendLine("WMM2a Candidates with at least one hit: " + motifScans2a.Count(ms => ms.TotalHitCount > 0));
            sb.AppendLine();

            
            // Filtered down, managable set of just the 0x4 matches
            Sam sam2b = new Sam();
            //string fileName2b = @"C:\Users\jordanf\Downloads\SRR5831944.resorted2.sam";
            string fileName2b = @"C:\Users\jordanf\Downloads\CandidateSuperset.sam";
            string info2b = sam2b.FindCandidates(fileName2b);

            sb.AppendLine();
            sb.AppendLine("------------------");
            sb.AppendLine("Candidate Set 2b");
            sb.AppendLine("------------------");
            sb.AppendLine("File: " + fileName2b);
            sb.Append(info2b);
            sb.AppendLine();

            Meme meme2b = new Meme(sam2b, WMM.CreateWMM1());
            WMM wmm2b = meme2b.Run();

            motifScans0 = new List<MotifScan>();
            motifScans1 = new List<MotifScan>();
            List<MotifScan> motifScans2b = new List<MotifScan>();

            foreach (Read read in sam2b.Candidates)
            {
                MotifScan motifScan0 = new MotifScan("WMM0", WMM.CreateWMM0(), read);
                motifScan0.Scan();
                motifScans0.Add(motifScan0);

                MotifScan motifScan1 = new MotifScan("WMM1", WMM.CreateWMM1(), read);
                motifScan1.Scan();
                motifScans1.Add(motifScan1);

                MotifScan motifScan2b = new MotifScan("WMM2b", wmm2b, read);
                motifScan2b.Scan();
                motifScans2b.Add(motifScan2b);
            }

            sb.AppendLine("------------------");
            sb.AppendLine("Results");
            sb.AppendLine("------------------");
            sb.AppendLine();
            sb.AppendLine("WMM0 Average Distance: " + CalculateAverageDistance(motifScans0));
            sb.AppendLine("WMM0 Relative Entropy: " + WMM.CreateWMM0().RelativeEntropy);
            sb.AppendLine("WMM0 Candidates with at least one hit: " + motifScans0.Count(ms => ms.TotalHitCount > 0));
            sb.AppendLine();
            sb.AppendLine("WMM1 Average Distance: " + CalculateAverageDistance(motifScans1));
            sb.AppendLine("WMM1 Relative Entropy: " + WMM.CreateWMM1().RelativeEntropy);
            sb.AppendLine("WMM1 Candidates with at least one hit: " + motifScans1.Count(ms => ms.TotalHitCount > 0));
            sb.AppendLine();
            sb.AppendLine("WMM2b Average Distance: " + CalculateAverageDistance(motifScans2b));
            sb.AppendLine("WMM2b Relative Entropy: " + wmm2b.RelativeEntropy);
            sb.AppendLine("WMM2b Candidates with at least one hit: " + motifScans2b.Count(ms => ms.TotalHitCount > 0));

            return sb.ToString();
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

            Meme meme = new Meme(sam, WMM.CreateWMM1());
            WMM wmm2a = meme.Run();

            List<MotifScan> motifScans0 = new List<MotifScan>();
            List<MotifScan> motifScans1 = new List<MotifScan>();
            List<MotifScan> motifScans2a = new List<MotifScan>();
            foreach (Read read in sam.Candidates)
            {
                MotifScan motifScan0 = new MotifScan("WMM0", WMM.CreateWMM0(), read);
                motifScan0.Scan();
                motifScans0.Add(motifScan0);

                MotifScan motifScan1 = new MotifScan("WMM1", WMM.CreateWMM1(), read);
                motifScan1.Scan();
                motifScans1.Add(motifScan1);

                MotifScan motifScan2a = new MotifScan("WMM2a", wmm2a, read);
                motifScan2a.Scan();
                motifScans2a.Add(motifScan2a);
            }

            for (int i=0; i<motifScans0.Count; i++)
            {
                sb.AppendLine("------------------");
                sb.AppendLine("Read Candidate: " + (i + 1));
                sb.AppendLine("------------------");
                sb.AppendLine(motifScans0[i].Print());
                sb.AppendLine(motifScans1[i].Print());
                sb.AppendLine(motifScans2a[i].Print());
            }

            sb.AppendLine();
            sb.AppendLine("------------------");
            sb.AppendLine("Summary");
            sb.AppendLine("------------------");
            sb.AppendLine();
            sb.AppendLine("WMM0 Average Distance: " + CalculateAverageDistance(motifScans0));
            sb.AppendLine("WMM0 Relative Entropy: " + WMM.CreateWMM0().RelativeEntropy);
            sb.AppendLine("WMM1 Average Distance: " + CalculateAverageDistance(motifScans1));
            sb.AppendLine("WMM1 Relative Entropy: " + WMM.CreateWMM1().RelativeEntropy);
            sb.AppendLine("WMM2a Average Distance: " + CalculateAverageDistance(motifScans2a));
            sb.AppendLine("WMM2a Relative Entropy: " + wmm2a.RelativeEntropy);

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
