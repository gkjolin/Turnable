using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;

namespace TurnItUp.Locations
{
    public class Board
    {
        public Map Map { get; private set; }

        public Board(string tmxPath)
        {
            Map = new Map(tmxPath);
        }

        public bool IsObstacle(int x, int y, int layer)
        {
            return (
                // Is there an obstacle in the layer above?
                ((layer + 1) <= Map.Layers.Count) && IsObstacle(x, y, layer + 1) ||               
                (// Is Layer present?
                    (Map.Layers.Count - 1) >= layer && 
                    // Does the layer have any properties?
                    Map.Layers[layer].Properties != null &&
                    // Is there a property for this layer called IsCollision?
                    Map.Layers[layer].Properties.ContainsKey("IsCollision") &&
                    // If so is the value true (this is a Collision layer)
                    Map.Layers[layer].Properties["IsCollision"] == "true" && 
                    // If any tile is present at coordinates x, y it is an obstacle
                    Map.Layers[layer].Tiles.ContainsKey(new Tuple<int,int>(x, y)))
                );
        }
    }
}
