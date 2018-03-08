using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    /// <summary>
    /// Methods to print results
    /// </summary>
    public partial class Viterbi
    {
        public string Print(int maxHits = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PrintEmissions());
            sb.AppendLine();
            sb.Append(PrintTransitions());
            sb.AppendLine();
            sb.Append(PrintLogProbability());
            sb.AppendLine();
            sb.Append(PrintHits(maxHits));
            sb.AppendLine();

            //File.WriteAllText("tracebackpath.txt", PrintTraceback());

            return sb.ToString();
        }

        public string PrintHits(int maxHits)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Hits");
            sb.AppendLine("Count: " + Hits.Count);
            int colPad = 10;
            sb.Append("Start".PadLeft(colPad));
            sb.Append("End".PadLeft(colPad));
            sb.Append("Length".PadLeft(colPad));
            sb.AppendLine();

            int hitCount = 0;
            foreach (Tuple<int, int, int> hit in Hits)
            {
                // Print using 1-based index
                sb.Append((hit.Item1 + 1).ToString().PadLeft(colPad));
                sb.Append((hit.Item2 + 1).ToString().PadLeft(colPad));

                sb.Append(hit.Item3.ToString().PadLeft(colPad));
                sb.AppendLine();

                hitCount++;
                if (maxHits > 0 && hitCount >= maxHits)
                {
                    break;
                }
            }

            return sb.ToString();
        }

        public string PrintTraceback()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Observations.Length; i++)
            {
                sb.Append(TracebackPath[i]);
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintLogProbability()
        {
            StringBuilder sb = new StringBuilder();
            int iteration = 1;
            sb.Append("Log Probability for iteration " + iteration.ToString().PadLeft(2) + ": ");
            sb.Append(LogProbability.ToString("F10"));
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintTransitions()
        {
            StringBuilder sb = new StringBuilder();
            int firstColPad = 11;
            int colPad = 14;
            sb.AppendLine("Transitions".PadRight(firstColPad) + "State1".PadLeft(colPad) + "State2".PadLeft(colPad));
            sb.Append("Begin".PadRight(firstColPad));
            sb.Append(BeginTransitions[State1].ToString("F8").PadLeft(colPad));
            sb.Append(BeginTransitions[State2].ToString("F8").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State1".PadRight(firstColPad));
            sb.Append(Transitions[State1, State1].ToString("F8").PadLeft(colPad));
            sb.Append(Transitions[State1, State2].ToString("F8").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State2".PadRight(firstColPad));
            sb.Append(Transitions[State2, State1].ToString("F8").PadLeft(colPad));
            sb.Append(Transitions[State2, State2].ToString("F8").PadLeft(colPad));
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintEmissions()
        {
            StringBuilder sb = new StringBuilder();
            int statePad = 10;
            int colPad = 12;
            sb.AppendLine("Emissions".PadRight(statePad) + "A".PadLeft(colPad) + "C".PadLeft(colPad) + "G".PadLeft(colPad) + "T".PadLeft(colPad));
            sb.Append("State1".PadRight(statePad));
            sb.Append(Emissions[State1, A].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State1, C].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State1, G].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State1, T].ToString("F8").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State2".PadRight(statePad));
            sb.Append(Emissions[State2, A].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State2, C].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State2, G].ToString("F8").PadLeft(colPad));
            sb.Append(Emissions[State2, T].ToString("F8").PadLeft(colPad));
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
