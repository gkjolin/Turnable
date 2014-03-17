using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Randomization;
using TurnItUp.Tmx;
using System.Tuples;

namespace Tests.Randomization
{
    [TestClass]
    public class LevelRandomizerTests
    {
        private Level _level;
        private LevelRandomizer _levelRandomizer;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _levelRandomizer = new LevelRandomizer(_level);
        }

        [TestMethod]
        public void LevelRandomizer_Construction_IsSuccessful()
        {
            LevelRandomizer levelRandomizer = new LevelRandomizer(_level);

            Assert.AreEqual(_level, levelRandomizer.Level);
        }

        [TestMethod]
        public void LevelRandomizer_BuildingARandomSetOfCharactersForALevel_ReturnsATileListWithASetOfRandomCharactersInRandomButValidLocations()
        {
            TileList randomCharacters = _levelRandomizer.BuildRandomTileList(_level.Map.Layers["Characters"], 5);

            Assert.AreEqual(5, randomCharacters.Count);

            foreach (Tuple<int, int> position in randomCharacters.Keys)
            {

            }
        }
    }
}
