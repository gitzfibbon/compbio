using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    class Protein
    {

        private string encoding;
        public string Encoding
        {
            get
            {
                return this.encoding; 
            }

            set
            {
                this.encoding = value.ToUpper();   
            }
        }

        public string Name { get; set;}

        public string Accession { get; set; }

        public Protein() { }

        public Protein(string accession, string name)
        {
            this.Accession = accession;
            this.Name = name;
            this.LoadProtein(accession);
        }

        private void LoadProtein(string accession)
        {
            string fileName = Path.Combine("data", accession + ".fasta.txt");
            string[] input = File.ReadAllLines(fileName);

            StringBuilder protein = new StringBuilder();
            foreach (string line in input)
            {
                if (line.StartsWith(">")) { continue; }
                else
                {
                    protein.Append(line);
                }
            }

            this.Encoding = protein.ToString();

        }
    }
}
