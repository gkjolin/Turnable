using System;
using NUnit.Framework;
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
    [TestFixture]
    public class LevelSetupParametersTests
    {
        private LevelSetupParameters _levelSetupParameters; 

        [SetUp]
        public void Initialize()
        {
            _levelSetupParameters = new LevelSetupParameters();
        }

        [Test]
        public void DefaultConstructor_InitializesAllProperties()
        {
            LevelSetupParameters levelSetupParameters = new LevelSetupParameters();

            Assert.IsNull(levelSetupParameters.TmxFullFilePath);
        }

        [Test]
        public void LevelSetupParameters_ImplementsTheISetupParametersInterface()
        {
            Assert.IsInstanceOfType(_levelSetupParameters, typeof(ISetupParameters));
        }
    }
}
