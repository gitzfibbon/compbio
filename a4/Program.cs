using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4
{
    class Program
    {
        static void Main(string[] args)
        {
            RunViterbiTraining();
            //RunViterbi();
        }

        private static void RunViterbiTraining()
        {
            string genome = ReadGenome(@"data\GCF_000091665.1_ASM9166v1_genomic.fna");

            ViterbiTraining vt = new ViterbiTraining(genome);
            vt.Run(1);

            //string text = viterbi.Print();
            //Console.WriteLine(text);
            //File.WriteAllText("results.txt", text);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }


        private static void RunViterbi()
        {
            string genome = ReadGenome(@"data\GCF_000091665.1_ASM9166v1_genomic.fna");
            Viterbi viterbi = new Viterbi(genome);
            viterbi.Run();

            string text = viterbi.Print();
            Console.WriteLine(text);
            File.WriteAllText("viterbi_results.txt", text);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static string ReadGenome(string filePath)
        {
            string[] data = File.ReadAllLines(filePath);
            StringBuilder genome = new StringBuilder();

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i].StartsWith(">"))
                {
                    break;
                }

                string line = data[i].Trim().ToUpper();
                for (int j = 0; j<line.Length; j++)
                {
                    if (line[j] == 'A' || line[j] == 'C' || line[j] == 'G')
                    {
                        genome.Append(line[j]);
                    }
                    else
                    {
                        // Treat any other letter as 'T'
                        genome.Append('T');
                    }
                    
                }
                
            }

            return genome.ToString();
        }
    }
}
