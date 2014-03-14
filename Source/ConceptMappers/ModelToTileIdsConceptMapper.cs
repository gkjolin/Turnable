using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.ConceptMappers
{
    public class ModelToTileIdsConceptMapper : IConceptMapper<string, List<int>>
    {
        public Level Level { get; set; }

        public ModelToTileIdsConceptMapper(Level level)
        {
            Level = level;
        }

        public Dictionary<string, List<int>> BuildMapping()
        {
            // The mapping maps model names to 
            throw new NotImplementedException();
        }
    }
}
