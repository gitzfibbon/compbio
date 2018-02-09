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
        /// <summary>
        /// To run from command line pass in the input file and a random seed for initialization.
        /// Eg. a3.exe input.txt 12345
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                BestBIC(@"data\input2.txt", 2);
                //Test1_5d();
                //Test2();
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            else if (args.Length == 2)
            {
                BestBIC(args[0], Convert.ToInt32(args[1]));
            }
            else
            {
                Console.WriteLine("Provide input file as first argument and random seed as second argument");
                Console.WriteLine("Example: a3.exe input.txt 12345");
            }
        }

        private static void BestBIC(string inputFile, int randomSeed)
        {
            double[] data = ReadData(inputFile);

            List<EM> results = new List<EM>();

            int maxK = 5;
            int highestBICIndex = 0;
            for (int k = 0; k < maxK; k++)
            {
                EM em = new EM(data, k + 1);
                em.Initialize(randomSeed);
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
            sb.AppendLine();

            sb.Append(PrintEM(results[highestBICIndex], false));

            Console.WriteLine(sb.ToString());

            File.WriteAllText("results.txt", sb.ToString());
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
            EM em = new EM(data, 3);
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

        private static string PrintEM(EM em, bool printToConsole = true)
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

            if (printToConsole)
            {
                Console.WriteLine(sb.ToString());
            }

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
