using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using a3;

namespace a3_tests
{
    [TestClass]
    public class EMTests
    {
        [TestMethod]
        public void TestLikelihood()
        {
            EM em = new EM(new double[0], 1);
            double likelihood = em.Likelihood(2, 0, 1);
            Console.WriteLine(likelihood);
        }

        [TestMethod]
        public void TestExpectedzij()
        {
            EM em = new EM(new double[5] {-2, -1, 0, 1, 2 }, 2);
            double expected = em.Expected_zij(0, 1);
            Assert.AreEqual(0.5, expected, "0 falls in the middle and should be 0.5");
            Console.WriteLine(expected);
        }

        [TestMethod]
        public void TestEStep()
        {
            EM em = new EM(new double[5] { -2, -1, 0, 1, 2 }, 3);
            em.E();
        }
    }
}
