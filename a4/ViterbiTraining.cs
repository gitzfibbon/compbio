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

            this.Iterations = new List<Viterbi>();

            Viterbi viterbi = new Viterbi(this.Observations);
            viterbi.Train();
            viterbi.Traceback();

        }

        /// <summary>
        /// Updates emission and transition probabilities based on previous iteration
        /// then runs a new iteration
        /// </summary>
        /// <param name="previousViterbi"></param>
        /// <returns></returns>
        private Viterbi RunOnce(Viterbi previousViterbi)
        {
            // Use previous viterbi results to update new emission/transition probs
            if (previousViterbi != null)
            {

            }



            return null;
        }

    }
}
