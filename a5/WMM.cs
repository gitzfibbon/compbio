using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    public class WMM
    {
        // Row order: 0=A, 1=C, 2=G, 3=T
        public double[,] Matrix { get; set; }

        /// <summary>
        /// Map a nucleotide char (A,C,G,T) to its index in a WMM
        /// Matches Row order: 0=A, 1=C, 2=G, 3=T
        /// </summary>
        public Dictionary<char, int> NTMap;

        public WMM()
        {
            this.Matrix = new double[4, 6];

            this.NTMap = new Dictionary<char, int>();
            NTMap.Add('A', 0);
            NTMap.Add('C', 1);
            NTMap.Add('G', 2);
            NTMap.Add('T', 3);
        }

        public static WMM CreateWMM0()
        {
            WMM wmm = new a5.WMM();

            // 100% consensus

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    wmm.Matrix[i, j] = 0;
                }
            }

            wmm.Matrix[0, 0] = 1;
            wmm.Matrix[0, 1] = 1;
            wmm.Matrix[3, 2] = 1;
            wmm.Matrix[0, 3] = 1;
            wmm.Matrix[0, 4] = 1;
            wmm.Matrix[0, 5] = 1;

            return wmm;
        }

        public static WMM CreateWMM1()
        {
            // 85% consensus

            WMM wmm = new WMM();

            wmm.Matrix[0, 0] = 0.85;
            wmm.Matrix[1, 0] = 0.05;
            wmm.Matrix[2, 0] = 0.05;
            wmm.Matrix[3, 0] = 0.05;

            wmm.Matrix[0, 1] = 0.85;
            wmm.Matrix[1, 1] = 0.05;
            wmm.Matrix[2, 1] = 0.05;
            wmm.Matrix[3, 1] = 0.05;

            wmm.Matrix[0, 2] = 0.05;
            wmm.Matrix[1, 2] = 0.05;
            wmm.Matrix[2, 2] = 0.05;
            wmm.Matrix[3, 2] = 0.85;

            wmm.Matrix[0, 3] = 0.85;
            wmm.Matrix[1, 3] = 0.05;
            wmm.Matrix[2, 3] = 0.05;
            wmm.Matrix[3, 3] = 0.05;

            wmm.Matrix[0, 4] = 0.85;
            wmm.Matrix[1, 4] = 0.05;
            wmm.Matrix[2, 4] = 0.05;
            wmm.Matrix[3, 4] = 0.05;

            wmm.Matrix[0, 5] = 0.85;
            wmm.Matrix[1, 5] = 0.05;
            wmm.Matrix[2, 5] = 0.05;
            wmm.Matrix[3, 5] = 0.05;

            return wmm;
        }



    }
}
