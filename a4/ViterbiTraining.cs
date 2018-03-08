using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    /// <summary>
    /// Runs through multiple iterations and updates the Emission and Transition probabilities
    /// on each iteration.
    /// </summary>
    class ViterbiTraining
    {
        /// <summary>
        /// Initial observations
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// History of every training iteration
        /// </summary>
        public List<Viterbi> Iterations { get; set; }

        public ViterbiTraining(string observations)
        {
            this.Observations = observations;
        }

        /// <summary>
        /// Run through all the iterations
        /// </summary>
        /// <param name="numIterations"></param>
        public void Run(int numIterations)
        {
            if (numIterations <= 0)
            {
                return;
            }

            // Do an initial run
            Viterbi viterbi = new Viterbi(this.Observations);
            viterbi.Run();

            this.Iterations = new List<Viterbi>();
            this.Iterations.Add(viterbi);

            for (int i = 1; i < numIterations; i++)
            {
                // Set up the new iteration
                viterbi = new Viterbi(this.Observations);

                // Overwrite the default emissions and transitions
                viterbi.Transitions = Iterations.Last().RetrainedTransitions;
                viterbi.Emissions = Iterations.Last().RetrainedEmissions;

                viterbi.Run();
                this.Iterations.Add(viterbi);
            }

        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Iterations.Count; i++)
            {
                sb.AppendLine();
                sb.AppendLine("----------------------------------------");
                sb.AppendLine("Iteration " + (i + 1).ToString());
                sb.AppendLine("----------------------------------------");
                sb.AppendLine();

                if (i < Iterations.Count - 1)
                {
                    sb.AppendLine(Iterations[i].Print(5));
                }
                else
                {
                    sb.AppendLine(Iterations[i].Print());
                }


            }


            return sb.ToString();
        }


    }
}
