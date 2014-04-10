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
            Level = level;
            Entrance = null;
            Exits = new List<Position>();

            foreach (Tile tile in Level.Map.Layers["Objects"].Tiles.Values)
            {
                foreach (Tileset tileset in level.Map.Tilesets)
                {
                    ReferenceTile exitReferenceTile = tileset.FindReferenceTileByProperty("IsExit", "true");
                    if (exitReferenceTile != null)
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
