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
        private bool _eventTriggeredFlag;
        private EventArgs _eventArgs;

        [TestInitialize]
        public void Initialize()
        {
            _statManager = new StatManager();
            _stat = _statManager.CreateStat("Health", 100);
            _eventTriggeredFlag = false;
        }

        [TestMethod]
        public void Stat_ConstructionWithNonDefaultMinimumAndMaximumValues_IsSuccessful()
        {
            Stat stat = _statManager.CreateStat("Hit Chance", 10, 5, 95);
            Assert.AreEqual(5, stat.MinimumValue);
            Assert.AreEqual(95, stat.MaximumValue);
        }

        [TestMethod]
        public void Stat_ConstructionWithAHealthStat_IsSuccessful()
        {
            Stat stat = _statManager.CreateStat("Hit Chance", 10, 5, 95, true);
            Assert.IsTrue(stat.IsHealth);
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

        [TestMethod]
        public void Stat_WhenChanged_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value -= 10;

            Assert.IsTrue(_eventTriggeredFlag);

            StatChangedEventArgs e = (StatChangedEventArgs)_eventArgs;
            Assert.AreEqual(_stat, e.Stat);
        }

        [TestMethod]
        public void Stat_WhenChangedAndClampedToMaximumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value += 10;

            Assert.IsTrue(_eventTriggeredFlag);

            StatChangedEventArgs e = (StatChangedEventArgs)_eventArgs;
            Assert.AreEqual(_stat, e.Stat);
        }

        [TestMethod]
        public void Stat_WhenChangedAndClampedToMinimumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value -= 250;

            Assert.IsTrue(_eventTriggeredFlag);

            StatChangedEventArgs e = (StatChangedEventArgs)_eventArgs;
            Assert.AreEqual(_stat, e.Stat);
        }


        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = e;
        }
    }
}
