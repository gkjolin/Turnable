using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Components;
using Entropy;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Components
{
    [TestClass]
    public class OnLevelTests
    {
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void OnLevel_IsAnEntropyComponent()
        {
            OnLevel onLevel = new OnLevel(_level);

            Assert.IsInstanceOfType(onLevel, typeof(IComponent));
        }

        [TestMethod]
        public void OnLevel_HasADefaultConstructor()
        {
            OnLevel onLevel = new OnLevel();

            Assert.IsNull(onLevel.Level);
        }

        [TestMethod]
        public void OnLevel_Construction_IsSuccessful()
        {
            OnLevel onLevel = new OnLevel(_level);

            Assert.AreEqual(_level, onLevel.Level);
        }
    }
}
