using System;
using System.Collections.Generic;
using System.IO;
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
        public const int GapPenalty = -4;

        public Protein Protein1 { get; private set; }
        public Protein Protein2 { get; private set; }

        private int[,] scoreMatrix { get; set; }

        public int Score { get; private set; }

        private List<int> P1Trace { get; set; }
        private List<int> P2Trace { get; set; }

        /// <summary>
        /// The [i,j] location of the Score so we can start traceback from there
        /// </summary>
        private Tuple<int, int> scorePosition { get; set; }

        public SW(Protein protein1, Protein protein2)
        {
            this.Protein1 = protein1;
            this.Protein2 = protein2;
        }

        public int ComputeScore()
        {
            this.ComputeScoreMatrix();
            this.ComputeTraceback();

            return this.Score;
        }

        private void ComputeTraceback()
        {
            this.P1Trace = new List<int>();
            this.P2Trace = new List<int>();

            if (this.scoreMatrix == null) { return; }

            // Start at the location of the max score and work backwards
            int i = this.scorePosition.Item1;
            int j = this.scorePosition.Item2;

            P1Trace.Add(i);
            P2Trace.Add(j);

            while (true)
            {
                int current = this.scoreMatrix[i, j];

                // Check which adjacent location caused us to get here


                // Check diagonal
                if (this.scoreMatrix[i - 1, j - 1] > 0 && this.scoreMatrix[i - 1, j - 1] + Blosum62.Sigma(Protein1.Encoding[i].ToString(), Protein2.Encoding[j].ToString()) == current)
                {
                    // We could have landed here from the diagonal adjacent element
                    i = i - 1;
                    j = j - 1;

                    P1Trace.Add(i);
                    P2Trace.Add(j);

                    continue;
                }

                // Check above
                if (this.scoreMatrix[i - 1, j] > 0 && this.scoreMatrix[i - 1, j] + SW.GapPenalty == current)
                {
                    // We could have landed here from the element to the above
                    i = i - 1;

                    P1Trace.Add(i);
                    P2Trace.Add(-1); // gap

                    continue;
                }

                // Check left
                if (this.scoreMatrix[i, j - 1] > 0 && this.scoreMatrix[i, j - 1] + SW.GapPenalty == current)
                {
                    // We could have landed here from the element to the left
                    j = j - 1;

                    P1Trace.Add(-1); // gap
                    P2Trace.Add(j);

                    continue;
                }

                break;
            }

            this.P1Trace.Reverse();
            this.P2Trace.Reverse();

        }

        private void ComputeScoreMatrix()
        {
            // Will be initalized to all zeros since that is the default int value in c#
            this.scoreMatrix = new int[Protein1.Encoding.Length, Protein2.Encoding.Length];

            for (int i = 1; i < Protein1.Encoding.Length; i++)
            {
                for (int j = 1; j < Protein2.Encoding.Length; j++)
                {
                    int v1 = scoreMatrix[i - 1, j - 1] + Blosum62.Sigma(Protein1.Encoding[i].ToString(), Protein2.Encoding[j].ToString());
                    int v2 = scoreMatrix[i - 1, j] + SW.GapPenalty;
                    int v3 = scoreMatrix[i, j - 1] + SW.GapPenalty;
                    int v4 = 0;

                    int max = Math.Max(Math.Max(v1, v2), Math.Max(v3, v4));
                    this.scoreMatrix[i, j] = max;

                    // Update with the new score
                    if (max > this.Score)
                    {
                        this.Score = max;
                        this.scorePosition = new Tuple<int, int>(i, j);
                    }
                }
            }
        }

        #region Printing Methods


        public void PrintResult(bool includeScoringMatrix = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Score: " + this.Score);
            sb.AppendLine();

            sb.AppendLine("Alignment:");
            sb.AppendLine(this.GetP1Trace());
            sb.AppendLine(this.GetP2Trace());
            sb.AppendLine();


            if (includeScoringMatrix)
            {
                sb.AppendLine("Scoring Matrix:");
                sb.AppendLine(this.GetScoreMatrix());
            }

            Console.WriteLine(sb.ToString());

            File.WriteAllText("result_" + this.Protein1.Name + "--" + this.Protein2.Name + ".txt", sb.ToString());
        }

        private string GetP1Trace()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int x in this.P1Trace)
            {
                if (x == -1)
                {
                    // This is a gap
                    sb.Append("-");
                }
                else
                {
                    sb.Append(this.Protein1.Encoding[x]);
                }
            }

            return sb.ToString();
        }

        private string GetP2Trace()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int x in this.P2Trace)
            {
                if (x == -1)
                {
                    // This is a gap
                    sb.Append("-");
                }
                else
                {
                    sb.Append(this.Protein2.Encoding[x]);
                }
            }

            return sb.ToString();
        }

        private string GetScoreMatrix()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("  ");
            for (int j = 0; j < Protein2.Encoding.Length; j++)
            {
                sb.Append("   " + this.Protein2.Encoding[j] + " ");
            }
            sb.AppendLine();


            for (int i = 0; i < Protein1.Encoding.Length; i++)
            {
                sb.Append(this.Protein1.Encoding[i] + " ");

                for (int j = 0; j < Protein2.Encoding.Length; j++)
                {
                    sb.Append(this.SpacedInt(this.scoreMatrix[i, j]) + " ");
                }

                sb.AppendLine();
            }

            return sb.ToString();

        }

        /// <summary>
        /// Return a string with spacing so that the scoring matrix is nicely aligned when printing
        /// </summary>
        private string SpacedInt(int i)
        {
            // Assume i is non-negative
            // Max i value is 9999

            if (i < 10) { return "   " + i; }
            if (i < 100) { return "  " + i; }
            if (i < 1000) { return " " + i; }
            if (i < 10000) { return "" + i; }

            return i.ToString();
        }

        #endregion
    }
}
