using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;

namespace Tests.ConceptMappers
{
    [TestClass]
    public class ModelToTileIdsConceptMapperTests
    {
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
        }

        [TestMethod]
        public void ModelToTileIdsConceptMapper_CreatingAModelToTileIdsMapping_IsSuccessful()
        {
            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper();
        }
    }
}
