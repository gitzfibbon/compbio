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
        public enum Direction
        {
            Reset = 0,
            FromLeft,
            FromAbove,
            FromDiagonal,
            
        }

        public const int GapPenalty = -1;

        public Protein Protein1 { get; private set; }
        public Protein Protein2 { get; private set; }

        private int[,] scoreMatrix { get; set; }
        private Direction[,] traceMatrix { get; set; }

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

        public int ComputeScore(bool computeTraceback = true)
        {
            this.ComputeScoreMatrix();
            if (computeTraceback)
            {
                this.ComputeTraceback();
            }

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

            while (true)
            {
                if (this.scoreMatrix[i, j] == 0)
                {
                    break;
                }

                int current = this.scoreMatrix[i, j];

                // Check which adjacent location caused us to get here

                // Check above
                int x = this.scoreMatrix[i - 1, j];
                if (this.scoreMatrix[i - 1, j] + SW.GapPenalty == current)
                {
                    P1Trace.Add(i);
                    P2Trace.Add(-1); // gap
                    
                    // We could have landed here from the element to the above
                    i = i - 1;

                    continue;
                }

                // Check left
                x = this.scoreMatrix[i, j - 1];
                if (this.scoreMatrix[i, j - 1] + SW.GapPenalty == current)
                {
                    P1Trace.Add(-1); // gap
                    P2Trace.Add(j);

                    // We could have landed here from the element to the left
                    j = j - 1;

                    continue;
                }

                // Check diagonal
                int a = this.scoreMatrix[i - 1, j - 1];
                int b = Blosum62.Sigma(Protein1.Encoding[i - 1].ToString(), Protein2.Encoding[j - 1].ToString());
                x = a + b;
                if (this.scoreMatrix[i - 1, j - 1] + Blosum62.Sigma(Protein1.Encoding[i - 1].ToString(), Protein2.Encoding[j - 1].ToString()) == current)
                {
                    P1Trace.Add(i);
                    P2Trace.Add(j);

                    // We could have landed here from the diagonal adjacent element
                    i = i - 1;
                    j = j - 1;

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
            this.scoreMatrix = new int[Protein1.Encoding.Length + 1, Protein2.Encoding.Length + 1];
            this.traceMatrix = new Direction[Protein1.Encoding.Length + 1, Protein2.Encoding.Length + 1];

            for (int i = 0; i < Protein1.Encoding.Length; i++)
            {
                for (int j = 0; j < Protein2.Encoding.Length; j++)
                {
                    int v1 = scoreMatrix[i, j] + Blosum62.Sigma(Protein1.Encoding[i].ToString(), Protein2.Encoding[j].ToString());
                    int v2 = scoreMatrix[i, j + 1] + SW.GapPenalty;
                    int v3 = scoreMatrix[i + 1, j] + SW.GapPenalty;
                    int v4 = 0;

                    int max = Math.Max(Math.Max(v1, v2), Math.Max(v3, v4));
                    this.scoreMatrix[i + 1, j + 1] = max;

                    // Update with the new score
                    if (max > this.Score)
                    {
                        this.Score = max;
                        this.scorePosition = new Tuple<int, int>(i + 1, j + 1);
                    }

                    // Update the traceback matrix
                    if (v1 == max)
                    {
                        this.traceMatrix[i + 1, j + 1] = Direction.FromDiagonal;
                    }
                    else if (v2 == max)
                    {
                        this.traceMatrix[i + 1, j + 1] = Direction.FromAbove;
                    }
                    else if (v3 == max)
                    {
                        this.traceMatrix[i + 1, j + 1] = Direction.FromLeft;
                    }
                    else
                    {
                        this.traceMatrix[i + 1, j + 1] = Direction.Reset;
                    }
                }
            }
        }

        #region Printing Methods


        public void PrintResult(bool includeScoringMatrix = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Protein1.Name + " " + this.Protein1.Accession);
            sb.AppendLine(this.Protein2.Name + " " + this.Protein2.Accession);
            sb.AppendLine();

            sb.AppendLine("Score: " + this.Score);
            sb.AppendLine();

            sb.AppendLine("Alignment:");
            sb.AppendLine();
            sb.AppendLine(this.GetCombinedTraces());
            sb.AppendLine();

            sb.AppendLine("Traceback:");
            sb.AppendLine();
            sb.AppendLine(this.GetTraceMatrix());
            sb.AppendLine();

            if (includeScoringMatrix)
            {
                sb.AppendLine("Scoring Matrix:");
                sb.AppendLine(this.GetScoreMatrix());
            }

            Console.WriteLine(sb.ToString());

            File.WriteAllText("result_" + this.Protein1.Name + "--" + this.Protein2.Name + ".txt", sb.ToString());
        }

        private string GetCombinedTraces()
        {
            StringBuilder sb = new StringBuilder();

            string p1TraceString = this.GetP1Trace();
            string p2TraceString = this.GetP2Trace();

            int p1StartIndex = this.P1Trace[0];
            int p2StartIndex = this.P2Trace[0];

            int idLength = Math.Max(this.Protein1.Accession.Length, this.Protein2.Accession.Length);
            string paddedProtein1 = this.Protein1.Accession.PadLeft(idLength);
            string paddedProtein2 = this.Protein2.Accession.PadLeft(idLength);

            int i = 0;
            while (true)
            {
                sb.Append(paddedProtein1 + ": " + (p1StartIndex + (i * 60)).ToString().PadLeft(4) + " ");
                sb.AppendLine(p1TraceString.Substring(i * 60, Math.Min(60, p1TraceString.Length - (i * 60))));

                sb.Append(paddedProtein2 + ": " + (p2StartIndex + (i * 60)).ToString().PadLeft(4) + " ");
                sb.AppendLine(p2TraceString.Substring(i * 60, Math.Min(60, p2TraceString.Length - (i * 60))));

                sb.AppendLine();

                if ((i + 1) * 60 >= this.P1Trace.Count)
                {
                    break;
                }

                i++;
            }

            return sb.ToString();

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
                    sb.Append(this.Protein1.Encoding[x - 1]);
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
                    sb.Append(this.Protein2.Encoding[x - 1]);
                }
            }

            return sb.ToString();
        }

        private string GetScoreMatrix()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("       ");
            for (int j = 0; j < Protein2.Encoding.Length; j++)
            {
                sb.Append("   " + this.Protein2.Encoding[j] + " ");
            }
            sb.AppendLine();

            for (int i = 0; i < Protein1.Encoding.Length + 1; i++)
            {
                if (i > 0)
                {
                    sb.Append(this.Protein1.Encoding[i - 1] + " ");
                }
                else
                {
                    sb.Append("  ");
                }

                for (int j = 0; j < Protein2.Encoding.Length + 1; j++)
                {
                    sb.Append((this.scoreMatrix[i, j]).ToString().PadLeft(4) + " ");
                }

                sb.AppendLine();
            }

            return sb.ToString();

        }


        private string GetTraceMatrix()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("       ");
            for (int j = 0; j < Protein2.Encoding.Length; j++)
            {
                sb.Append("   " + this.Protein2.Encoding[j] + " ");
            }
            sb.AppendLine();

            for (int i = 0; i < Protein1.Encoding.Length + 1; i++)
            {
                if (i > 0)
                {
                    sb.Append(this.Protein1.Encoding[i - 1] + " ");
                }
                else
                {
                    sb.Append("  ");
                }

                for (int j = 0; j < Protein2.Encoding.Length + 1; j++)
                {
                    sb.Append((this.GetTraceSymbol(this.traceMatrix[i, j])).PadLeft(4) + " ");
                }

                sb.AppendLine();
            }

            return sb.ToString();

        }

        private string GetTraceSymbol(Direction direction)
        {
            switch (direction)
            {
                case Direction.FromLeft:
                    return "-";
                case Direction.FromAbove:
                    return "|";
                case Direction.FromDiagonal:
                    return @"\";
                default:
                    return "0";
            }
        }

        #endregion
    }
}
