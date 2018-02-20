using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    public enum HmmState
    {
        OneLowGC = 0,
        TwoHighGC,
        Begin
    }

    class Viterbi
    {
        // Constants for array indexing
        private const int State1 = 0;
        private const int State2 = 1;
        private const int A = 0;
        private const int C = 1;
        private const int G = 2;
        private const int T = 3;

        public string Observations { get; set; }

        public double[,] BeginTransitions { get; set; }

        public double[,] Transitions { get; set; }

        public double[,] Emissions { get; set; }

        public Viterbi(string observations)
        {
            this.Observations = observations;

            this.BeginTransitions = new double[2,1];
            this.BeginTransitions[State1, State1] = 0.9999;
            this.BeginTransitions[State1, State2] = 0.0001;

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
        }

        public void Train()
        {

        }

    }
}
