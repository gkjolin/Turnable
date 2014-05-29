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
using TurnItUp.Randomization;

namespace Tests.Locations
{
    [TestClass]
    public class LevelFactoryTests
    {
        private World _world;
        private ILevelFactory _levelFactory;
        private Mock<ILevelFactory> _levelFactoryMock;
        private Mock<ILevelRandomizer> _levelRandomizerMock;
        private Mock<ITransitionPointManager> _transitionPointManagerMock;
        private Mock<ILevel> _levelMock;
        private Position _entrancePosition;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _levelFactory = new LevelFactory();
            _levelRandomizerMock = new Mock<ILevelRandomizer>();
            _levelFactory.LevelRandomizer = _levelRandomizerMock.Object;
            
            _entrancePosition = new Position(1, 2);

            _transitionPointManagerMock = new Mock<ITransitionPointManager>();
            _transitionPointManagerMock.SetupGet<Position>(tpm => tpm.Entrance).Returns(_entrancePosition);
            _transitionPointManagerMock.CallBase = true;

            _levelMock = new Mock<ILevel>();
            _levelMock.SetupGet<ITransitionPointManager>(l => l.TransitionPointManager).Returns(_transitionPointManagerMock.Object);

            _levelFactoryMock = new Mock<ILevelFactory>();
            _levelFactoryMock.CallBase = true;
        }

        [TestMethod]
        public void LevelFactory_Construction_IsSuccessful()
        {
            LevelFactory levelFactory = new LevelFactory();

            Assert.IsNotNull(levelFactory.LevelRandomizer);
        }

        [TestMethod]
        public void LevelFactory_SettingUpALevelWithDefaultSetUpParams_IsSuccessful()
        {
            LevelSetUpParams setUpParams = new LevelSetUpParams();

            _levelFactory.SetUp(_levelMock.Object, setUpParams);

            // TmxPath NULL - Make sure that the Map is not loaded up and that the characters are not set up
            _levelMock.Verify(l => l.SetUpMap(It.IsAny<string>()),Times.Never());
            _levelMock.Verify(l => l.SetUpCharacters(setUpParams.PlayerModel, setUpParams.PlayerX, setUpParams.PlayerY), Times.Never());
            // Verify that a Pathfinder is set up
            _levelMock.Verify(l => l.SetUpPathfinder(setUpParams.AllowDiagonalMovement));
        }

        [TestMethod]
        public void LevelFactory_SettingUpALevelWithAnEntranceAndUnsetPlayerPosition_UsesThePositionOfTheEntranceForThePlayer()
        {
            LevelSetUpParams setUpParams = new LevelSetUpParams();
            setUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            setUpParams.AllowDiagonalMovement = true;

            _levelFactory.SetUp(_levelMock.Object, setUpParams);

            _levelMock.Verify(l => l.SetUpMap(It.IsAny<string>()));
            _levelMock.Verify(l => l.SetUpCharacters(setUpParams.PlayerModel, _entrancePosition.X, _entrancePosition.Y));
            // Verify that a Pathfinder is set up
            _levelMock.Verify(l => l.SetUpPathfinder(setUpParams.AllowDiagonalMovement));
        }

        [TestMethod]
        public void LevelFactory_SettingUpALevelWithAnInitialTiledMapAndSpecificPlayerPosition_IsSuccessful()
        {
            LevelSetUpParams setUpParams = new LevelSetUpParams();
            setUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            setUpParams.PlayerX = 1;
            setUpParams.PlayerY = 2;
            setUpParams.AllowDiagonalMovement = true;

            _levelFactory.SetUp(_levelMock.Object, setUpParams);

            _levelMock.Verify(l => l.SetUpMap("../../Fixtures/FullExample.tmx"));
            _levelMock.Verify(l => l.SetUpCharacters(setUpParams.PlayerModel, setUpParams.PlayerX, setUpParams.PlayerY));
            // Verify that a Pathfinder is set up
            _levelMock.Verify(l => l.SetUpPathfinder(setUpParams.AllowDiagonalMovement));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LevelFactory_RandomizingALevelWithDefaultRandomizationParams_Fails()
        {
            _levelFactory.Randomize(_levelMock.Object, new LevelRandomizationParams());
        }

        [TestMethod]
        public void LevelFactory_RandomizingALevelWithLayerNameAndTileCount_CallsTheCorrectMethodOnTheLevelRandomizer()
        {
            LevelRandomizationParams randomizationParams = new LevelRandomizationParams();
            randomizationParams.LayerName = "Characters";
            randomizationParams.TileCount = 5;

            _levelFactory.Randomize(_levelMock.Object, randomizationParams);
            _levelRandomizerMock.Verify(lr => lr.Randomize(_levelMock.Object, randomizationParams.LayerName, randomizationParams.TileCount.Value));
        }

        [TestMethod]
        public void LevelFactory_RandomizingALevelWithLayerNameTileCountAndTileMaximum_CallsTheCorrectMethodOnTheLevelRandomizer()
        {
            LevelRandomizationParams randomizationParams = new LevelRandomizationParams();
            randomizationParams.LayerName = "Characters";
            randomizationParams.TileCount = 5;
            randomizationParams.TileMaximum = 10;

            _levelFactory.Randomize(_levelMock.Object, randomizationParams);
            _levelRandomizerMock.Verify(lr => lr.Randomize(_levelMock.Object, randomizationParams.LayerName, randomizationParams.TileCount.Value, randomizationParams.TileMaximum.Value + 1));
        }

        [TestMethod]
        public void LevelFactory_CanBuildABlankLevel()
        {
            ILevel level = _levelFactory.BuildLevel(_world);

            Assert.IsNull(level.Map);
            Assert.IsNull(level.PathFinder);
            Assert.IsNull(level.TransitionPointManager);
            Assert.IsNull(level.Viewport);
            Assert.AreEqual(_world, level.World);
        }

        [TestMethod]
        public void LevelFactory_GivenSetUpParams_CanBuildAProperlySetUpLevel()
        {
            // TODO: Test this
        }

        [TestMethod]
        public void LevelFactory_GivenBothASetUpAndRandomizationParams_CanBuildAProperlySetUpAndRandomizedLevel()
        {
            // TODO: Test this
        }
    }
}
