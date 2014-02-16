using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy;
using System.Collections.Generic;
using TurnItUp.Stats;

namespace Tests.Stats
{
    [TestClass]
    public class StatTests
    {
        private StatManager _statManager;
        private Stat _stat;

        [TestInitialize]
        public void Initialize()
        {
            _statManager = new StatManager();
            _stat = _statManager.CreateStat("Health", 100);
        }

        [TestMethod]
        public void Stat_ConstructionWithNonDefaultMinimumAndMaximumValues_IsSuccessful()
        {
            Stat stat = _statManager.CreateStat("Hit Chance", 10, 5, 95);
            Assert.AreEqual(5, stat.MinimumValue);
            Assert.AreEqual(95, stat.MaximumValue);
        }

        [TestMethod]
        public void Stat_CanBeReduced()
        {
            _stat.Value -= 5;
            Assert.AreEqual(95, _stat.Value);
        }

        [TestMethod]
        public void Stat_CanBeReducedToZero()
        {
            _stat.Value -= 100;
            Assert.AreEqual(0, _stat.Value);
        }

        [TestMethod]
        public void Stat_WhenReduced_IsClampedToAMinimumValueOf0ByDefault()
        {
            _stat.Value -= 101;
            Assert.AreEqual(0, _stat.Value);
        }

        [TestMethod]
        public void Stat_WhenIncreased_IsClampedToAMaximumValueOf100ByDefault()
        {
            _stat.Value += 100;
            Assert.AreEqual(100, _stat.Value);
        }

        [TestMethod]
        public void Stat_WhenReducedOrIncreased_ClampsToValuesOtherThanTheDefault()
        {
            Stat stat = _statManager.CreateStat("Hit Chance", 10, 5, 95);
            stat.Value += 100;
            Assert.AreEqual(95, stat.Value);
            stat.Value -= 100;
            Assert.AreEqual(5, stat.Value);
        }

        [TestMethod]
        public void Stat_CanResetItself()
        {
            _stat.Value -= 3;
            _stat.Reset();
            Assert.AreEqual(100, _stat.Value);
        }
    }
}
