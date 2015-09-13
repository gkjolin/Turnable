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
            _eventTriggeredFlag = false;
            _statChangedEventArgs = null;
        }

        [Test]
        public void Value_CanBeChanged()
        {
            _stat.Value -= 5;

            Assert.That(_stat.Value, Is.EqualTo(85));
        }

        [Test]
        public void Value_CanBeSetTo0()
        {
            _stat.Value -= 90;

            Assert.That(_stat.Value, Is.EqualTo(0));
        }

        [Test]
        public void Value_IsClampedToAMinimumValueOf0ByDefault()
        {
            _stat.Value -= 100;

            Assert.That(_stat.Value, Is.EqualTo(0));
        }

        [Test]
        public void Value_IsClampedToAMaximumValueOf100ByDefault()
        {
            _stat.Value += 100;

            Assert.That(_stat.Value, Is.EqualTo(100));
        }

        [Test]
        public void Value_CanBeClampedToAValueOtherThanTheDefault()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            
            stat.Value += 100;
            Assert.That(stat.Value, Is.EqualTo(95));

            stat.Value -= 100;
            Assert.That(stat.Value, Is.EqualTo(5));
        }

        [Test]
        public void Reset_SetsTheValueToTheInitialValue()
        {
            int initialValue = _stat.Value;
            _stat.Value -= 50;

            _stat.Reset();

            Assert.That(_stat.Value, Is.EqualTo(90));
        }

        [Test]
        public void Value_WhenChanged_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value -= 10;

            Assert.That(_eventTriggeredFlag, Is.True);
            Assert.That(_statChangedEventArgs.Stat, Is.SameAs(_stat));
            Assert.That(_statChangedEventArgs.OldValue, Is.EqualTo(90));
            Assert.That(_statChangedEventArgs.NewValue, Is.EqualTo(80));
        }

        [Test]
        public void Value_WhenThereIsNoChange_DoesNotRaiseAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value = 90;

            Assert.That(_eventTriggeredFlag, Is.False);
        }

        [Test]
        public void Value_WhenChangedAndClampedToMaximumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value += 20;

            Assert.That(_eventTriggeredFlag, Is.True);
            Assert.That(_statChangedEventArgs.Stat, Is.SameAs(_stat));
            Assert.That(_statChangedEventArgs.OldValue, Is.EqualTo(90));
            Assert.That(_statChangedEventArgs.NewValue, Is.EqualTo(100));
        }

        [Test]
        public void Value_WhenChangeIsNotAppliedDueToValueAlreadyBeingClampedToMaximumValue_DoesNotRaiseAChangedEvent()
        {
            _stat.Value += 20;
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value += 20;

            Assert.That(_eventTriggeredFlag, Is.False);
        }

        [Test]
        public void Stat_WhenChangedAndClampedToMinimumValue_RaisesAChangedEvent()
        {
            _stat.Changed += this.SetEventTriggeredFlag;
            _stat.Value -= 250;

            Assert.IsTrue(_eventTriggeredFlag);

            Assert.That(_statChangedEventArgs.Stat, Is.SameAs(_stat));
            Assert.That(_statChangedEventArgs.OldValue, Is.EqualTo(90));
            Assert.That(_statChangedEventArgs.NewValue, Is.EqualTo(0));
        }

        [Test]
        public void Value_WhenChangeIsNotAppliedDueToValueAlreadyBeingClampedToMinimumValue_DoesNotRaiseAChangedEvent()
        {
            _stat.Value -= 100;
            _stat.Changed += this.SetEventTriggeredFlag;

            _stat.Value -=100;

            Assert.That(_eventTriggeredFlag, Is.False);
        }

        private void SetEventTriggeredFlag(object sender, StatChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _statChangedEventArgs = e;
        }
    }
}
