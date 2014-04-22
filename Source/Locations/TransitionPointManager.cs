using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Tmx;

namespace TurnItUp.Locations
{
    public class TransitionPointManager
    {
        public ILevel Level { get; private set; }
        public Position Entrance { get; private set; }
        public List<Position> Exits { get; private set; }

        public TransitionPointManager(ILevel level)
        {
            // Each level can have multiple Exits and one Entrance.
            Level = level;
            Entrance = null;
            Exits = new List<Position>();

            // TODO: Too much work being done in constructor
            ReferenceTile entranceReferenceTile = null;
            ReferenceTile exitReferenceTile = null;

            entranceReferenceTile = level.Map.Tilesets["World"].FindReferenceTileByProperty("IsEntrance", "true");
            exitReferenceTile = level.Map.Tilesets["World"].FindReferenceTileByProperty("IsExit", "true");

            foreach (Tile tile in Level.Map.Layers["Objects"].Tiles.Values)
            {
                if (entranceReferenceTile != null)
                {
                    foreach (Tileset tileset in level.Map.Tilesets)
                    {
                        if ((tile.Gid - tileset.FirstGid) == entranceReferenceTile.Id)
                        {
                            Entrance = new Position(tile.X, tile.Y);
                        }
                    }
                }

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
