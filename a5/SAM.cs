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
        public List<Read> Candidates { get; set; }

        public void FindCandidates()
        {
            string samFileShort = @"C:\Users\jordanf\Downloads\SRR5831944.sorted.sam";
            string samFileLong = @"C:\Users\jordanf\Downloads\SRR5831944.resorted2.sam";
            this.ReadSamFile(samFileLong);
        }

        private void ReadSamFile(string fileName)
        {
            // File is 4GB so stream read not all at once
            this.Candidates = new List<Read>();
            int totalCount = 0;
            string line;

            StreamReader file = new StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                totalCount++;
                string[] fields = line.Split('\t');

                if (fields.Length >= 12)
                {
                    Read read = new Read(fields);
                    if (read.MeetsCriteria())
                    {
                        this.Candidates.Add(read);
                    }
                }
            }

            file.Close();
            Console.WriteLine("Read {0} lines", totalCount);
            Console.WriteLine("Found {0} candidates", this.Candidates.Count);
        }


    }
}
