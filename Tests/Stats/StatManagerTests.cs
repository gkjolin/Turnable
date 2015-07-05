using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Entropy;
using Turnable.Stats;
using Moq;
using Tests.Factories;
using Entropy.Core;
using Turnable.Api;

namespace Tests
{
    [TestClass]
    public class StatManagerTests
    {
        private IStatManager _statManager;
        private bool _eventTriggeredFlag;
        private StatChangedEventArgs _statChangedEventArgs;

        [TestInitialize]
        public void Initialize()
        {
            _statManager = new StatManager();
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
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
        public void BuildStat_BuildsANewStatWithMinimumValue0AndMaximumValue100()
        {
            Stat stat = _statManager.BuildStat("Health", 100);

            Assert.AreEqual(1, _statManager.Stats.Count);
            Assert.AreEqual("Health", stat.Name);
            Assert.AreEqual(100, stat.Value);
            Assert.AreEqual(0, stat.MinimumValue);
            Assert.AreEqual(100, stat.MaximumValue);
        }

        [TestMethod]
        public void BuildStat_GivenMinimumAndMaximumValuesForStat_BuildsNewStat()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            Assert.AreEqual(10, stat.Value);
            Assert.AreEqual(5, stat.MinimumValue);
            Assert.AreEqual(95, stat.MaximumValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildStat_BuildingAStatThatAlreadyExists_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Health", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildStat_BuildingAStatThatAlreadyExistsDifferingByCaseForName_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("HEALTH", 100);
        }

        [TestMethod]
        public void GetStat_GivenAStatName_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("Health");
            Assert.AreEqual("Health", stat.Name);
        }

        [TestMethod]
        public void GetStat_GivenAStatNameThatDiffersByCase_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("health");
            Assert.AreEqual("Health", stat.Name);
        }

        [TestMethod]
        public void GetStat_GivenANameThatDoesNotExist_ReturnsNull()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("Adrenaline");
            Assert.IsNull(stat);
        }

        [TestMethod]
        public void StatManager_WhenAnyStatIsChanged_RaisesAStatChangedEvent()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 100);

            Stat stat = _statManager.GetStat("Health");
            _statManager.StatChanged += SetEventTriggeredFlag;
            stat.Value = 50;

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(stat, _statChangedEventArgs.Stat);
            Assert.AreEqual(100, _statChangedEventArgs.OldValue);
            Assert.AreEqual(50, _statChangedEventArgs.NewValue);
        }

        private void SetEventTriggeredFlag(object sender, StatChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _statChangedEventArgs = e;
        }

        //[TestMethod]
        //public void StatManager_WhenAHealthStatIsReducedToZero_AsksForTheCharacterToBeDestroyed()
        //{
        //    ILevel level = LocationsFactory.BuildLevel();
        //    _entity = level.CharacterManager.Characters[0];

        //    Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>();
        //    level.CharacterManager = characterManagerMock.Object;

        //    Stat stat = _entity.GetComponent<StatManager>().GetStat("Health");
        //    stat.Value -= 100;

        //    characterManagerMock.Verify(cm => cm.DestroyCharacter(_entity));
        //}
    }
}
