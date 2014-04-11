using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Tmx;

namespace TurnItUp.Locations
{
    public class TransitionPointManager
    {
        public Level Level { get; private set; }
        public Position Entrance { get; private set; }
        public List<Position> Exits { get; private set; }

        public TransitionPointManager(Level level)
        {
            // Each level can have multiple Exits and one Entrance.
            Level = level;
            Entrance = null;
            Exits = new List<Position>();
            ReferenceTile entranceReferenceTile = null;
            ReferenceTile exitReferenceTile = null;

            entranceReferenceTile = level.Map.Tilesets["World"].FindReferenceTileByProperty("IsEntrance", "true");
            exitReferenceTile = level.Map.Tilesets["World"].FindReferenceTileByProperty("IsExit", "true");

            foreach (Tile tile in Level.Map.Layers["Objects"].Tiles.Values)
            {
                if (exitReferenceTile != null)
                {
                    foreach (Tileset tileset in level.Map.Tilesets)
                    {
                        if ((tile.Gid - tileset.FirstGid) == exitReferenceTile.Id)
                        {
                            Exits.Add(new Position(tile.X, tile.Y));
                        }
                    }
                }
            }
        }
    }
}
