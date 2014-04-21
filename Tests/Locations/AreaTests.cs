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
using Tests.MoqExtensions;

namespace Tests.Locations
{
    [TestClass]
    public class AreaTests
    {
        private Area _area;
        private Level _level;
        private Level _anotherLevel;
        private World _world;
        private Mock<ILevelFactory> _levelFactoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _area = new Area();
            _world = new World();
            _level = LocationsFactory.BuildLevel("../../Fixtures/HubExample.tmx");
            _level.SetUpCharacters();
            _level.SetUpTransitionPoints();
            _level.SetUpPathfinder();
            _anotherLevel = LocationsFactory.BuildLevel("../../Fixtures/FullExample.tmx");
            _anotherLevel.SetUpCharacters();
            _anotherLevel.SetUpTransitionPoints();
            _anotherLevel.SetUpPathfinder();
            _levelFactoryMock = new Mock<ILevelFactory>();

            _levelFactoryMock.Setup(lf => lf.BuildLevel(It.IsAny<IWorld>(), It.IsAny<LevelInitializationParams>())).ReturnsInOrder(_level, _anotherLevel);
            _area.LevelFactory = _levelFactoryMock.Object;
        }

        [TestMethod]
        public void Area_Construction_IsSuccessful()
        {
            Area area = new Area();

            Assert.IsNotNull(area.Levels);
            Assert.IsNotNull(area.Connections);
            Assert.IsNull(area.CurrentLevel);
            Assert.IsNotNull(area.LevelFactory);
        }

        [TestMethod]
        public void Area_Initializing_BuildsALevelWithTheLevelFactoryAndSetsUpTheConnectionsCorrectly()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/HubExample.tmx";

            _area.Initialize(_world, initializationParams);

            _levelFactoryMock.Verify(lf => lf.BuildLevel(_area.World, initializationParams));

            // Is the newly created level added to the level cache in the Area?
            Assert.AreEqual(_world, _area.World);
            Assert.AreEqual(_level, _area.CurrentLevel);
            Assert.AreEqual(1, _area.Levels.Count);

            // Are the connections between levels set up?
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
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.Initialize(_world, initializationParams);
            ILevel currentLevel = _area.CurrentLevel;

            _area.Enter(_area.Connections[0], initializationParams);

            _levelFactoryMock.Verify(lf => lf.BuildLevel(_area.World, initializationParams));

            Assert.AreNotEqual(currentLevel, _area.CurrentLevel);
            Assert.AreEqual(2, _area.Levels.Count);

            Assert.AreEqual(5, _area.Connections.Count);
            Assert.AreEqual(_area.CurrentLevel, _area.Connections[0].EndNode.Level);

            foreach (Connection connection in _area.Connections)
            {
                Assert.AreEqual(_area.CurrentLevel, connection.StartNode.Level);
                Assert.IsNull(connection.EndNode);
            }
        }
    }
}
