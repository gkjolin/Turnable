using System;
using NUnit.Framework;
using Turnable.Stats;
using Turnable.Api;

namespace Tests.Stats
{
    [TestFixture]
    public class StatChangedEventArgsTests
    {
        [Test]
        public void Constructor_InitializesAllProperties()
        {
            IStatManager statManager = new StatManager();
            Stat stat = statManager.BuildStat("Health", 100);

            StatChangedEventArgs statChangedEvent = new StatChangedEventArgs(stat, 100, 90);

            Assert.That(stat, statChangedEvent.Stat);
            Assert.That(100, statChangedEvent.OldValue);
            Assert.That(90, statChangedEvent.NewValue);
        }
    }
}
