using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] data = ReadData(@"data\input1.txt");

            EM em = new EM(data, 3);
            em.Run();
        }

        private static void PrintEM(EM em)
        {

        }

        private static double[] ReadData(string inputFile)
        {
            List<double> data = new List<double>();

            string[] input = File.ReadAllText(inputFile).Split(' ','\t','\n');
            foreach (string s in input)
            {
                if (!String.IsNullOrWhiteSpace(s))
                {
                    data.Add(Convert.ToDouble(s));
                }
            }

            return data.ToArray();
        }
    }
}
