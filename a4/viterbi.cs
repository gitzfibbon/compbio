﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    public partial class Viterbi
    {
        // Constants for array indexing
        public const int State1 = 0;
        public const int State2 = 1;
        public const int A = 0;
        public const int C = 1;
        public const int G = 2;
        public const int T = 3;

        // The sequence of A,C,G,T
        public string Observations { get; set; }

        // Initial state transition probabilities
        public double[] BeginTransitions { get; set; }

        public double[,] Transitions { get; set; }

        /// <summary>
        /// Adjusted after training
        /// </summary>
        public double[,] RetrainedTransitions { get; set; }

        public double[,] Emissions { get; set; }

        /// <summary>
        /// Adjusted after training
        /// </summary>
        public double[,] RetrainedEmissions { get; set; }

        /// <summary>
        /// From slide 29, probability of the most probable path at each observation
        /// </summary>
        public double[,] V { get; set; }

        /// <summary>
        /// Aligned with V but keeps track of backtrace pointers
        /// </summary>
        public int[,] Traces { get; set; }

        /// <summary>
        /// The traceback path / most probable path
        /// </summary>
        public int[] TracebackPath { get; set; }

        /// <summary>
        /// Highest Log Probability
        /// </summary>
        public double LogProbability { get; set; }

        /// <summary>
        /// Maps a nucleotide (A,C,G,T) to its index value
        /// </summary>
        private Dictionary<char, int> NucMap;

        /// <summary>
        /// Maps an int to a state
        /// </summary>
        private Dictionary<int, int> StateMap;

        /// <summary>
        /// Stores all of the hits
        /// </summary>
        List<Tuple<int, int, int>> Hits;

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

            // Keep this in sync with the constants A,C,G,T
            this.NucMap = new Dictionary<char, int>();
            this.NucMap.Add('A', A);
            this.NucMap.Add('C', C);
            this.NucMap.Add('G', G);
            this.NucMap.Add('T', T);

            // Keep this in sync with the constants State1 and State2
            this.StateMap = new Dictionary<int, int>();
            this.StateMap.Add(0, State1);
            this.StateMap.Add(1, State2);
        }

        /// <summary>
        /// Combine the Train and Traceback into a full "Run"
        /// </summary>
        public void Run()
        {
            this.Train();
            this.Traceback();
            this.Retrain();
        }

        /// <summary>
        /// Retrain the initial transition and emission probabilities
        /// </summary>
        public void Retrain()
        {

            // Update the transitions

            double state1Count = 0;
            double state2Count = 0;
            double state1To2 = 0;
            double state2To1 = 0;
            double[,] obsCount = new double[2, 4]; // How many times ACGT were occurred in each state
            for (int i=0; i<this.TracebackPath.Length; i++)
            {
                if (TracebackPath[i] == State1)
                {
                    // Increment counter for this observation in State1
                    obsCount[State1, NucMap[Observations[i]]]++;

                    state1Count++;
                    if (i + 1 < TracebackPath.Length && TracebackPath[i+1] == State2)
                    {
                        state1To2++;
                    }
                }
                else
                {
                    // Increment counter for this observation in State1
                    obsCount[State2, NucMap[Observations[i]]]++;

                    state2Count++;
                    if (i + 1 < TracebackPath.Length && TracebackPath[i + 1] == State1)
                    {
                        state2To1++;
                    }
                }
            }

            this.RetrainedTransitions = new double[2, 2];
            this.RetrainedTransitions[State1, State2] = state1To2 / state1Count;
            this.RetrainedTransitions[State1, State1] = 1 - this.RetrainedTransitions[State1, State2];
            this.RetrainedTransitions[State2, State1] = state2To1 / state2Count;
            this.RetrainedTransitions[State2, State2] = 1 - this.RetrainedTransitions[State2, State1];

            this.RetrainedEmissions = new double[2, 4];
            for (int i=0; i<4; i++)
            {
                this.RetrainedEmissions[State1, i] = obsCount[State1, i] / state1Count;
                this.RetrainedEmissions[State2, i] = obsCount[State2, i] / state2Count;
            }
        }

        /// <summary>
        /// Calculate the log probabilities for each state at each observation
        /// Construct the traceback data structure that will be used for traceback
        /// </summary>
        public void Train()
        {
            // Set up the first state
            this.V = new double[2, this.Observations.Length];
            V[State1, 0] = Math.Log(BeginTransitions[State1]) + Math.Log(Emissions[State1, NucMap[Observations[0]]]);
            V[State2, 0] = Math.Log(BeginTransitions[State2]) + Math.Log(Emissions[State2, NucMap[Observations[0]]]);

            this.Traces = new int[2, this.Observations.Length];
            this.Traces[State1, 0] = State1;
            this.Traces[State2, 0] = State2;

            // i will iterate through each observation
            for (int i = 1; i < this.Observations.Length; i++)
            {
                for (int j = 0; j < this.StateMap.Count; j++)
                {
                    int currentState = this.StateMap[j];
                    double prevState1 = V[State1, i - 1];
                    double prevState2 = V[State2, i - 1];
                    double transitionFromState1 = Transitions[currentState, State1];
                    double transitionFromState2 = Transitions[currentState, State2];
                    int observation = NucMap[Observations[i]];
                    double emissionFromState1 = Emissions[State1, observation];
                    double emissionFromState2 = Emissions[State2, observation];

                    double p1 = (prevState1) + Math.Log(transitionFromState1) + Math.Log(emissionFromState1);
                    double p2 = (prevState2) + Math.Log(transitionFromState2) + Math.Log(emissionFromState2);

                    if (p1 > p2)
                    {
                        this.V[currentState, i] = p1;
                        this.Traces[currentState, i] = State1;
                    }
                    else
                    {
                        this.V[currentState, i] = p2;
                        this.Traces[currentState, i] = State2;
                    }

                    //this.V[currentState, i] = Math.Max(p1, p2);

                }
            }
        }

        /// <summary>
        /// Use the traceback data structure to get the most probable path
        /// </summary>
        public void Traceback()
        {
            this.TracebackPath = new int[Observations.Length];

            // Determine where to start the traceback from
            if (V[State1, Observations.Length - 1] > V[State2, Observations.Length - 1])
            {
                this.TracebackPath[Observations.Length - 1] = State1;
                this.LogProbability = V[State1, Observations.Length - 1];
            }
            else
            {
                this.TracebackPath[Observations.Length - 1] = State2;
                this.LogProbability = V[State2, Observations.Length - 1];
            }

            // traceback
            for (int i = Observations.Length - 2; i >= 0; i--)
            {
                TracebackPath[i] = Traces[TracebackPath[i + 1], i];
            }

            this.Hits = CalculateHits(this.TracebackPath);
        }

        /// <summary>
        /// Find the hits in the most probable path
        /// </summary>
        public static List<Tuple<int, int, int>> CalculateHits(int[] tracebackPath)
        {
            List<Tuple<int, int, int>> hits = new List<Tuple<int, int, int>>();

            // calculate hits
            int minLength = 50;
            int currentState;
            for (int i = 0; i < tracebackPath.Length; i++)
            {
                currentState = tracebackPath[i];

                int hitStart = i;
                while (currentState == State2 && i < tracebackPath.Length)
                {
                    i++;
                    if (i >= tracebackPath.Length)
                    {
                        break;
                    }

                    currentState = tracebackPath[i];
                }
                int hitEnd = i - 1;
                int hitLength = i - hitStart;

                if (hitLength >= minLength)
                {
                    hits.Add(new Tuple<int, int, int>(hitStart, hitEnd, hitLength));
                }
            }

            return hits;
        }

    }
}
