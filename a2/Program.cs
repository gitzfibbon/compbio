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

        public static void Main(string[] args)
        {
            Program.Test();
        }

        private static void Test()
        {
            Protein x1 = new Protein();
            x1.Encoding = "deadly";
            x1.Name = "x1";
            Protein x2 = new Protein();
            x2.Encoding = "ddgearlyk";
            x2.Name = "x2";

            SW sw = new SW(x1, x2);
            sw.ComputeScore();
            sw.PrintResult(true);
        }

        private static void Run()
        {


            Protein MYOD1_HUMAN = new Protein("P15172", "MYOD1_HUMAN");
            Protein TAL1_HUMAN = new Protein("P17542", "TAL1_HUMAN");
            Protein MYOD1_MOUSE = new Protein("P10085", "MYOD1_MOUSE");



        }

    }
}
