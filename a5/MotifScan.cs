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

        public WMM WMM { get; set; }

        public double LLR_WMM0 { get; set; }
        public int PolyALeftIndex_WMM0 { get; set; }
        public int HitCount_WMM0 { get; set; }

        public double LLR_WMM1 { get; set; }
        public int PolyALeftIndex_WMM1 { get; set; }
        public int HitCount_WMM1 { get; set; }

        /// <summary>
        /// Map a nucleotide char (A,C,G,T) to its index in a WMM
        /// </summary>
        private Dictionary<char, int> ntIndex;

        public MotifScan()
        {
            this.WMM = new WMM();

            this.ntIndex = new Dictionary<char, int>();
            ntIndex.Add('A', 0);
            ntIndex.Add('C', 1);
            ntIndex.Add('G', 2);
            ntIndex.Add('T', 3);
        }

        public void ScanAll(Read read)
        {
            Tuple<double, int, int> wmm0Result = Scan(read, WMM.WMM0);
            this.LLR_WMM0 = wmm0Result.Item1;
            this.PolyALeftIndex_WMM0 = wmm0Result.Item2;
            this.HitCount_WMM0 = wmm0Result.Item3;

            Tuple<double, int, int> wmm1Result = Scan(read, WMM.WMM1);
            this.LLR_WMM1 = wmm1Result.Item1;
            this.PolyALeftIndex_WMM1 = wmm1Result.Item2;
            this.HitCount_WMM1 = wmm1Result.Item3;
        }

        /// <summary>
        /// Returns LLR, 0-based index of motif position closest to cleavage site, hit count
        /// Eg. CGAATAAACGCG.AAAAAA returns 7 as index which is the last A in AATAAA
        /// </summary>
        /// <returns></returns>
        public Tuple<double, int, int> Scan(Read read, double[,] wmm)
        {
            double maxLLR = 0;
            int maxLLRIndex = -1; // From the left side of the 6 element sub sequence
            int hitCount = 0;

            // Scan from left to right (cleavage site)
            for (int i = 0; i + MotifLength <= read.CleavageSite; i++)
            {
                string subSequence = read.Sequence.Substring(i, MotifLength);

                // Calculate probability the subSequence is the poly-A signal
                double llr = 0;
                for (int j = 0; j < 6; j++)
                {
                    char nt = subSequence[j];
                    llr += Math.Log(wmm[ntIndex[nt],j] / BackgroundProbability);
                }

                // The equality in the >= is important so that we take the rightmost index (closest to cleavage site)
                if (llr > 0)
                {
                    hitCount++;

                    if (llr >= maxLLR)
                    {
                        maxLLR = llr;
                        maxLLRIndex = i;
                    }
                }
            }

            return new Tuple<double, int, int>(maxLLR, maxLLRIndex, hitCount);
        }
    }
}
