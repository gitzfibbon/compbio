using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    class Probability
    {
        /// <summary>
        /// Create a permutation of this protein
        /// </summary>
        public static Protein Permute(Protein protein)
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
