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
            Assert.AreEqual(-1, _levelSetUpParams.PlayerX);
            Assert.AreEqual(-1, _levelSetUpParams.PlayerY);
            Assert.AreEqual(-1, _levelSetUpParams.ViewportMapOriginX);
            Assert.AreEqual(-1, _levelSetUpParams.ViewportMapOriginY);
            Assert.AreEqual(-1, _levelSetUpParams.ViewportWidth);
            Assert.AreEqual(-1, _levelSetUpParams.ViewportHeight);
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

        [TestMethod]
        public void LevelSetUpParams_CanDetermineIfPlayerPositionIsSet()
        {
            _levelSetUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelSetUpParams.PlayerX = -1;
            _levelSetUpParams.PlayerY = -1;

            Assert.IsFalse(_levelSetUpParams.IsPlayerPositionSet());

            _levelSetUpParams.PlayerX = 0;
            _levelSetUpParams.PlayerY = 0;
            Assert.IsTrue(_levelSetUpParams.IsPlayerPositionSet());
        }

        [TestMethod]
        public void LevelSetUpParams_CanDetermineIfViewportSizeIsSet()
        {
            _levelSetUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelSetUpParams.ViewportWidth = -1;
            _levelSetUpParams.ViewportHeight = -1;

            Assert.IsFalse(_levelSetUpParams.IsViewportSizeSet());

            _levelSetUpParams.ViewportWidth = 5;
            _levelSetUpParams.ViewportHeight = 5;
            Assert.IsTrue(_levelSetUpParams.IsViewportSizeSet());
        }

        [TestMethod]
        public void LevelSetUpParams_CanDetermineIfAllViewportParamsAreSet()
        {
            _levelSetUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelSetUpParams.ViewportWidth = -1;
            _levelSetUpParams.ViewportHeight = -1;
            _levelSetUpParams.ViewportMapOriginX = -1;
            _levelSetUpParams.ViewportMapOriginY = -1;

            Assert.IsFalse(_levelSetUpParams.IsViewportParamsSet());

            _levelSetUpParams.ViewportWidth = 5;
            _levelSetUpParams.ViewportHeight = 5;
            _levelSetUpParams.ViewportMapOriginX = 0;
            _levelSetUpParams.ViewportMapOriginY = 0;
            Assert.IsTrue(_levelSetUpParams.IsViewportParamsSet());
        }
    }
}
