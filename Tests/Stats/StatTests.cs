using System;
using NUnit.Framework;
using Turnable.Stats;
using Turnable.Api;

namespace Tests.Stats
{
    [TestFixture]
    public class StatTests
    {
        private IStatManager _statManager;
        private IStat _stat;
        private bool _eventTriggeredFlag;
        private StatChangedEventArgs _statChangedEventArgs;

        [SetUp]
        public void Initialize()
        {
            _statManager = new StatManager();
            _stat = _statManager.BuildStat("Health", 90);
        }

        [Test]
        public void Value_CanBeChanged()
        {
            _stat.Value -= 5;

            Assert.AreEqual(85, _stat.Value);
        }

        [Test]
        public void Value_CanBeSetTo0()
        {
            _stat.Value -= 90;

            Assert.AreEqual(0, _stat.Value);
        }

        [Test]
        public void Value_IsClampedToAMinimumValueOf0ByDefault()
        {
            _stat.Value -= 100;
            Assert.AreEqual(0, _stat.Value);
        }

        [Test]
        public void Value_IsClampedToAMaximumValueOf100ByDefault()
        {
            _stat.Value += 100;
            Assert.AreEqual(100, _stat.Value);
        }

        [Test]
        public void Value_CanBeClampedToAValueOtherThanTheDefault()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            
            stat.Value += 100;
            Assert.AreEqual(95, stat.Value);

            stat.Value -= 100;
            Assert.AreEqual(5, stat.Value);
        }

        [Test]
        public void Reset_SetsTheValueToTheInitialValue()
        {
            int initialValue = _stat.Value;
            _stat.Value -= 50;

            _stat.Reset();

            Assert.AreEqual(90, _stat.Value);
        }

        [Test]
        public void Value_WhenChanged_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value -= 10;

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(_stat, _statChangedEventArgs.Stat);
            Assert.AreEqual(90, _statChangedEventArgs.OldValue);
            Assert.AreEqual(80, _statChangedEventArgs.NewValue);
        }

        [Test]
        public void Value_WhenThereIsNoChange_DoesNotRaiseAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value = 90;

            Assert.IsFalse(_eventTriggeredFlag);
        }

        [Test]
        public void Value_WhenChangedAndClampedToMaximumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value += 20;

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(_stat, _statChangedEventArgs.Stat);
            Assert.AreEqual(90, _statChangedEventArgs.OldValue);
            Assert.AreEqual(100, _statChangedEventArgs.NewValue);
        }

        [Test]
        public void Value_WhenChangeIsNotAppliedDueToValueAlreadyBeingClampedToMaximumValue_DoesNotRaiseAChangedEvent()
        {
            _stat.Value += 20;
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value += 20;

            Assert.IsFalse(_eventTriggeredFlag);
        }

        [Test]
        public void Stat_WhenChangedAndClampedToMinimumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value -= 250;

            Assert.IsTrue(_eventTriggeredFlag);

            Assert.AreEqual(_stat, _statChangedEventArgs.Stat);
            Assert.AreEqual(90, _statChangedEventArgs.OldValue);
            Assert.AreEqual(0, _statChangedEventArgs.NewValue);
        }

        [Test]
        public void Value_WhenChangeIsNotAppliedDueToValueAlreadyBeingClampedToMinimumValue_DoesNotRaiseAChangedEvent()
        {
            _stat.Value -= 100;
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value -=100;

            Assert.IsFalse(_eventTriggeredFlag);
        }

        private void SetEventTriggeredFlag(object sender, StatChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _statChangedEventArgs = e;
        }
    }
}
