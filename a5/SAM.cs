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
        /// <summary>
        /// The list of candidates
        /// </summary>
        public List<Read> Candidates { get; set; }

        public void FindCandidates()
        {
            string samFileShort = @"C:\Users\jordanf\Downloads\SRR5831944.sorted.sam";
            string samFileLong = @"C:\Users\jordanf\Downloads\SRR5831944.resorted2.sam";
            string samSuperset1 = @"C:\Users\jordanf\Downloads\CandidateSuperset.sam";
            string samSuperset2 = @"data\CandidateSuperset.sam";
            this.ReadSamFile(samSuperset1, 0, false);
        }

        private void ReadSamFile(string fileName, int maxLines = 0, bool writeToFile = false)
        {
            StringBuilder sb = new StringBuilder();
            int batchWriteCount = 200000;
            int batchCount = 0;
            string outputFileName = "CandidateSuperset.sam";
            if (writeToFile && File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

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

                        if (writeToFile)
                        {
                            sb.AppendLine(line);
                            batchCount++;
                        }
                    }
                }

                if (maxLines > 0 && totalCount >= maxLines)
                {
                    Console.WriteLine("Read the specified max number of lines ({0})", maxLines.ToString("N0"));
                    break;
                }

                if (writeToFile && batchCount >= batchWriteCount )
                {
                    // Flush
                    Console.WriteLine("Flush");
                    File.AppendAllText(outputFileName, sb.ToString());
                    sb = new StringBuilder();
                    batchCount = 0;
                }
            }

            file.Close();
            Console.WriteLine("Read {0} lines", totalCount.ToString("N0"));
            Console.WriteLine("Found {0} candidates", this.Candidates.Count.ToString("N0"));

            if (writeToFile)
            {
                // Create a more manageable list to work with
                File.AppendAllText(outputFileName, sb.ToString());
            }
        }


    }
}
