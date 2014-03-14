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
            _level = LocationsFactory.BuildLevel();
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

            Assert.AreEqual(378, mapping.Count);
        }
    }
}
