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
    public class LevelInitializationParamsTests
    {
        private LevelSetUpParams _levelInitializationParams;

        [TestInitialize]
        public void Initialize()
        {
            _levelInitializationParams = new LevelSetUpParams();
        }

        [TestMethod]
        public void LevelInitializationParams_Construction_SetsUpDefaultValuesForIndividualParams()
        {
            Assert.AreEqual(null, _levelInitializationParams.TmxPath);
            Assert.AreEqual(false, _levelInitializationParams.AllowDiagonalMovement);
        }

        [TestMethod]
        public void LevelInitializationParams_SettingIndividualParams_IsSuccessful()
        {
            _levelInitializationParams.TmxPath = "../../Fixtures/FullExample.tmx";
            _levelInitializationParams.AllowDiagonalMovement = true;
            
            Assert.AreEqual("../../Fixtures/FullExample.tmx", _levelInitializationParams.TmxPath);
            Assert.AreEqual(true, _levelInitializationParams.AllowDiagonalMovement);
        }
    }
}
