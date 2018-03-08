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
        public double[,] ProbabilityMatrix { get; set; }
        public double[,] BackgroundMatrix { get; set; }
        public double[,] LLRMatrix { get; set; }
        public double RelativeEntropy { get; private set; }

        /// <summary>
        /// Map a nucleotide char (A,C,G,T) to its index in a WMM
        /// Matches Row order: 0=A, 1=C, 2=G, 3=T
        /// </summary>
        public Dictionary<char, int> NTMap;

        public WMM(double[,] probabilityMatrix)
        {
            this.ProbabilityMatrix = probabilityMatrix;
            this.BackgroundMatrix = CreateBackgroundMatrix();
            this.LLRMatrix = ConvertToLog(CreateRatioMatrix());
            this.CalculateRelativeEntropy();

            this.NTMap = new Dictionary<char, int>();
            NTMap.Add('A', 0);
            NTMap.Add('C', 1);
            NTMap.Add('G', 2);
            NTMap.Add('T', 3);
        }

        private double[,] CreateBackgroundMatrix()
        {
            double[,] background = new double[4, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    background[i, j] = 0.25;
                }
            }

            return background;
        }

        /// <summary>
        /// Creates a matrix with the foreground/background ratios
        /// </summary>
        private double[,] CreateRatioMatrix()
        {
            double[,] logProbability = new double[4, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    logProbability[i, j] = ProbabilityMatrix[i,j] / BackgroundMatrix[i,j];
                }
            }

            return logProbability;
        }

        /// <summary>
        /// Convert all values to Log base 2
        /// </summary>
        public static double[,] ConvertToLog(double[,] matrix)
        {
            double[,] newMatrix = new double[4, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    newMatrix[i, j] = Math.Log(matrix[i,j], 2);
                }
            }

            return newMatrix;
        }

        private void CalculateRelativeEntropy()
        {
            double sum = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    double Px = this.ProbabilityMatrix[i,j];
                    double LLR = this.LLRMatrix[i, j];
                    LLR = Double.IsNegativeInfinity(LLR) ? 0 : LLR;
                    sum += Px * LLR;
                }
            }

            this.RelativeEntropy = sum;
        }

        public static WMM CreateWMM0()
        {
            // 100% consensus

            double[,] foregroundMatrix = new double[4, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    foregroundMatrix[i, j] = 0;
                }
            }

            foregroundMatrix[0, 0] = 1;
            foregroundMatrix[0, 1] = 1;
            foregroundMatrix[3, 2] = 1;
            foregroundMatrix[0, 3] = 1;
            foregroundMatrix[0, 4] = 1;
            foregroundMatrix[0, 5] = 1;

            return new WMM(foregroundMatrix);
        }

        public static WMM CreateWMM1()
        {
            // 85% consensus

            double[,] foregroundMatrix = new double[4, 6];

            foregroundMatrix[0, 0] = 0.85;
            foregroundMatrix[1, 0] = 0.05;
            foregroundMatrix[2, 0] = 0.05;
            foregroundMatrix[3, 0] = 0.05;

            foregroundMatrix[0, 1] = 0.85;
            foregroundMatrix[1, 1] = 0.05;
            foregroundMatrix[2, 1] = 0.05;
            foregroundMatrix[3, 1] = 0.05;

            foregroundMatrix[0, 2] = 0.05;
            foregroundMatrix[1, 2] = 0.05;
            foregroundMatrix[2, 2] = 0.05;
            foregroundMatrix[3, 2] = 0.85;

            foregroundMatrix[0, 3] = 0.85;
            foregroundMatrix[1, 3] = 0.05;
            foregroundMatrix[2, 3] = 0.05;
            foregroundMatrix[3, 3] = 0.05;

            foregroundMatrix[0, 4] = 0.85;
            foregroundMatrix[1, 4] = 0.05;
            foregroundMatrix[2, 4] = 0.05;
            foregroundMatrix[3, 4] = 0.05;

            foregroundMatrix[0, 5] = 0.85;
            foregroundMatrix[1, 5] = 0.05;
            foregroundMatrix[2, 5] = 0.05;
            foregroundMatrix[3, 5] = 0.05;

            return new a5.WMM(foregroundMatrix);
        }

        public static string PrintMatrix(double[,] matrix)
        {
            StringBuilder sb = new StringBuilder();

            int padding = 4;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i,j].ToString().PadLeft(padding));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
