using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    class Read
    {
        public string[] Fields { get; set; }
        public string Id { get; set; }
        public int Flag { get; set; }
        public string LocationChr { get; set; }
        public int Location { get; set; }
        public string Cigar { get; set; }
        public string Sequence { get; set; }
        public string QualityScores { get; set; }
        public int AS { get; set; }
        public int NM { get; set; }
        public string MD { get; set; }

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
            bool isUnmapped = (this.Flag & 4) == 0;
            if (isUnmapped)
            {
                return false;
            }

            this.Id = this.Fields[0];
            this.LocationChr = this.Fields[2];
            this.Location = Convert.ToInt32(this.Fields[3]);
            this.Cigar = this.Fields[5];
            this.Sequence = this.Fields[9];
            this.QualityScores = this.Fields[10];

            //for (int i=11; i< this.Fields.Length; i++)
            //{
            //    if (this.Fields[i].StartsWith("AS"))
            //    {
            //        this.AS = Convert.ToInt32(this.Fields[i].Split(':')[2]);
            //    }
            //    else if (this.Fields[i].StartsWith("NM"))
            //    {
            //        this.AS = Convert.ToInt32(this.Fields[i].Split(':')[2]);
            //    }
            //    else if (this.Fields[i].StartsWith("MD"))
            //    {
            //        this.MD = this.Fields[i].Split(':')[2];
            //    }
            //}

            return true;

        }
    }
}
