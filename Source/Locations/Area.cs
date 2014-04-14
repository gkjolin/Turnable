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
using TurnItUp.Pathfinding;
using TurnItUp.Randomization;

namespace TurnItUp.Locations
{
    public class Area
    {
        public List<Level> Levels { get; private set; }
        public List<Connection> Connections { get; private set; }
        public Level CurrentLevel { get; private set; }
        public World World { get; private set; }

        public Area()
        {
            Levels = new List<Level>();
            Connections = new List<Connection>();
            CurrentLevel = null;
        }

        private void SetupConnections(Level level)
        {
            foreach (Layer layer in level.Map.Layers)
            {
                foreach (Tile tile in layer.Tiles.Values)
                {
                    foreach (Tileset tileset in level.Map.Tilesets)
                    {
                        ReferenceTile entranceReferenceTile = tileset.FindReferenceTileByProperty("IsEntrance", "true");
                        if (entranceReferenceTile != null)
                        {
                            if ((tile.Gid - tileset.FirstGid) == entranceReferenceTile.Id)
                            {
                                Connections.Add(new Connection(new Node(level, tile.X, tile.Y), null));
                            }
                        }
                    }
                }
            }
        }

        public void Initialize(World world, string startingLevelTmxPath)
        {
            World = world;
            CurrentLevel = new Level(world);
            Levels.Add(CurrentLevel);
            SetupConnections(CurrentLevel);
        }

        public void Enter(string tmxPath, Connection connection)
        {
            CurrentLevel = new Level(World);
            Levels.Add(CurrentLevel);
        }
    }
}
