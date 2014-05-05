using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Fov;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Interfaces
{
    public interface ILevel
    {
        Map Map { get; set; }
        ICharacterManager CharacterManager { get; set; }
        IPathFinder PathFinder { get; set; }
        TransitionPointManager TransitionPointManager { get; set; }
        IWorld World { get; set; }
        IViewport Viewport { get; set; }
        FovCalculator FovCalculator { get; set; }

        void SetUpCharacters();
        void SetUpCharacters(int playerX, int playerY);
        void SetUpTransitionPoints();
        void SetUpMap(string tmxPath);
        void SetUpPathfinder(bool allowDiagonalMovement = false);
        void SetUpViewport(int mapOriginX, int mapOriginY, int width, int height);
        void SetUpFov();

        List<Position> CalculateWalkablePositions();

        // Facade methods
        bool IsObstacle(int x, int y);
        bool IsCharacterAt(int x, int y);
        MoveResult MovePlayer(Direction direction);
        MoveResult MoveCharacterTo(Entity character, Position destination);
    }
}
