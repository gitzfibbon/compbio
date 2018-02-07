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

        public EM(double[] data, int numClusters)
        {
            this.x = data;
            this.k = numClusters;
            this.means = new List<double[]>();
            this.tau = 1d / this.k; // fixed
            this.sigma = 1; // fixed

            this.Initialize();
        }

        public void Run()
        {
            this.Initialize();

        }

        private void E()
        {

        }

        private void M()
        {

        }

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

        public double Likelihood(double xi, double mu, double sigma)
        {
            double sigma_squared = Math.Pow(sigma, 2);
            double e_power = -1 * Math.Pow(xi - mu, 2) / (2 * sigma_squared);
            double likelihood = Math.Exp(e_power) / Math.Sqrt(2 * Math.PI * sigma_squared);

            return likelihood;
        }

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
