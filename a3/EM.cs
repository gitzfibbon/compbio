using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
    class EM
    {
        public double[] x { get; private set; }

        public int k { get; private set; }

        public List<double[]> means { get; private set; }

        public EM(double[] data, int numClusters)
        {
            this.x = data;
            this.k = numClusters;
            this.means = new List<double[]>();

        }

        public void Run()
        {
            this.Initialize();

        }

        private void E()
        {

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

            int maxGroupSize =  (int)Math.Ceiling((double)this.x.Length / k);
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
