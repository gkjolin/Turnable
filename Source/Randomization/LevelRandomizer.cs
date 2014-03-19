using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
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

        public TileList BuildRandomTileList(Layer targetLayer, int count)
        {
            TileList returnValue = new TileList();

            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(Level);
            Dictionary<string, List<int>> map = mapper.BuildMapping();
            List<Position> randomSubsetOfWalkablePositions = RandomSelector.Next<Position>(Level.CalculateWalkablePositions(), count);

            foreach (Position position in randomSubsetOfWalkablePositions)
            {
                int randomCharacterTileId = RandomSelector.Next<string, List<int>, int>(map);
                returnValue.Add(new Tuple<int,int>(position.X, position.Y), new Tile((uint)Level.Map.Tilesets["Characters"].FirstGid + (uint)randomCharacterTileId, position.X, position.Y));
            }

            return returnValue;
        }
    }
}
