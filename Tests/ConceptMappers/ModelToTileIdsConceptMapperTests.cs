using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;

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
        public void ModelToTileIdsConceptMapper_CreatingAModelToTileIdsMapping_IsSuccessful()
        {
            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(_level);
        }
    }
}
