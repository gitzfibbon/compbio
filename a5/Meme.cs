using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    class Meme
    {
        public Sam Sam { get; private set; }

        public WMM InitialWMM { get; private set; }
        public WMM NewWMM { get; private set; }

        public Meme(Sam sam, WMM initialWMM)
        {
            this.Sam = sam;
            this.InitialWMM = initialWMM;
        }

        public WMM Run()
        {
            // Go through each candidate
            foreach (Read candidate in Sam.Candidates)
            {
                ProcessCandidate(candidate);
            }



            return this.NewWMM;
        }

        private void ProcessCandidate(Read candidate)
        {
            double[,] probabilityAccumulator = new double[4, 6];
            double[,] logAccumulator = new double[4, 6];

            // Scan from left to right (cleavage site)
            for (int i = 0; i + MotifScan.MotifLength <= candidate.CleavageSite; i++)
            {
                string subSequence = candidate.Sequence.Substring(i, MotifScan.MotifLength);

                // Calculate probability the subSequence is the poly-A signal
                double llr = 0;
                double probability = 1;
                for (int j = 0; j < MotifScan.MotifLength; j++)
                {
                    char nt = subSequence[j]; // nucleotide
                    int ntIndex = WMM.NTMap[nt];
                    llr += this.InitialWMM.LLRMatrix[ntIndex, j];
                    probability *= this.InitialWMM.ProbabilityMatrix[ntIndex, j];
                }

                // Store the probability in each column of the accumulator
                for (int j = 0; j < MotifScan.MotifLength; j++)
                {
                    char nt = subSequence[j]; // nucleotide
                    int ntIndex = WMM.NTMap[nt];
                    probabilityAccumulator[ntIndex, j] += probability;
                    logAccumulator[ntIndex, j] += llr;
                }
            }

            
            Normalize(probabilityAccumulator);
            Normalize(logAccumulator);
        }

        /// <summary>
        /// Normalizes each column by the sum of the column
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private double[,] Normalize(double[,] matrix)
        {
            double[,] normalized = new double[4, 6];

            double columnSum = 0;
            bool isCalculated = false;

            for (int j=0; j<6;j++)
            {

                // Each column sums to the same so calculate only for first column
                if (!isCalculated)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        columnSum += matrix[i, j];
                    }

                    isCalculated = true;
                }

                // Divide each element by the columnSum
                for (int i = 0; i < 4; i++)
                {
                    normalized[i, j] = matrix[i,j] / columnSum;
                }

                //// Each column should sum to 1 on the first iteration
                //double testSum = 0;
                //for (int i = 0; i < 4; i++)
                //{
                //    testSum += normalized[i, j];
                //}

            }


            return normalized;
        }


    }
}
