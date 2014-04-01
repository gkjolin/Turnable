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
    public class Level : ILevel
    {
        // Facade pattern

        public Map Map { get; set; }
        public ICharacterManager CharacterManager { get; set; }
        public IPathFinder PathFinder { get; set; }
        public IWorld World { get; set; }
        public Viewport Viewport { get; private set; }

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

        public void Initialize(IWorld world, string tmxPath, bool allowDiagonalMovement = false, bool shouldRandomize = false)
        {
            World = world;
            Map = new Map(tmxPath);

            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");
            if (charactersLayer != null)
            {
                CharacterManager = new CharacterManager(world, this);
            }

            // Randomize
            if (shouldRandomize)
            {
                TileList randomCharactersTileList = new LevelRandomizer(this).BuildRandomTileList(this.Map.Layers["Characters"], Prng.Next(1, 11));

                this.Map.Layers["Characters"].Tiles.Merge(randomCharactersTileList);

                // TODO: This code is not optimal at all. I am creating a CharacterManager to load up pre-defined characters.
                // Then if the level is being randomized, I am re-creating the CharacterManager from scratch once again!
                CharacterManager = new CharacterManager(world, this);
            }

            PathFinder = new PathFinder(this, allowDiagonalMovement);
        }

        public List<Position> CalculateWalkablePositions()
        {
            List<Position> returnValue = new List<Position>();

            for (int row = 0; row < Map.Height; row++)
            {
                for (int col = 0; col < Map.Width; col++)
                {
                    if (new Node(this, col, row).IsWalkable())
                    {
                        returnValue.Add(new Position(col, row));
                    }
                }
            }

            return returnValue;
        }

        public void SetupViewport(int mapOriginX, int mapOriginY, int width, int height)
        {
            Viewport = new Viewport(this, mapOriginX, mapOriginY, width, height);
        }
    }
}
