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
            //PermuteTest0();
            Test0();
        }

        private static void PermuteTest0()
        {
            Protein x1 = new Protein();
            x1.Encoding = "deadly";
            x1.Name = "x1";
            x1.Accession = "D001";
            Protein x2 = new Protein();
            x2.Encoding = "ddgearlyk";
            x2.Name = "x2";
            x2.Accession = "D002";

            double result = Probability.EmpiricalProbability(x1, x2, 1000);
        }

        private static void Test0()
        {
            Protein x1 = new Protein();
            x1.Encoding = "ara"; 
            x1.Name = "x1";
            x1.Accession = "X001";
            Protein x2 = new Protein();
            x2.Encoding = "aa";
            x2.Name = "x2";
            x2.Accession = "X002";

            SW sw = new SW(x1, x2);
            sw.ComputeScore();
            sw.PrintResult(true);
        }

        private static void Test1()
        {
            Protein x1 = new Protein();
            x1.Encoding = "deadly";
            x1.Name = "x1";
            x1.Accession = "D001";
            Protein x2 = new Protein();
            x2.Encoding = "ddgearlyk";
            x2.Name = "x2";
            x2.Accession = "D002";

            SW sw = new SW(x1, x2);
            sw.ComputeScore();
            sw.PrintResult(true);
        }

        private static void Test2()
        {
            Protein HBB_HUMAN = new Protein("P68871", "HBB_HUMAN");
            Protein Q14SN0_APIME = new Protein("Q14SN0", "Q14SN0_APIME");

            SW sw = new SW(HBB_HUMAN, Q14SN0_APIME);
            sw.ComputeScore();
            sw.PrintResult(true);
        }

        private static void Run()
        {

            Protein MYOD1_HUMAN = new Protein("P15172", "MYOD1_HUMAN");
            Protein TAL1_HUMAN = new Protein("P17542", "TAL1_HUMAN");
            Protein MYOD1_MOUSE = new Protein("P10085", "MYOD1_MOUSE");

            Protein MYOD1_CHICK = new Protein("P16075", "MYOD1_CHICK");
            Protein MYODA_XENLA = new Protein("P13904", "MYODA_XENLA");
            Protein MYOD1_DANRE = new Protein("Q90477", "MYOD1_DANRE");
            Protein Q8IU24_BRABE = new Protein("Q8IU24", "Q8IU24_BRABE");
            Protein MYOD_DROME = new Protein("P22816", "MYOD_DROME");
            Protein LIN32_CAEEL = new Protein("Q10574", "LIN32_CAEEL");
            Protein SYFM_HUMAN = new Protein("O95363", "SYFM_HUMAN");



        }

    }
}
