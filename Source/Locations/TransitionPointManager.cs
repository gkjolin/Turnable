using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Tmx;

namespace TurnItUp.Locations
{
    public class TransitionPointManager : ITransitionPointManager
    {
        public ILevel Level { get; set; }
        public Position Entrance { get; set; }
        public List<Position> Exits { get; set; }

        public TransitionPointManager(ILevel level)
        {
            // TODO: Unit test the next line
            if (!TransitionPointManager.DoesLevelMeetRequirements(level)) 
            {
                return;
            }

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

        public static bool DoesLevelMeetRequirements(ILevel level)
        {
            return (level.Map.Tilesets.Contains("World") && level.Map.Layers.Contains("Objects"));
        }
    }
}
