using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using Entropy;

namespace TurnItUp.Locations
{
    public class Board
    {
        // Facade pattern

        public Map Map { get; private set; }
        public TurnManager TurnManager { get; set; }
        public ICharacterManager CharacterManager { get; set; }

        public void Initialize(World world, string tmxPath)
        {
            Map = new Map(tmxPath);
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");

            if (charactersLayer != null)
            {
                TurnManager = new TurnManager(this);
                CharacterManager = new CharacterManager(world, this);
            }
        }

        public bool IsObstacle(int x, int y)
        {
            Layer obstacleLayer = Map.FindLayerByProperty("IsCollision", "true");

            // No obstacle layer exists. Currently the only way to mark obstacles is to use a layer in Tiled that has a IsCollision propert with the value "true"
            if (obstacleLayer == null) return false;

            return (obstacleLayer.Tiles.ContainsKey(new Tuple<int,int>(x, y)));
        }

        public bool IsCharacterAt(int x, int y)
        {
            return CharacterManager.IsCharacterAt(x, y);
        }

        public Tuple<MoveResult, List<Position>> MovePlayer(Direction direction)
        {
            return CharacterManager.MovePlayer(direction);
        }
    }
}
