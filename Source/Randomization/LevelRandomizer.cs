using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.ConceptMappers;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Randomization
{
    public class LevelRandomizer : ILevelRandomizer
    {
        public LevelRandomizer()
        {
        }

        public TileList BuildRandomTileList(ILevel level, Layer targetLayer, int count)
        {
            TileList returnValue = new TileList();

            ModelToTileIdsConceptMapper mapper = new ModelToTileIdsConceptMapper(level);
            Dictionary<string, List<uint>> map = mapper.BuildMapping();
            List<Position> randomSubsetOfWalkablePositions = RandomSelector.Next<Position>(level.CalculateWalkablePositions(), count);

            foreach (Position position in randomSubsetOfWalkablePositions)
            {
                uint randomCharacterTileId = RandomSelector.Next<string, List<uint>, uint>(map);
                returnValue.Add(new Tuple<int, int>(position.X, position.Y), new Tile((uint)level.Map.Tilesets["Characters"].FirstGid + (uint)randomCharacterTileId, position.X, position.Y));
            }

            return returnValue;
        }

        public void Randomize(ILevel level, string layerName, int count)
        {
            Randomize(level, layerName, count, count + 1);
        }

        public void Randomize(ILevel level, string layerName, int inclusiveMinimumValue, int exclusiveMaximumValue)
        {
            TileList randomCharactersTileList = BuildRandomTileList(level, level.Map.Layers[layerName], Prng.Next(inclusiveMinimumValue, exclusiveMaximumValue));

            level.Map.Layers["Characters"].Tiles.Merge(randomCharactersTileList);
        }
    }
}
