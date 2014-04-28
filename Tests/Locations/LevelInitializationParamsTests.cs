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
            Assert.AreEqual(null, _levelSetUpParams.TmxPath);
            Assert.AreEqual(false, _levelSetUpParams.AllowDiagonalMovement);
            Assert.AreEqual(false, _levelSetUpParams.UseFov);
        }

        [TestMethod]
        public void LevelSetUpParams_SettingIndividualParams_IsSuccessful()
        {
            _levelSetUpParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelSetUpParams.AllowDiagonalMovement = true;
            _levelSetUpParams.UseFov = true;
            
            Assert.AreEqual("../../Fixtures/FullExample.tmx", _levelSetUpParams.TmxPath);
            Assert.AreEqual(true, _levelSetUpParams.AllowDiagonalMovement);
            Assert.AreEqual(true, _levelSetUpParams.UseFov);
        }
    }
}
