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
            BestBIC();
            //Test1_5d();
            //Test2();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void BestBIC()
        {
            double[] data = ReadData(@"data\input2.txt");

            List<EM> results = new List<EM>();

            int maxK = 5;
            int highestBICIndex = 0;
            for (int k = 0; k < maxK; k++)
            {
                EM em = new EM(data, k + 1);
                em.Initialize(2);
                em.Run();
                results.Add(em);

                // Keep track of max BIC
                if (em.BICs.Last() > results[highestBICIndex].BICs.Last())
                {
                    highestBICIndex = k;
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("k".PadLeft(6));
            sb.Append("BIC".PadLeft(14));
            sb.AppendLine();
            for (int i = 0; i < maxK; i++)
            {
                sb.Append((i + 1).ToString().PadLeft(6));
                sb.Append(results[i].BICs.Last().ToString("F6").PadLeft(14));

                if (i == highestBICIndex)
                {
                    sb.Append("  <--- Highest BIC");
                }

                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());

            PrintEM(results[highestBICIndex]);
        }

        private static void Test1_3()
        {
            double[] data = ReadData(@"data\input1.txt");
            EM em = new EM(data, 3);
            em.Initialize(new double[3] { 21, 46, 55 });
            em.Run();
            PrintEM(em);
        }

        private static void Test1_5a()
        {
            double[] data = ReadData(@"data\input1.txt");
            EM em = new EM(data, 5);
            em.Initialize(new double[5] { 35, 12, 46, 22, 45 });
            em.Run();
            PrintEM(em);
        }

        private static void Test1_5b()
        {
            double[] data = ReadData(@"data\input1.txt");
            EM em = new EM(data, 5);
            em.Initialize(new double[5] { 9, 10, 46, 49, 57 });
            em.Run();
            PrintEM(em);
        }

        private static void Test1_5c()
        {
            double[] data = ReadData(@"data\input1.txt");
            EM em = new EM(data, 5);
            em.Initialize();
            em.Run();
            PrintEM(em);
        }

        private static void Test1_5d()
        {
            double[] data = ReadData(@"data\input1.txt");
            EM em = new EM(data, 5);
            em.Initialize(2);
            em.Run();
            PrintEM(em);
        }

        private static void Test2()
        {
            double[] data = ReadData(@"data\input2.txt");
            EM em = new EM(data, 5);
            em.Initialize(new double[5] { 35, 12, 46, 22, 45 });
            em.Run();
            PrintEM(em);
        }

        private static string PrintEM(EM em)
        {
            int padSize = 15;
            string numberFormat = "F6";
            StringBuilder sb = new StringBuilder();

            // Print Mean, LL, BIC

            for (int j = 0; j < em.K; j++)
            {
                sb.Append(("mu" + (j + 1)).PadLeft(padSize));
            }
            sb.Append("LogLik".PadLeft(padSize));
            sb.Append("BIC".PadLeft(padSize));

            sb.AppendLine();

            for (int i = 0; i < em.Means.Count; i++)
            {
                for (int j = 0; j < em.K; j++)
                {
                    sb.Append(em.Means[i][j].ToString(numberFormat).PadLeft(padSize));
                }
                sb.Append(em.LogLikelihoods[i].ToString(numberFormat).PadLeft(padSize));
                sb.Append(em.BICs[i].ToString(numberFormat).PadLeft(padSize));
                sb.AppendLine();
            }

            // Print xi
            sb.AppendLine();
            sb.Append("xi".PadLeft(padSize));
            for (int j = 0; j < em.K; j++)
            {
                sb.Append(("P(cls " + (j + 1) + " | xi)").PadLeft(padSize));
            }
            sb.AppendLine();

            for (int i = 0; i < em.X.Length; i++)
            {
                sb.Append(("[" + (i + 1) + "]").PadLeft(6));
                sb.Append(em.X[i].ToString().PadLeft(padSize - 6));
                for (int j = 0; j < em.K; j++)
                {
                    sb.Append(em.E_Steps.Last()[i, j].ToString("e6").PadLeft(padSize));
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        private static double[] ReadData(string inputFile)
        {
            List<double> data = new List<double>();

            string[] input = File.ReadAllText(inputFile).Split(' ', '\t', '\n');
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
