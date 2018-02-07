using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
    public class EM
    {
        public double[] x { get; private set; }

        public int k { get; private set; }

        public List<double[]> means { get; private set; }

        public double tau { get; private set; }

        public double sigma { get; private set; }

        public List<double[,]> e_steps { get; private set; }

        public EM(double[] data, int numClusters)
        {
            this.x = data;
            this.k = numClusters;
            this.means = new List<double[]>();
            this.tau = 1d / this.k; // fixed
            this.sigma = 1; // fixed
            this.e_steps = new List<double[,]>();

            this.Initialize();
        }

        public void Run()
        {
            this.Initialize();
            this.E();

        }

        #region E Step

        /// <summary>
        /// E step: calculate the expected zij for every i and j
        /// </summary>
        public void E()
        {
            double[,] e_step = new double[this.x.Length, this.k];
            for (int i = 0; i < this.x.Length; i++)
            {
                for (int j = 0; j < this.k; j++)
                {
                    e_step[i, j] = Expected_zij(this.x[i], j);
                }
            }

            this.e_steps.Add(e_step);
        }

        /// <summary>
        /// Helper for the E step. Calculates the Expected zij for a particular xi and j.
        /// </summary>
        public double Expected_zij(double xi, int j)
        {
            double numerator = Likelihood(xi, this.means.Last()[j], this.sigma) * this.tau;

            List<double> denominators = new List<double>(); // for debugging
            double denominator = 0;
            for (int k = 0; k < this.k; k++)
            {
                double likelihood = Likelihood(xi, this.means.Last()[k], this.sigma);
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
            double[] updatedMeans = new double[this.k];

            for (int j=0; j<this.k; j++)
            {
                updatedMeans[j] = Mu_j(j);
            }

            this.means.Add(updatedMeans);
        }

        /// <summary>
        /// Helper for the M step. Updates mu (theta) at j.
        /// </summary>
        public double Mu_j(int j)
        {
            double numerator = 0;
            double denominator = 0;
            for (int i = 0; i < this.x.Length; i++)
            {
                numerator += this.e_steps.Last()[i, j] * this.x[i];
                denominator += this.e_steps.Last()[i, j];

            }

            return numerator / denominator;
        }

        #endregion

        /// <summary>
        /// Initialization step to start EM
        /// </summary>
        private void Initialize()
        {
            // Initialize estimates for means
            double[] initialMeans = new double[this.k];


            // Method: sort the data set then break up into k equal groups and take mean of each group
            // For simplicity, if k is not a factor of groupSize, ignore the remaining 1 item
            List<double> data = this.x.ToList();
            data.Sort();

            int maxGroupSize = (int)Math.Ceiling((double)this.x.Length / k);
            int minGroupSize = this.x.Length / k;

            for (int i = 0; i < this.k; i++)
            {
                double sum = 0;
                for (int j = 0; j < minGroupSize; j++)
                {

                    sum += data[j + (i * maxGroupSize)];
                }
                initialMeans[i] = sum / minGroupSize;
            }

            this.means.Add(initialMeans);
        }
    }
}
