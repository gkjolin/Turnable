using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Moq;
using TurnItUp.Components;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Tmx;
using System.Tuples;
using Tests.Factories;

namespace Tests.Locations
{
    [TestClass]
    public class AreaTests
    {
        private Area _area;
        private Level _level;
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _area = new Area();
            _world = new World();
            _level = LocationsFactory.BuildLevel("../../Fixtures/HubExample.tmx");
        }

        [TestMethod]
        public void Area_Construction_IsSuccessful()
        {
            Area area = new Area();

            Assert.IsNotNull(area.Levels);
            Assert.IsNotNull(area.Connections);
            Assert.IsNull(area.CurrentLevel);
        }

        [TestMethod]
        public void Area_Initializing_InitializesTheCurrentLevelAndSetsUpTheConnectionsCorrectly()
        {
            _area.Initialize(_world, "../../Fixtures/HubExample.tmx");

            Assert.IsNotNull(_area.CurrentLevel);
            Assert.AreEqual(1, _area.Levels.Count);
            Assert.AreEqual(_world, _area.World);

            Assert.AreEqual(4, _area.Connections.Count);

            foreach (Connection connection in _area.Connections)
            {
                Assert.AreEqual(_area.CurrentLevel, connection.StartNode.Level);
                Assert.IsNull(connection.EndNode);
            }
        }

        [TestMethod]
        public void Area_EnteringANewLevelViaAConnection_InitializesTheNewLevelAndCompletesAnIncompleteConnection()
        {
            _area.Initialize(_world, "../../Fixtures/HubExample.tmx");
            Level currentLevel = _area.CurrentLevel;

            _area.Enter("../../Fixtures/HubExample.tmx", _area.Connections[0]);

            Assert.AreNotEqual(currentLevel, _area.CurrentLevel);
            Assert.AreEqual(2, _area.Levels.Count);

            Assert.AreEqual(4, _area.Connections.Count);
            Assert.AreEqual(_area.CurrentLevel, _area.Connections[0].EndNode.Level);

            foreach (Connection connection in _area.Connections)
            {
                Assert.AreEqual(_area.CurrentLevel, connection.StartNode.Level);
                Assert.IsNull(connection.EndNode);
            }
        }
    }
}
