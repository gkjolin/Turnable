using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Stats;

namespace Tests
{
    [TestClass]
    public class StatManagerTests
    {
        private StatManager _statManager;

        [TestInitialize]
        public void Initialize()
        {
            _statManager = new StatManager();
        }

        [TestMethod]
        public void StatManager_Construction_IsSuccessful()
        {
            StatManager statManager = new StatManager();

            Assert.IsNotNull(statManager.Stats);
        }

        [TestMethod]
        public void StatManager_IsAnEntropyComponent()
        {
            Assert.IsInstanceOfType(_statManager, typeof(IComponent));
        }

        [TestMethod]
        public void StatManager_CreatingAStat_IsSuccessful()
        {
            Stat stat = _statManager.CreateStat("Health", 100);

            Assert.AreEqual(1, _statManager.Stats.Count);
            Assert.AreEqual("Health", stat.Name);
            Assert.AreEqual(100, stat.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StatManager_AddingAStatThatAlreadyExists_IsUnsuccessful()
        {
            _statManager.CreateStat("Health", 100);
            _statManager.CreateStat("Health", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StatManager_AddingAStatThatAlreadyExistsDifferingInCaseForName_IsUnsuccessful()
        {
            _statManager.CreateStat("Health", 100);
            _statManager.CreateStat("HEALTH", 100);
        }

        [TestMethod]
        public void StatManager_GettingAStatByName_IsSuccessful()
        {
            _statManager.CreateStat("Health", 100);
            _statManager.CreateStat("Mana", 50);

            Stat stat = _statManager.GetStat("Health");
            Assert.AreEqual("Health", stat.Name);
        }

        [TestMethod]
        public void StatManager_GettingAStatByName_ReturnsNullIfStatDoesNotExist()
        {
            _statManager.CreateStat("Health", 100);
            _statManager.CreateStat("Mana", 50);

            Stat stat = _statManager.GetStat("Adrenaline");
            Assert.IsNull(stat);
        }
    }
}
