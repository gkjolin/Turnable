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
    public class LevelSetUpParamsTests
    {
        private LevelSetUpParams _levelSetUpParams;

        [TestInitialize]
        public void Initialize()
        {
            _levelSetUpParams = new LevelSetUpParams();
        }

        [TestMethod]
        public void LevelSetUpParams_Construction_SetsUpDefaultValuesForIndividualParams()
        {
            Assert.IsNull(_levelSetUpParams.TmxPath);
            Assert.AreEqual(false, _levelSetUpParams.AllowDiagonalMovement);
            Assert.AreEqual(false, _levelSetUpParams.UseVisionCalculator);
            Assert.IsNull(_levelSetUpParams.PlayerModel);
            Assert.AreEqual(0, _levelSetUpParams.PlayerX);
            Assert.AreEqual(0, _levelSetUpParams.PlayerY);
        }

        [TestMethod]
        public void LevelSetUpParams_SettingIndividualParams_IsSuccessful()
        {
            _levelSetUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelSetUpParams.AllowDiagonalMovement = true;
            _levelSetUpParams.UseVisionCalculator = true;
            _levelSetUpParams.PlayerModel = "Knight M";
            _levelSetUpParams.PlayerX = 1;
            _levelSetUpParams.PlayerY = 2;
            
            Assert.AreEqual("../../Fixtures/FullExample.tmx", _levelSetUpParams.TmxPath);
            Assert.AreEqual(true, _levelSetUpParams.AllowDiagonalMovement);
            Assert.AreEqual(true, _levelSetUpParams.UseVisionCalculator);
            Assert.AreEqual("Knight M", _levelSetUpParams.PlayerModel);
            Assert.AreEqual(1, _levelSetUpParams.PlayerX);
            Assert.AreEqual(2, _levelSetUpParams.PlayerY);
        }
    }
}
