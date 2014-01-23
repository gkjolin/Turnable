using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;

namespace TurnItUp.Locations
{
    public class Board
    {
        public Map Map { get; private set; }
        public TurnManager TurnManager { get; set; }

        public Board(string tmxPath)
        {
            Map = new Map(tmxPath);
            // TODO: Write a function that retrieves a layer with a certain property/properties and use here and in IsObstacle
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");

            if (charactersLayer != null)
            {
                TurnManager = new TurnManager(Map.Tilesets["Characters"], Map.Layers["Characters"]);
            }
        }

        public bool IsObstacle(int x, int y, int layer)
        {
            Layer obstacleLayer = Map.FindLayerByProperty("IsCollision", "true");

            // No obstacle layer exists. Currently the only way to mark obstacles is to use a layer in Tiled that has a IsCollision propert with the value "true"
            if (obstacleLayer == null) return false;

            return (obstacleLayer.Tiles.ContainsKey(new Tuple<int,int>(x, y)));
        }
    }
}
