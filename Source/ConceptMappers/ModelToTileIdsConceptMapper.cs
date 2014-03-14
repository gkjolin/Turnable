using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Tmx;

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
            Dictionary<string, List<int>> returnValue = new Dictionary<string,List<int>>();
            string modelName = null;

            // The mapping maps model names to a list of tile ids that can be used to represent that model
            foreach (ReferenceTile referenceTile in Level.Map.Tilesets["Characters"].ReferenceTiles.Values)
            {
                if (referenceTile.Properties.ContainsKey("Model"))
                {
                    modelName = referenceTile.Properties["Model"];
                    if (!(returnValue.ContainsKey(modelName)))
                    {
                        returnValue[modelName] = new List<int>();
                    }
                    returnValue[modelName].Add(referenceTile.Id);
                }
            }

            return returnValue;
        }
    }
}
