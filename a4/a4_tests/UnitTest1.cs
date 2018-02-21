using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a4;

namespace a4_tests
{
    [TestClass]
    public class A4Tests
    {
        [TestMethod]
        public void TestCalculateHits()
        {
            int[] traceBackPath = new int[10];
            traceBackPath[0] = 1;
            traceBackPath[1] = 1;
            traceBackPath[2] = 1;
            traceBackPath[3] = 1;
            traceBackPath[4] = 1;
            traceBackPath[5] = 0;
            traceBackPath[6] = 0;
            traceBackPath[7] = 0;
            traceBackPath[8] = 1;
            traceBackPath[9] = 1;

            var result = Viterbi.CalculateHits(traceBackPath);
        }
    }
}
