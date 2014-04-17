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
    public class LevelRandomizationParamsTests
    {
        private LevelRandomizationParams _randomizationParams;

        [TestInitialize]
        public void Initialize()
        {
            _randomizationParams = new LevelRandomizationParams();
        }

        [TestMethod]
        public void LevelRandomizationParams_Construction_SetsUpDefaultValuesForIndividualParams()
        {
            LevelRandomizationParams randomizationParams = new LevelRandomizationParams();

            Assert.IsNull(randomizationParams.LayerName);
            Assert.IsNull(randomizationParams.TileCount);
            Assert.IsNull(randomizationParams.TileMaximum);
        }

        [TestMethod]
        public void LevelRandomizationParams_SettingIndividualParams_IsSuccessful()
        {
            _randomizationParams.LayerName = "Characters";
            _randomizationParams.TileCount = 5;
            _randomizationParams.TileMaximum = 10;

            Assert.AreEqual("Characters", _randomizationParams.LayerName);
            Assert.AreEqual(5, _randomizationParams.TileCount);
            Assert.AreEqual(10, _randomizationParams.TileMaximum);
        }
    }
}
