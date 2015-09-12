using System;
using NUnit.Framework;
using Tests.Factories;
using Turnable.Api;
using Turnable.Models;
using System.Collections.Generic;
using Turnable.Tiled;

namespace Tests.Models
{
    [TestFixture]
    public class ModelManagerTests
    {
        private ILevel _level;

        [SetUp]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            IModelManager modelManager = new ModelManager(_level);

            Assert.AreEqual(_level, modelManager.Level);
            Assert.IsNotNull(modelManager.Models);
            Assert.IsInstanceOfType(modelManager.Models, typeof(IDictionary<string, SpecialTile>));
            Assert.AreEqual(3, modelManager.Models.Count);
            // Is the right SpecialTile associated with the model name?
            SpecialTile specialTile = modelManager.Models["Knight M"];
            Assert.AreEqual((uint)0, specialTile.Id);
            Assert.AreEqual("Knight M", specialTile.Properties["Model"]);
        }
    }
}
