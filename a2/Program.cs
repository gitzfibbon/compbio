using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2
{
    class Program
    {

        static void Main(string[] args)
        {
            Blosum62 blosum62 = new Blosum62();
            Protein MYOD1_HUMAN = new Protein("P15172");
            Protein TAL1_HUMAN = new Protein("P17542");
            Protein MYOD1_MOUSE = new Protein("P10085");
        }

    }
}
