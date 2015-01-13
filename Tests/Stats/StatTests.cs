using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Stats;
using Turnable.Api;

namespace Tests.Stats
{
    [TestClass]
    public class StatTests
    {
        private IStatManager _statManager;
        private IStat _stat;
        private bool _eventTriggeredFlag;
        private StatChangedEventArgs _statChangedEventArgs;

        [TestInitialize]
        public void Initialize()
        {
            _statManager = new StatManager();
            _stat = _statManager.BuildStat("Health", 100);
        }

        //[TestMethod]
        //public void Stat_ConstructionWithAHealthStat_IsSuccessful()
        //{
        //    Stat stat = _statManager.CreateStat("Hit Chance", 10, 5, 95, true);
        //    Assert.IsTrue(stat.IsHealth);
        //}

        [TestMethod]
        public void Value_CanBeChanged()
        {
            _stat.Value -= 5;

            Assert.AreEqual(95, _stat.Value);
        }

        [TestMethod]
        public void Value_CanBeSetTo0()
        {
            _stat.Value -= 100;

            Assert.AreEqual(0, _stat.Value);
        }

        [TestMethod]
        public void Value_IsClampedToAMinimumValueOf0ByDefault()
        {
            _stat.Value -= 101;
            Assert.AreEqual(0, _stat.Value);

            _stat.Value = -50;
            Assert.AreEqual(0, _stat.Value);
        }

        [TestMethod]
        public void Value_IsClampedToAMaximumValueOf100ByDefault()
        {
            _stat.Value += 100;
            Assert.AreEqual(100, _stat.Value);

            _stat.Value = 101;
            Assert.AreEqual(100, _stat.Value);
        }

        [TestMethod]
        public void Value_CanBeClampedToAValueOtherThanTheDefault()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            
            stat.Value += 100;
            Assert.AreEqual(95, stat.Value);

            stat.Value -= 100;
            Assert.AreEqual(5, stat.Value);
        }

        [TestMethod]
        public void Reset_SetsTheValueToTheInitialValue()
        {
            int initialValue = _stat.Value;
            _stat.Value -= 50;

            _stat.Reset();

            Assert.AreEqual(100, _stat.Value);
        }

        [TestMethod]
        public void Value_WhenChanged_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value -= 10;

            Assert.IsTrue(_eventTriggeredFlag);

            Assert.AreEqual(_stat, _statChangedEventArgs.Stat);
            Assert.AreEqual(100, _statChangedEventArgs.OldValue);
            Assert.AreEqual(90, _statChangedEventArgs.NewValue);
        }

        //[TestMethod]
        //public void Stat_WhenChangedAndClampedToMaximumValue_RaisesAChangedEvent()
        //{
        //    _stat.Changed += this.SetEventTriggeredFlag;
        //    _stat.Value += 10;

        //    Assert.IsTrue(_eventTriggeredFlag);

        //    StatChangedEventArgs e = (StatChangedEventArgs)_eventArgs;
        //    Assert.AreEqual(_stat, e.Stat);
        //}

        //[TestMethod]
        //public void Stat_WhenChangedAndClampedToMinimumValue_RaisesAChangedEvent()
        //{
        //    _stat.Changed += this.SetEventTriggeredFlag;
        //    _stat.Value -= 250;

        //    Assert.IsTrue(_eventTriggeredFlag);

        //    StatChangedEventArgs e = (StatChangedEventArgs)_eventArgs;
        //    Assert.AreEqual(_stat, e.Stat);
        //}

        private void SetEventTriggeredFlag(object sender, StatChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _statChangedEventArgs = e;
        }
    }
}
