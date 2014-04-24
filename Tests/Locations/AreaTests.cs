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
        private EventArgs _eventArgs;
        private bool _eventTriggeredFlag;

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
            _eventTriggeredFlag = false;

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
        [ExpectedException(typeof(ArgumentException))]
        public void Area_Initializing_WithoutInitializationParams_Fails()
        {
            _area.Initialize(_world, null);
        }

        // Area - Finding Connections
        [TestMethod]
        public void Area_FindingAConnectionAtAPosition_ReturnsTheConnection()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.Initialize(_world, initializationParams);

            Connection connection = _area.FindConnection(_area.Connections[0].StartNode.Position);

            Assert.AreEqual(connection, _area.Connections[0]);
        }

        [TestMethod]
        public void Area_FindingAConnectionAtAPosition_ReturnsNullIfNoConnectionCanBeFound()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.Initialize(_world, initializationParams);

            Connection connection = _area.FindConnection(new Position(0, 0));

            Assert.IsNull(connection);
        }

        [TestMethod]
        public void Area_EnteringANewConnection_BuildsANewLevelAndCompletesAnIncompleteConnection()
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
            Assert.AreEqual(_level, _area.Connections[0].StartNode.Level);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Area_EnteringANewConnectionWithoutInitializationParams_Fails()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.Initialize(_world, initializationParams);
            ILevel currentLevel = _area.CurrentLevel;

            _area.Enter(_area.Connections[0], null);
        }

        [TestMethod]
        public void Area_EnteringAConnectionWithLevelsOnBothSidesLoaded_DoesNotBuildANewLevel()
        {
            // Test setup
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _area.Initialize(_world, initializationParams);
            _area.Enter(_area.Connections[0], initializationParams);

            //Try entering the level from the EndNode side
            _area.Enter(_area.Connections[0]);
            _levelFactoryMock.Verify(lf => lf.BuildLevel(_area.World, initializationParams), Times.Exactly(2));
            Assert.AreEqual(_level, _area.CurrentLevel);

            // Try entering the connection again, this time from the StartNode side
            _area.Enter(_area.Connections[0]);
            _levelFactoryMock.Verify(lf => lf.BuildLevel(_area.World, initializationParams), Times.Exactly(2));
            Assert.AreEqual(_anotherLevel, _area.CurrentLevel);
        }

        [TestMethod]
        public void Area_WhenInitializingTheFirstLevel_RaisesAfterInitializationEvent()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.AfterInitialization += SetEventTriggeredFlag;
            // TODO: Test this event is triggered AFTER initialization
            _area.Initialize(_world, initializationParams);

            Assert.IsTrue(_eventTriggeredFlag);
        }

        [TestMethod]
        public void Area_EnteringAnUnbuiltLevel_RaisesAfterEnteringEvent()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";

            _area.Initialize(_world, initializationParams);

            _area.AfterEnteringLevel += SetEventTriggeredFlag;
            _area.Enter(_area.Connections[0], initializationParams);
            // TODO: Test this event is triggered AFTER initialization

            Assert.IsTrue(_eventTriggeredFlag);
        }

        [TestMethod]
        public void Area_EnteringABuiltLevel_RaisesAfterEnteringEvent()
        {
            // Test setup
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _area.Initialize(_world, initializationParams);
            _area.Enter(_area.Connections[0], initializationParams);

            _area.AfterEnteringLevel += SetEventTriggeredFlag;
            _area.Enter(_area.Connections[0]);
            // TODO: Test this event is triggered AFTER initialization

            Assert.IsTrue(_eventTriggeredFlag);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Area_TryingToEnterABuiltLevelAndPassingInInitializationParams_Fails()
        {
            // Test setup
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _area.Initialize(_world, initializationParams);
            _area.Enter(_area.Connections[0], initializationParams);
            _area.Enter(_area.Connections[0], initializationParams);
        }

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = e;
        }
    }
}
