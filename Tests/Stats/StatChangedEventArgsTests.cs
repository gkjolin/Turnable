using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Stats;
using Turnable.Api;

namespace Tests.Stats
{
    [TestClass]
    public class StatChangedEventArgsTests
    {
        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            IStatManager statManager = new StatManager();
            Stat stat = statManager.BuildStat("Health", 100);

            StatChangedEventArgs statChangedEvent = new StatChangedEventArgs(stat, 100, 90);

            Assert.AreEqual(stat, statChangedEvent.Stat);
            Assert.AreEqual(100, statChangedEvent.OldValue);
            Assert.AreEqual(90, statChangedEvent.NewValue);
        }
    }
}
