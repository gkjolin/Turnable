using System;
using NUnit.Framework;
using System.Collections.Generic;
using Entropy;
using Turnable.Stats;
using Moq;
using Tests.Factories;
using Entropy.Core;
using Turnable.Api;

namespace Tests
{
    [TestFixture]
    public class StatManagerTests
    {
        private IStatManager _statManager;
        private bool _eventTriggeredFlag;
        private StatChangedEventArgs _statChangedEventArgs;

        [SetUp]
        public void Initialize()
        {
            _statManager = new StatManager();
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            StatManager statManager = new StatManager();

            Assert.IsNotNull(statManager.Stats);
        }

        [Test]
        public void StatManager_IsAnEntropyComponent()
        {
            Assert.IsInstanceOfType(_statManager, typeof(IComponent));
        }

        [Test]
        public void BuildStat_BuildsANewStatWithMinimumValue0AndMaximumValue100()
        {
            Stat stat = _statManager.BuildStat("Health", 100);

            Assert.That(1, _statManager.Stats.Count);
            Assert.That("Health", stat.Name);
            Assert.That(100, stat.Value);
            Assert.That(0, stat.MinimumValue);
            Assert.That(100, stat.MaximumValue);
        }

        [Test]
        public void BuildStat_GivenMinimumAndMaximumValuesForStat_BuildsNewStat()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            Assert.That(10, stat.Value);
            Assert.That(5, stat.MinimumValue);
            Assert.That(95, stat.MaximumValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildStat_BuildingAStatThatAlreadyExists_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Health", 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildStat_BuildingAStatThatAlreadyExistsDifferingByCaseForName_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("HEALTH", 100);
        }

        [Test]
        public void GetStat_GivenAStatName_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("Health");
            Assert.That("Health", stat.Name);
        }

        [Test]
        public void GetStat_GivenAStatNameThatDiffersByCase_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("health");
            Assert.That("Health", stat.Name);
        }

        [Test]
        public void GetStat_GivenANameThatDoesNotExist_ReturnsNull()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("Adrenaline");
            Assert.IsNull(stat);
        }

        [Test]
        public void StatManager_WhenAnyStatIsChanged_RaisesAStatChangedEvent()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 100);

            Stat stat = _statManager.GetStat("Health");
            _statManager.StatChanged += SetEventTriggeredFlag;
            stat.Value = 50;

            Assert.That(_eventTriggeredFlag);
            Assert.That(stat, _statChangedEventArgs.Stat);
            Assert.That(100, _statChangedEventArgs.OldValue);
            Assert.That(50, _statChangedEventArgs.NewValue);
        }

        private void SetEventTriggeredFlag(object sender, StatChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _statChangedEventArgs = e;
        }

        //[Test]
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
