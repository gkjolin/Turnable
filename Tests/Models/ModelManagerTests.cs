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

            Assert.That(modelManager.Level, Is.SameAs(_level));
            Assert.That(modelManager.Models, Is.Not.Null);
            Assert.That(modelManager.Models, Is.InstanceOf<IDictionary<string, SpecialTile>>());
            Assert.That(modelManager.Models.Count, Is.EqualTo(3));
            // Is the right SpecialTile associated with the model name?
            SpecialTile specialTile = modelManager.Models["Knight M"];
            Assert.That(specialTile.Id, Is.EqualTo((uint)0));
            Assert.That(specialTile.Properties["Model"], Is.EqualTo("Knight M"));
        }
    }
}
