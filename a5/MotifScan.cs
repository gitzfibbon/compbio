using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    public partial class MotifScan
    {
        private const int MotifLength = 6;

        public MotifScan()
        {
            this.InitializeWMM();
        }

        /// <summary>
        /// Returns 0-based index of motif position closest to cleavage site
        /// Eg. CGAATAAACGCG.AAAAAA returns 7 which is the last A in AATAAA
        /// </summary>
        /// <returns></returns>
        public int Scan(Read read, double[,] wmm)
        {
            // Scan from left to right (cleavage site)
            for (int i = 0; i + MotifLength <= read.CleavageSite; i++)
            {
                string subSequence = read.Sequence.Substring(i, MotifLength);
            }

            return 0;
        }
    }
}
