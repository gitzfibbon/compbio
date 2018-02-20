using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    class Viterbi
    {
        // Constants for array indexing
        private const int State1 = 0;
        private const int State2 = 1;
        private const int A = 0;
        private const int C = 1;
        private const int G = 2;
        private const int T = 3;
        //private const int State1_A = 0;
        //private const int State1_C = 1;
        //private const int State1_G = 2;
        //private const int State1_T = 3;
        //private const int State2_A = 4;
        //private const int State2_C = 5;
        //private const int State2_G = 6;
        //private const int State2_T = 7;

        public string Observations { get; set; }

        public double[] BeginTransitions { get; set; }

        public double[,] Transitions { get; set; }

        public double[,] Emissions { get; set; }

        /// <summary>
        /// From slide 29, probability of the most probable path at each observation
        /// </summary>
        public double[,] V { get; set; }

        /// <summary>
        /// Maps a nucleotide (A,C,G,T) to its index value
        /// </summary>
        private Dictionary<char, int> NucMap;

        public Viterbi(string observations)
        {
            this.Observations = observations;

            this.BeginTransitions = new double[2];
            this.BeginTransitions[State1] = 0.9999;
            this.BeginTransitions[State2] = 0.0001;

            this.Transitions = new double[2, 2];
            this.Transitions[State1, State1] = 0.9999;
            this.Transitions[State1, State2] = 0.0001;
            this.Transitions[State2, State1] = 0.01;
            this.Transitions[State2, State2] = 0.99;

            this.Emissions = new double[2, 4];
            this.Emissions[State1, A] = 0.25;
            this.Emissions[State1, C] = 0.25;
            this.Emissions[State1, G] = 0.25;
            this.Emissions[State1, T] = 0.25;
            this.Emissions[State2, A] = 0.2;
            this.Emissions[State2, C] = 0.3;
            this.Emissions[State2, G] = 0.3;
            this.Emissions[State2, T] = 0.2;

            this.NucMap = new Dictionary<char, int>();
            this.NucMap.Add('A', A);
            this.NucMap.Add('C', C);
            this.NucMap.Add('G', G);
            this.NucMap.Add('T', T);
        }

        public void Train()
        {
            this.V = new double[2, this.Observations.Length];
            this.V[State1, 0] = this.BeginTransitions[State1] * this.Emissions[State1, NucMap[Observations[0]]];
            this.V[State2, 0] = this.BeginTransitions[State2] * this.Emissions[State2, NucMap[Observations[0]]];

            //this.V[State1_A, 0] = this.BeginTransitions[State1] * this.
            //this.V[State1_C, 0] = 0;
            //this.V[State1_G, 0] = 0;
            //this.V[State1_G, 0] = 0;
            //this.V[State2_A, 0] = 0;
            //this.V[State2_C, 0] = 0;
            //this.V[State2_G, 0] = 0;
            //this.V[State2_G, 0] = 0;



        }

    }
}
