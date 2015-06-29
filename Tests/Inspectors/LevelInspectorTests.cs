using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Api;
using Tests.Factories;
using Turnable.Inspectors;

namespace Tests.Inspectors
{
    [TestClass]
    public class LevelInspectorTests
    {
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            LevelInspector levelInspector = new LevelInspector(_level);

            Assert.AreEqual(_level, levelInspector.Level);
        }

        
    }
}
