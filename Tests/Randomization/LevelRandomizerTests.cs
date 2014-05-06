using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Randomization;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Interfaces;

namespace Tests.Randomization
{
    [TestClass]
    public class LevelRandomizerTests
    {
        private ILevel _level;
        private LevelRandomizer _levelRandomizer;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _levelRandomizer = new LevelRandomizer();
        }

        [TestMethod]
        public void LevelRandomizer_BuildingARandomSetOfCharactersForALevel_ReturnsATileListWithASetOfRandomCharactersInRandomButValidLocations()
        {
            TileList randomCharacters = _levelRandomizer.BuildRandomTileList(_level, _level.Map.Layers["Characters"], 5);

            Assert.AreEqual(5, randomCharacters.Count);

            foreach (Tuple<int, int> position in randomCharacters.Keys)
            {
                Tile tile = randomCharacters[position];

                Assert.AreEqual(position.Element1, tile.X);
                Assert.AreEqual(position.Element2, tile.Y);

                Assert.IsTrue(tile.Gid >= _level.Map.Tilesets["Characters"].FirstGid);
            }
        }

        [TestMethod]
        public void LevelRandomizer_RandomizingTheCharactersWithAFixedNumberOfNewRandomCharacters_MergesARandomizedSetOfTilesWithTheCharactersInALevel()
        {
            _levelRandomizer.Randomize(_level, "Characters", 5);

            Assert.AreEqual(14, _level.Map.Layers["Characters"].Tiles.Count);
        }

        [TestMethod]
        public void LevelRandomizer_RandomizingTheCharactersWithARandomNumberOfNewRandomCharacters_MergesARandomizedSetOfTilesWithTheCharactersInALevel()
        {
            _levelRandomizer.Randomize(_level, "Characters", 1, 11);

            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.Count > 9);
            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.Count <= 19);
        }
    }
}
