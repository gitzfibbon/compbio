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

        public string FindCandidates()
        {
            string samFileShort = @"C:\Users\jordanf\Downloads\SRR5831944.sorted.sam";
            string samFileLong = @"C:\Users\jordanf\Downloads\SRR5831944.resorted2.sam";
            string samSuperset1 = @"C:\Users\jordanf\Downloads\CandidateSuperset.sam";
            string samSuperset2 = @"data\CandidateSuperset.sam";
            return this.ReadSamFile(samSuperset1, 0, false);
        }

        private string ReadSamFile(string fileName, int maxLines = 0, bool writeToFile = false)
        {
            StringBuilder sbInfo = new StringBuilder();

            StringBuilder sbRawCandidates = new StringBuilder();
            int batchWriteCount = 200000;
            int batchCount = 0;
            string outputFileName = "CandidateSuperset.sam";
            if (writeToFile && File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            // File is 14GB so stream read not all at once
            this.Candidates = new List<Read>();
            int totalReadCount = 0;
            string line;

            StreamReader file = new StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                string[] fields = line.Split('\t');

                if (fields.Length >= 12)
                {
                    totalReadCount++;

                    Read read = new Read(fields);
                    if (read.MeetsCriteria())
                    {
                        this.Candidates.Add(read);

                        if (writeToFile)
                        {
                            sbRawCandidates.AppendLine(line);
                            batchCount++;
                        }
                    }
                }

                if (maxLines > 0 && totalReadCount >= maxLines)
                {
                    sbInfo.AppendLine(String.Format("Read the specified max number of lines ({0})", maxLines.ToString("N0")));
                    break;
                }

                if (writeToFile && batchCount >= batchWriteCount )
                {
                    // Flush
                    sbInfo.AppendLine(String.Format("Flush"));
                    File.AppendAllText(outputFileName, sbRawCandidates.ToString());
                    sbRawCandidates = new StringBuilder();
                    batchCount = 0;
                }
            }

            file.Close();
            sbInfo.AppendLine("Reads: " + totalReadCount.ToString("N0"));
            sbInfo.AppendLine("Candidates: " + this.Candidates.Count.ToString("N0"));

            if (writeToFile)
            {
                // Create a more manageable list to work with
                File.AppendAllText(outputFileName, sbRawCandidates.ToString());
            }

            return sbInfo.ToString();
        }

    }
}
