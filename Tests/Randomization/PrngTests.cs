using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Randomization;

namespace Tests.Randomization
{
    [TestClass]
    public class PrngTests
    {
        [TestMethod]
        public void Prng_AskedToGenerateARandomIntegerInARange_ConsidersTheMinimumValueToBeInclusiveAndTheUpperValueToBeExclusive()
        {
            int randomValue = Prng.Next(1, 101);

            Assert.IsTrue(randomValue >= 1);
            Assert.IsTrue(randomValue < 101);
        }

        [TestMethod]
        public void Prng_AskedToGenerateARandomIntegerInAReasonableLargeRange_DoesNotRepeatIfNextIsCalledInSuccession()
        {
            int randomValue = Prng.Next(1, 1001);
            int randomValue2 = Prng.Next(1, 1001);
            int randomValue3 = Prng.Next(1, 1001);
            int randomValue4 = Prng.Next(1, 1001);
            int randomValue5 = Prng.Next(1, 1001);

            Assert.AreNotEqual(randomValue, randomValue2);
            Assert.AreNotEqual(randomValue2, randomValue3);
            Assert.AreNotEqual(randomValue3, randomValue4);
            Assert.AreNotEqual(randomValue4, randomValue5);
        }

        [TestMethod]
        public void Prng_AskedToGenerateARandomDouble_ReturnsAValueBetween0And1()
        {
            double randomValue = Prng.NextDouble();

            Assert.IsTrue(randomValue >= 0.0);
            Assert.IsTrue(randomValue <= 1.0);
        }
    }
}
