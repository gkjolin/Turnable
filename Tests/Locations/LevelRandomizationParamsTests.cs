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
        private LevelRandomizationParams _levelRandomizationParams;

        [TestInitialize]
        public void Initialize()
        {
            _levelRandomizationParams = new LevelRandomizationParams();
        }

        [TestMethod]
        public void LevelRandomizationParams_Construction_SetsUpDefaultValuesForIndividualParams()
        {
            LevelRandomizationParams randomizationParams = new LevelRandomizationParams();

            Assert.IsNull(randomizationParams.LayerName);
            Assert.AreEqual(0, randomizationParams.TileCount);
            Assert.AreEqual(0, randomizationParams.TileMaximum);
        }

        [TestMethod]
        public void LevelRandomizationParams_SettingIndividualParams_IsSuccessful()
        {
            //_levelRandomizationParams.LayerName = "Characters";
            //_levelRandomizationParams.Count = 5;
            //_levelRandomizationParams.Maximum = 10;

            //Assert.AreEqual("Characters", _levelRandomizationParams.LayerName);
        }
    }
}
