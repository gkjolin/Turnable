using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Randomization;

namespace Tests.Randomization
{
    [TestClass]
    public class PrngTests
    {
        [TestMethod]
        public void Prng_AskedToGenerateARandomNumberInARange_ConsidersTheMinimumValueToBeInclusiveAndTheUpperValueToBeExclusive()
        {
            int randomValue = Prng.Next(1, 101);

            Assert.IsTrue(randomValue >= 1);
            Assert.IsTrue(randomValue < 101);
        }
    }
}
