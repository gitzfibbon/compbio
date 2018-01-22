using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    /// <summary>
    /// Smith-Waterman local alignment implementation for proteins
    /// </summary>
    class SW
    {
        public Protein Protein1 { get; private set; }
        public Protein Protein2 { get; private set; }

        private int[,] scoreMatrix { get; set; }

        public SW(Protein protein1, Protein protein2)
        {
            this.Protein1 = protein1;
            this.Protein2 = protein2;
        }

        public void ComputeScore()
        {
            this.ComputeScoreMatrix();

        }

        private void ComputeScoreMatrix()
        {
            int gapCost = -4;

            // Will be initalized to all zeros since that is the default int value in c#
            this.scoreMatrix = new int[Protein1.Encoding.Length, Protein2.Encoding.Length];

            for (int i = 1; i < Protein1.Encoding.Length; i++)
            {
                for (int j = 1; j < Protein2.Encoding.Length; j++)
                {
                    int v1 = scoreMatrix[i - 1, j - 1] + Blosum62.Sigma(Protein1.Encoding[i].ToString(), Protein2.Encoding[j].ToString());
                    int v2 = scoreMatrix[i - 1, j] + gapCost;
                    int v3 = scoreMatrix[i, j - 1] + gapCost;
                    int v4 = 0;

                    int max = Math.Max(Math.Max(v1, v2), Math.Max(v3, v4));
                    this.scoreMatrix[i, j] = max;
                }
            }
        }
    }
}
