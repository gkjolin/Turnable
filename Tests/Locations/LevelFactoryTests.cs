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
        private ILevelFactory _levelFactory;
        private Mock<ILevel> _levelMock;

        [TestInitialize]
        public void Initialize()
        {
            _levelFactory = new LevelFactory();
            _levelMock = new Mock<ILevel>();
        }

        [TestMethod]
        public void LevelFactory_Construction_IsSuccessful()
        {
            LevelFactory levelFactory = new LevelFactory();

            Assert.IsNotNull(levelFactory.LevelRandomizer);
        }

        [TestMethod]
        public void LevelFactory_InitializingALevelWithDefaultInitializationParams_IsSuccessful()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();

            _levelFactory.Initialize(_levelMock.Object, initializationParams);

            // TmxPath NULL - Make sure that the Map is not loaded up and that the characters are not set up
            _levelMock.Verify(l => l.SetUpMap(It.IsAny<string>()),Times.Never());
            _levelMock.Verify(l => l.SetUpCharacters(), Times.Never());
            // Verify that a Pathfinder is set up
            _levelMock.Verify(l => l.SetUpPathfinder(initializationParams.AllowDiagonalMovement));
        }

        [TestMethod]
        public void LevelFactory_InitializingALevelWithAnInitialTiledMap_IsSuccessful()
        {
            LevelInitializationParams initializationParams = new LevelInitializationParams();
            initializationParams.TmxPath = "../../Fixtures/FullExample.tmx";
            initializationParams.AllowDiagonalMovement = true;

            _levelFactory.Initialize(_levelMock.Object, initializationParams);

            // TmxPath NULL - Make sure that the Map is not loaded up and that the characters are not set up
            _levelMock.Verify(l => l.SetUpMap("../../Fixtures/FullExample.tmx"));
            _levelMock.Verify(l => l.SetUpCharacters());
            // Verify that a Pathfinder is set up
            _levelMock.Verify(l => l.SetUpPathfinder(initializationParams.AllowDiagonalMovement));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LevelFactory_RandomizingALevelWithDefaultRandomizationParams_Fails()
        {
            _levelFactory.Randomize(_levelMock.Object, new LevelRandomizationParams());
        }
    }
}
