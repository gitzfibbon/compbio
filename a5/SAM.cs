using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    class Sam
    {
        public Sam()
        {

        }

        public void FindCandidates()
        {
            this.ReadSamFile(@"C:\Users\jordanf\Downloads\SRR5831944.sorted.sam");
        }

        private void ReadSamFile(string fileName)
        {
            // File is 4GB so stream read not all at once
            int counter = 0;
            string line;

            StreamReader file = new StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                counter++;
                string[] fields = line.Split('\t');

                if (fields.Length >= 12)
                {
                    Read read = new Read(fields);
                }
            }

            file.Close();
            Console.WriteLine("There were {0} lines.", counter);
        }


    }
}
