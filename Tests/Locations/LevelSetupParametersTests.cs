using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy.Core;
using Turnable.Locations;
using Turnable.Tiled;
using Tests.Factories;
using Turnable.Pathfinding;
using Turnable.Api;
using Turnable.Components;
using System.Linq;
using Moq;

namespace Tests.Locations
{
    [TestClass]
    public class LevelSetupParametersTests
    {
        private LevelSetupParameters _levelSetupParameters; 

        [TestInitialize]
        public void Initialize()
        {
            _levelSetupParameters = new LevelSetupParameters();
        }

        [TestMethod]
        public void DefaultConstructor_InitializesAllProperties()
        {
            LevelSetupParameters levelSetupParameters = new LevelSetupParameters();

            Assert.IsNull(levelSetupParameters.TmxFullFilePath);
        }

        [TestMethod]
        public void LevelSetupParameters_ImplementsTheISetupParametersInterface()
        {
            Assert.IsInstanceOfType(_levelSetupParameters, typeof(ISetupParameters));
        }
    }
}
