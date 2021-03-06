﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    public class Read
    {
        public string[] Fields { get; private set; }
        public string Id { get; private set; }
        public int Flag { get; private set; }
        public string LocationChr { get; private set; }
        public int Location { get; private set; }
        public string Cigar { get; private set; }
        public string Sequence { get; set; }
        public string QualityScores { get; private set; }
        public int AS { get; private set; }
        public int NM { get; private set; }
        public string MD { get; private set; }

        // 0-based index of the first nt that is part of the poly-A tail
        public int CleavageSite { get; set; }

        public Read() { }

        public Read(string[] fields)
        {
            this.Fields = fields;
        }

        /// <summary>
        /// Returns true is this is a candidate read
        /// </summary>
        public bool MeetsCriteria()
        {
            this.Flag = Convert.ToInt32(this.Fields[1]);
            bool isUnmapped = (this.Flag & 0x4) != 0;
            if (isUnmapped)
            {
                // Must be considered a match
                return false;
            }

            this.Sequence = this.Fields[9];
            if (!this.Sequence.EndsWith("AAAA"))
            {
                // Ends with at least some A's
                return false;
            }

            this.Cigar = this.Fields[5];
            if (this.Cigar == "101M")
            {
                // Don't keep perfect matches (we expect some A's at the end to mismatch)
                return false;
            }

            if (!this.Cigar.EndsWith("S"))
            {
                // Don't keep perfect matches (we expect some A's at the end to mismatch)
                return false;
            }

            string softClipSizeString = String.Empty;
            int j = this.Cigar.Length - 2; // The character prior to the final "S"
            while (j > 0 && Char.IsDigit(this.Cigar[j]))
            {
                softClipSizeString = this.Cigar[j] + softClipSizeString;
                j--;
            }
            int softClipSize = Convert.ToInt32(softClipSizeString.ToString());
            this.CleavageSite = this.Sequence.Length - softClipSize;
            string softClipped = this.Sequence.Substring(this.CleavageSite);

            if (!softClipped.StartsWith("AAAA"))
            {
                // Soft clipped string start with some A's
                return false;
            }

            this.Id = this.Fields[0];
            this.LocationChr = this.Fields[2];
            this.Location = Convert.ToInt32(this.Fields[3]);
            this.QualityScores = this.Fields[10];

            for (int i = 11; i < this.Fields.Length; i++)
            {
                if (this.Fields[i].StartsWith("AS"))
                {
                    this.AS = Convert.ToInt32(this.Fields[i].Split(':')[2]);
                }
                else if (this.Fields[i].StartsWith("NM"))
                {
                    this.NM = Convert.ToInt32(this.Fields[i].Split(':')[2]);
                }
                else if (this.Fields[i].StartsWith("MD"))
                {
                    this.MD = this.Fields[i].Split(':')[2];
                }
            }

            return true;

        }
    }
}
