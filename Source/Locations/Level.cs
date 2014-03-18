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

namespace TurnItUp.Locations
{
    public class Level : ILevel
    {
        // Facade pattern

        public Map Map { get; set; }
        public ICharacterManager CharacterManager { get; set; }
        public IPathFinder PathFinder { get; set; }
        public IWorld World { get; set; }

        public Level()
        {
        }

        public Level(IWorld world, string tmxPath, bool allowDiagonalMovement = false)
        {
            Initialize(world, tmxPath, allowDiagonalMovement);
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

        public virtual MoveResult MovePlayer(Direction direction)
        {
            return CharacterManager.MovePlayer(direction);
        }

        public virtual MoveResult MoveCharacterTo(Entity character, Position destination)
        {
            return CharacterManager.MoveCharacterTo(character, destination);
        }

        public void Initialize(IWorld world, string tmxPath, bool allowDiagonalMovement = false)
        {
            World = world;
            Map = new Map(tmxPath);
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");

            if (charactersLayer != null)
            {
                CharacterManager = new CharacterManager(world, this);
            }

            PathFinder = new PathFinder(this, allowDiagonalMovement);
        }

        public List<Tuple<int, int>> CalculateWalkablePositions()
        {
            List<Tuple<int, int>> returnValue = new List<Tuple<int, int>>();

            for (int row = 0; row < Map.Height; row++)
            {
                for (int col = 0; row < Map.Width; col++)
                {
                    if (new Node(this, col, row).IsWalkable())
                    {
                        returnValue.Add(new Tuple<int, int>(row, col));
                    }
                }
            }

            return returnValue;
        }
    }
}
