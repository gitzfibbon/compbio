using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    public static class Blosum62
    {
        // BLOSUM 62: https://www.ncbi.nlm.nih.gov/Class/FieldGuide/BLOSUM62.txt

        private static int[,] matrix = null;
        private static int[,] Matrix
        {
            get
            {
                if (Blosum62.matrix == null)
                {
                    Blosum62.LoadBlosum62();
                }

                return matrix;
            }
        }

        private static Dictionary<string, int> aaMap = null;
        public static Dictionary<string, int> AAMap
        {
            get
            {
                if (Blosum62.aaMap == null)
                {
                    Blosum62.LoadBlosum62();
                }

                return Blosum62.aaMap;
            }
        }

        public static int Sigma(string aa1, string aa2)
        {
            return Blosum62.Matrix[AAMap[aa1.ToUpper()],AAMap[aa2.ToUpper()]];
        }

        private static void LoadBlosum62()
        {
            string[] input = File.ReadAllLines(@"data\Blosum62.csv");

            Blosum62.aaMap = new Dictionary<string, int>();
            string[] aa = input[0].Split(',');
            for (int i = 1; i < 21; i++)
            {
                Blosum62.aaMap.Add(aa[i], i-1);
            }

            int[,] blosum62 = new int[20, 20];
            for (int i = 1; i < 21; i++)
            {
                string[] row = input[i].Split(',');
                for (int j = 1; j < 21; j++)
                {
                    blosum62[i - 1, j - 1] = Convert.ToInt32(row[j]);
                }
            }

            Blosum62.matrix = blosum62;
        }
    }
}
