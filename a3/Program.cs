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

            PrintEM(em);
        }

        private static void PrintEM(EM em)
        {
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j< em.K; j++)
            {
                sb.Append(("mu" + j+1).PadLeft(10));
            }

            Console.WriteLine(sb.ToString());
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
