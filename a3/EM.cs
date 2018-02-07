using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
    public class EM
    {
        public double[] X { get; private set; }

        public int K { get; private set; }

        public List<double[]> Means { get; private set; }

        public double tau { get; private set; }

        public double sigma { get; private set; }

        public List<double[,]> e_steps { get; private set; }

        private int iterations;

        public EM(double[] data, int numClusters)
        {
            this.X = data;
            this.K = numClusters;
            this.Means = new List<double[]>();
            this.tau = 1d / this.K; // fixed
            this.sigma = 1; // fixed
            this.e_steps = new List<double[,]>();
            this.iterations = 0;

            this.Initialize();
        }

        public void Run()
        {
            while (Terminate() == false)
            {
                this.E();
                this.M();
                this.BIC();
                iterations++;
            }
        }

        #region E Step

        /// <summary>
        /// E step: calculate the expected zij for every i and j
        /// </summary>
        public void E()
        {
            double[,] e_step = new double[this.X.Length, this.K];
            for (int i = 0; i < this.X.Length; i++)
            {
                for (int j = 0; j < this.K; j++)
                {
                    e_step[i, j] = Expected_zij(this.X[i], j);
                }
            }

            this.e_steps.Add(e_step);
        }

        /// <summary>
        /// Helper for the E step. Calculates the Expected zij for a particular xi and j.
        /// </summary>
        public double Expected_zij(double xi, int j)
        {
            double numerator = Likelihood(xi, this.Means.Last()[j], this.sigma) * this.tau;

            List<double> denominators = new List<double>(); // for debugging
            double denominator = 0;
            for (int k = 0; k < this.K; k++)
            {
                double likelihood = Likelihood(xi, this.Means.Last()[k], this.sigma);
                denominators.Add(likelihood * this.tau);
                denominator += likelihood * this.tau;
            }

            return numerator / denominator;
        }

        /// <summary>
        /// Likelihood that a number came from a normal distribution
        /// </summary>
        public double Likelihood(double xi, double mu, double sigma)
        {
            double sigma_squared = Math.Pow(sigma, 2);
            double e_power = -1 * Math.Pow(xi - mu, 2) / (2 * sigma_squared);
            double likelihood = Math.Exp(e_power) / Math.Sqrt(2 * Math.PI * sigma_squared);

            return likelihood;
        }

        #endregion

        #region M Step

        /// <summary>
        /// M step: update theta (just mu in this case)
        /// </summary>
        public void M()
        {
            double[] updatedMeans = new double[this.K];

            for (int j = 0; j < this.K; j++)
            {
                updatedMeans[j] = Mu_j(j);
            }

            this.Means.Add(updatedMeans);
        }

        /// <summary>
        /// Helper for the M step. Updates mu (theta) at j.
        /// </summary>
        public double Mu_j(int j)
        {
            double numerator = 0;
            double denominator = 0;
            for (int i = 0; i < this.X.Length; i++)
            {
                numerator += this.e_steps.Last()[i, j] * this.X[i];
                denominator += this.e_steps.Last()[i, j];

            }

            return numerator / denominator;
        }

        #endregion

        public double BIC()
        {
            // BIC = 2 * ln L(x | θ-hat) - r ln n

            double L = OverallLikelihood();
            double bic = 2 * Math.Log(L) - this.K * Math.Log(this.X.Length);
            return bic;
        }

        /// <summary>
        /// The overall likelihood is the product of all the P(D) terms
        /// </summary>
        /// <returns></returns>
        public double OverallLikelihood()
        {
            double overallLikelihood = 1;
            double PD;
            for (int i = 0; i < this.X.Length; i++)
            {
                PD = 0;
                for (int j = 0; j < this.K; j++)
                {
                    double likelihood = Likelihood(this.X[i], this.Means.Last()[j], this.sigma);
                    PD += likelihood * this.tau;
                }

                overallLikelihood *= PD;
            }

            return overallLikelihood;
        }

        /// <summary>
        /// Initialization step to start EM
        /// </summary>
        private void Initialize()
        {
            // Initialize estimates for means
            double[] initialMeans = new double[this.K];


            // Method: sort the data set then break up into k equal groups and take mean of each group
            // For simplicity, if k is not a factor of groupSize, ignore the remaining 1 item
            List<double> data = this.X.ToList();
            data.Sort();

            int maxGroupSize = (int)Math.Ceiling((double)this.X.Length / K);
            int minGroupSize = this.X.Length / K;

            for (int i = 0; i < this.K; i++)
            {
                double sum = 0;
                for (int j = 0; j < minGroupSize; j++)
                {

                    sum += data[j + (i * maxGroupSize)];
                }
                initialMeans[i] = sum / minGroupSize;
            }

            this.Means.Add(initialMeans);
        }

        /// <summary>
        /// Decide whether to terminate the algorithm
        /// </summary>
        private bool Terminate()
        {
            if (this.Means.Count <= 1)
            {
                return false;
            }

            if (this.iterations >= 100)
            {
                return false;
            }

            // Compare delta of last 2 iterations to see if change was less than epsilon
            double epsilon = 0.001;
            int lastIteration = this.Means.Count - 1;
            for (int j = 0; j < this.K; j++)
            {
                double delta = this.Means[lastIteration][j] - this.Means[lastIteration - 1][j];
                if (Math.Abs(delta) > epsilon)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
