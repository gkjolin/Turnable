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

            Assert.That(statManager.Stats, Is.Not.Null);
        }

        [Test]
        public void StatManager_IsAnEntropyComponent()
        {
            Assert.That(_statManager, Is.InstanceOf<IComponent>());
        }

        [Test]
        public void BuildStat_BuildsANewStatWithMinimumValue0AndMaximumValue100()
        {
            Stat stat = _statManager.BuildStat("Health", 100);

            Assert.That(_statManager.Stats.Count, Is.EqualTo(1));
            Assert.That(stat.Name, Is.EqualTo("Health"));
            Assert.That(stat.Value, Is.EqualTo(100));
            Assert.That(stat.MinimumValue, Is.EqualTo(0));
            Assert.That(stat.MaximumValue, Is.EqualTo(100));
        }

        [Test]
        public void BuildStat_GivenMinimumAndMaximumValuesForStat_BuildsNewStat()
        {
            Stat stat = _statManager.BuildStat("Hit Chance", 10, 5, 95);
            Assert.That(stat.Value, Is.EqualTo(10));
            Assert.That(stat.MinimumValue, Is.EqualTo(5));
            Assert.That(stat.MaximumValue, Is.EqualTo(95));
        }

        [Test]
        public void BuildStat_BuildingAStatThatAlreadyExists_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            Assert.That(() => _statManager.BuildStat("Health", 100), Throws.ArgumentException);
        }

        [Test]
        public void BuildStat_BuildingAStatThatAlreadyExistsDifferingByCaseForName_ThrowsException()
        {
            _statManager.BuildStat("Health", 100);
            Assert.That(() => _statManager.BuildStat("HEALTH", 100), Throws.ArgumentException);
        }

        [Test]
        public void GetStat_GivenAStatName_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("Health");
            Assert.That(stat.Name, Is.EqualTo("Health"));
        }

        [Test]
        public void GetStat_GivenAStatNameThatDiffersByCase_FindsStat()
        {
            _statManager.BuildStat("Health", 100);
            _statManager.BuildStat("Mana", 50);

            Stat stat = _statManager.GetStat("health");
            Assert.That(stat.Name, Is.EqualTo("Health"));
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

            Assert.That(_eventTriggeredFlag, Is.True);
            Assert.That(_statChangedEventArgs.Stat, Is.SameAs(stat));
            Assert.That(_statChangedEventArgs.OldValue, Is.EqualTo(100));
            Assert.That(_statChangedEventArgs.NewValue, Is.EqualTo(50));
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
