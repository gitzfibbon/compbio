using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    public class MotifScan
    {
        public const int MotifLength = 6;
        private const double BackgroundProbability = 0.25;

        /// <summary>
        /// Eg. WMM0, WMM1
        /// </summary>
        public string Label { get; private set; }

        public WMM WMM { get; private set; }
        public double LLR { get; private set; }
        public int PolyALeftIndex { get; private set; }
        public int TotalHitCount { get; private set; }
        public int DistanceToCleavageSite { get; private set; }
        public string PolyASite { get; private set; }
        public Read Read { get; private set; }

        public MotifScan(string label, WMM wmm, Read read)
        {
            this.Label = label;
            this.WMM = wmm;
            this.Read = read;
        }

        /// <summary>
        /// Determines LLR, 0-based left side index of motif position closest to cleavage site, hit count
        /// Eg. CGAATAAACGCG.AAAAAA determines 2 as index which is the first A in AATAAA
        /// </summary>
        /// <returns></returns>
        public void Scan()
        {
            double maxLLR = 0;
            int polyALeftIndex = -1; // From the left side of the 6 element sub sequence
            int totalHitCount = 0;

            // Scan from left to right (cleavage site)
            for (int i = 0; i + MotifLength <= Read.CleavageSite; i++)
            {
                string subSequence = Read.Sequence.Substring(i, MotifLength);

                // Calculate probability the subSequence is the poly-A signal
                double llr = 0;
                for (int j = 0; j < 6; j++)
                {
                    char nt = subSequence[j];
                    int ntIndex = WMM.NTMap[nt];
                    llr += Math.Log(WMM.Matrix[ntIndex, j] / BackgroundProbability);
                }

                // The equality in the >= is important so that we take the rightmost index (closest to cleavage site)
                if (llr > 0)
                {
                    totalHitCount++;

                    if (llr >= maxLLR)
                    {
                        maxLLR = llr;
                        polyALeftIndex = i;
                    }
                }
            }

            this.LLR = maxLLR;
            this.PolyALeftIndex = polyALeftIndex;
            this.TotalHitCount = totalHitCount;
            this.DistanceToCleavageSite = Read.CleavageSite - this.PolyALeftIndex - MotifScan.MotifLength;
            this.PolyASite = this.PolyALeftIndex >= 0 ? Read.Sequence.Substring(this.PolyALeftIndex, MotifScan.MotifLength) : String.Empty;
        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Label + ":");

            if (this.PolyALeftIndex >= 0)
            {
                sb.AppendLine("  LLR: " + this.LLR);
                sb.AppendLine("  Poly-A Site: " + this.PolyASite);
                sb.AppendLine("  Cleavage Site: " + (this.Read.CleavageSite + 1)); // 1-based
                sb.AppendLine("  Distance: " + this.DistanceToCleavageSite);
                sb.AppendLine("  Poly-A Left Index: " + (this.PolyALeftIndex + 1)); // 1-based
                sb.AppendLine("  Hit Count: " + this.TotalHitCount);

            }
            else
            {
                sb.AppendLine("  No Poly-A Site found");
            }

            return sb.ToString();
        }
    }
}
