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

        public Read(string[] fields)
        {
            this.Fields = fields;
            this.Initialize();
        }

        private void Initialize()
        {
            this.Id = Fields[0];
        }
    }
}
