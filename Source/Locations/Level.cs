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
using TurnItUp.Fov;

namespace TurnItUp.Locations
{
    public class Level : ILevel
    {
        // Facade pattern

        public Map Map { get; set; }
        public TransitionPointManager TransitionPointManager { get; set; }
        public ICharacterManager CharacterManager { get; set; }
        public IPathFinder PathFinder { get; set; }
        public IWorld World { get; set; }
        public IViewport Viewport { get; set; }
        public FovCalculator FovCalculator { get; set; }

        public Level()
        {
        }

        public void SetUpPathfinder(bool allowDiagonalMovement = false)
        {
            PathFinder = new PathFinder(this, allowDiagonalMovement);
        }

        public void SetUpCharacters(string playerModel, int playerX, int playerY)
        {
            // TODO: Test that there is a check for CharactersLayer not being null here
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");
            if (charactersLayer != null)
            {
                CharacterManager.SetUpNpcs();
                CharacterManager.SetUpPc(playerModel, playerX, playerY);
            }
        }

        public void SetUpNpcs()
        {
            // TODO: Test that there is a check for CharactersLayer not being null here
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");
            if (charactersLayer != null)
            {
                CharacterManager.SetUpNpcs();
            }
        }

        public void SetUpViewport(int mapOriginX, int mapOriginY, int width, int height)
        {
            Viewport = new Viewport(this, mapOriginX, mapOriginY, width, height);
        }

        public void SetUpTransitionPoints()
        {
            TransitionPointManager = new TransitionPointManager(this);
        }

        public void SetUpMap(string tmxPath)
        {
            Map = new Map(tmxPath);
        }

        public void SetUpFov()
        {
            FovCalculator = new FovCalculator(this);
        }

        public Level(IWorld world)
        {
            World = world;
            CharacterManager = new CharacterManager(this);
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
            Position currentPlayerPosition = CharacterManager.Player.GetComponent<Position>().DeepClone();
            MoveResult returnValue = CharacterManager.MovePlayer(direction);

            // Is there a viewport in this level?
            if (Viewport != null && returnValue.Status == MoveResultStatus.Success)
            {
                if (currentPlayerPosition.X == Viewport.MapOrigin.X + Viewport.Width / 2)
                {
                    int oldViewportMapOriginY = Viewport.MapOrigin.Y;

                    Viewport.Move(direction);
                    Viewport.MapOrigin.Y = oldViewportMapOriginY;
                }

                if (currentPlayerPosition.Y == Viewport.MapOrigin.Y + Viewport.Height / 2)
                {
                    int oldViewportMapOriginX = Viewport.MapOrigin.X;

                    Viewport.Move(direction);
                    Viewport.MapOrigin.X = oldViewportMapOriginX;
                }
            }

            return returnValue;
        }

        public virtual MoveResult MoveCharacterTo(Entity character, Position destination)
        {
            return CharacterManager.MoveCharacterTo(character, destination);
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
    }
}
