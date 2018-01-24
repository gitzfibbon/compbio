using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    class Probability
    {
        public static double EmpiricalProbability(Protein primary, Protein secondary, int numPermutations)
        {
            SW sw = new SW(primary, secondary);
            sw.ComputeScore(false);
            int score = sw.Score;

            double k = 1;
            double N = numPermutations + 1;

            for (int i = 0; i < numPermutations; i++)
            {
                Protein pSecondary = Permute(secondary);

                sw = new SW(primary, pSecondary);
                sw.ComputeScore(false);

                if (sw.Score >= score)
                {
                    k++;
                }
            }

            return k / N;
        }

        private static int PermuteAndScore(Protein primary, Protein secondary)
        {
            // Permute the secondary protein
            Protein pSecondary = Permute(secondary);

            // Align the two proteins
            SW sw = new SW(primary, pSecondary);
            sw.ComputeScore(false);
            return sw.Score;
        }

        /// <summary>
        /// Create a permutation of this protein
        /// </summary>
        private static Protein Permute(Protein protein)
        {
            Random r = new Random();
            char[] permutation = protein.Encoding.ToCharArray();
            int n = permutation.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = r.Next(0, i);

                char temp = permutation[i];
                permutation[i] = permutation[j];
                permutation[j] = temp;
            }

            Protein newProtein = new Protein();
            newProtein.Accession = "PERM_" + protein.Accession;
            newProtein.Name = "PERM_" + protein.Name;
            newProtein.Encoding = new String(permutation);

            return newProtein;
        }


    }
}
