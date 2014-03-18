using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.ConceptMappers;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Randomization
{
    public class LevelRandomizer
    {
        public Level Level { get; set; }

        public LevelRandomizer(Level level)
        {
            Level = level;
        }

        public TileList BuildRandomTileList(Layer targetLayer, int countOfRandomTiles)
        {
            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(Level);
            Dictionary<string, List<int>> map = mapper.BuildMapping();

            for (int i = 0; i < countOfRandomTiles; i++)
            {
            }

            return null;
        }
    }
}
