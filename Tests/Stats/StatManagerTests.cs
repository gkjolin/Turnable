using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Stats;
using Moq;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using TurnItUp.Components;

namespace Tests
{
    [TestClass]
    public class StatManagerTests
    {
        private StatManager _statManager;
        private bool _eventTriggeredFlag;
        private EntityEventArgs _eventArgs;
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            World world = new World();
            _entity = world.CreateEntity();
            _statManager = new StatManager();
            _eventTriggeredFlag = false;
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

        [TestMethod]
        public void StatManager_WhenAStatIsChanged_RaisesAnEvent()
        {
            _entity.AddComponent(_statManager);
            _statManager.CreateStat("Health", 100);
            _statManager.CreateStat("Mana", 50);

            Stat stat = _statManager.GetStat("Health");
             _statManager.StatChanged += SetEventTriggeredFlag;
            stat.Value += 10;

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(stat, ((StatChangedEventArgs)_eventArgs).Stat);
            Assert.AreEqual(_entity, _eventArgs.Entity);
        }

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = (EntityEventArgs)e;
        }

        [TestMethod]
        public void StatManager_WhenAHealthStatIsReducedToZero_AsksForTheCharacterToBeDestroyed()
        {
            // TODO: Test this!

            //Board board = LocationsFactory.BuildBoard();
            //_entity = board.CharacterManager.Characters[0];

            //Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>();
            //board.CharacterManager = characterManagerMock.Object;

            //Stat stat = _entity.GetComponent<StatManager>().GetStat("Health");
            //stat.Value -= 100;

            //characterManagerMock.Verify(cm => cm.DestroyCharacter(_entity));
        }
    }
}
