using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Interfaces
{
    public interface IBoard
    {
        Map Map { get; set; }
        ICharacterManager CharacterManager { get; set; }
        IPathFinder PathFinder { get; set; }

        // Facade methods
        bool IsObstacle(int x, int y);
        bool IsCharacterAt(int x, int y);
        Tuple<MoveResult, List<Position>> MovePlayer(Direction direction);
        Tuple<MoveResult, List<Position>> MoveCharacterTo(Entity character, Position destination);
    }
}
