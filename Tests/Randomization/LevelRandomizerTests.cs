using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Randomization;

namespace Tests.Randomization
{
    [TestClass]
    public class LevelRandomizerTests
    {
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void LevelRandomizer_Construction_IsSuccessful()
        {
            LevelRandomizer levelRandomizer = new LevelRandomizer(_level);

            Assert.AreEqual(_level, levelRandomizer.Level);
        }
    }
}
