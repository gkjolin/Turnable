using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Locations;

namespace TurnItUp.Interfaces
{
    public interface ICharacterManager
    {
        List<Entity> Characters { get; set; }
        List<Entity> TurnQueue { get; set; }
        Board Board { get; set; }
        Entity Player { get; set; }

        bool IsCharacterAt(int x, int y);
        Tuple<MoveResult, List<Position>> MovePlayer(Direction direction);
    }
}

