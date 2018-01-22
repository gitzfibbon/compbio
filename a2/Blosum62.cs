using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    class Blosum62
    {
        // BLOSUM 62: https://www.ncbi.nlm.nih.gov/Class/FieldGuide/BLOSUM62.txt

        public Dictionary<string, int> AAMap
        {
            get;
            private set;
        }

        public Blosum62()
        {
            this.LoadBlosum62();
        }

        private void LoadBlosum62()
        {
            string[] input = File.ReadAllLines(@"data\Blosum62.csv");

            this.AAMap = new Dictionary<string, int>();
            string[] aa = input[0].Split(',');
            for (int i = 1; i < 21; i++)
            {
                this.AAMap.Add(aa[i], i);
            }
            
            int[,] blosum62 = new int[20, 20];
            for (int i=1; i<21; i++)
            {
                string[] row = input[i].Split(',');
                for (int j = 1; j < 21; j++)
                {
                    blosum62[i-1, j-1] = Convert.ToInt32(row[j]);
                }
            }
        }
    }
}
