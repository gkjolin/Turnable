using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;
using System.Collections.Generic;
using TurnItUp.ConceptMappers;

namespace Tests.ConceptMappers
{
    [TestClass]
    public class ModelToTileIdsConceptMapperTests
    {
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel("../../Fixtures/FullExampleWithExternalTilesetReference.tmx");
        }

        [TestMethod]
        public void ModelToTileIdsConceptMapper_Construction_IsSuccessful()
        {
            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(_level);

            Assert.AreEqual(_level, mapper.Level);
        }

        [TestMethod]
        public void ModelToTileIdsConceptMapper_CreatingAModelToTileIdsMapping_IsSuccessful()
        {
            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(_level);

            Dictionary<string, List<int>> mapping = mapper.BuildMapping();

            Assert.AreEqual(156, mapping.Count);

            // Some models have a whole list of tiles that can represent them. Check to see if a couple of these have been mapped correctly.
            Assert.AreEqual(3, mapping["Bandit"].Count);
            Assert.IsTrue(mapping["Bandit"].Contains(36));
            Assert.IsTrue(mapping["Bandit"].Contains(72));
            Assert.IsTrue(mapping["Bandit"].Contains(109));
        }
    }
}
