using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Api;
using Turnable.Components;
using Turnable.Tiled;

namespace Turnable.Locations
{
    public class Level : ILevel
    {
        public IMap Map { get; set; }
        public IPathfinder Pathfinder { get; set; }

        public Level()
        {

        }

        public bool IsCollision(Position position)
        {
            Layer collisionLayer = Map.SpecialLayers[Tiled.Map.SpecialLayer.Collision];

            //// No obstacle layer exists. Currently the only way to mark obstacles is to use a layer in Tiled that has a IsCollision propert with the value "true"
            //if (obstacleLayer == null) return false;

            return (collisionLayer.Tiles.ContainsKey(new Tuple<int, int>(position.X, position.Y)));
        }
    }
}
