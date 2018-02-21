using System;
using System.Collections.Generic;
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
        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PrintEmissions());
            sb.AppendLine();
            sb.Append(PrintTransitions());
            sb.AppendLine();
            sb.Append(PrintLogProbability());
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintLogProbability()
        {
            StringBuilder sb = new StringBuilder();
            int iteration = 1;
            sb.Append("Log Probability for iteration " + iteration.ToString().PadLeft(2) + ": ");
            sb.Append(Math.Max(V[State1, Observations.Length - 1], V[State2, Observations.Length - 1]).ToString("F10"));
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintTransitions()
        {
            StringBuilder sb = new StringBuilder();
            int firstColPad = 11;
            int colPad = 10;
            sb.AppendLine("Transitions".PadRight(firstColPad) + "State1".PadLeft(colPad) + "State2".PadLeft(colPad));
            sb.Append("Begin".PadRight(firstColPad));
            sb.Append(BeginTransitions[State1].ToString("F4").PadLeft(colPad));
            sb.Append(BeginTransitions[State2].ToString("F4").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State1".PadRight(firstColPad));
            sb.Append(Transitions[State1, State1].ToString("F4").PadLeft(colPad));
            sb.Append(Transitions[State1, State2].ToString("F4").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State2".PadRight(firstColPad));
            sb.Append(Transitions[State2, State1].ToString("F4").PadLeft(colPad));
            sb.Append(Transitions[State2, State2].ToString("F4").PadLeft(colPad));
            sb.AppendLine();

            return sb.ToString();
        }

        public string PrintEmissions()
        {
            StringBuilder sb = new StringBuilder();
            int statePad = 10;
            int colPad = 6;
            sb.AppendLine("Emissions".PadRight(statePad) + "A".PadLeft(colPad) + "C".PadLeft(colPad) + "G".PadLeft(colPad) + "T".PadLeft(colPad));
            sb.Append("State1".PadRight(statePad));
            sb.Append(Emissions[State1, A].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State1, C].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State1, G].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State1, T].ToString("F2").PadLeft(colPad));
            sb.AppendLine();
            sb.Append("State2".PadRight(statePad));
            sb.Append(Emissions[State2, A].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State2, C].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State2, G].ToString("F2").PadLeft(colPad));
            sb.Append(Emissions[State2, T].ToString("F2").PadLeft(colPad));
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
