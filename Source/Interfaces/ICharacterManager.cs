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
        List<Character> Characters { get; set; }
        Board Board { get; set; }
        Character PlayerCharacter { get; set; }

        bool IsCharacterAt(int x, int y);
        Tuple<MoveResult, List<Position>> MovePlayer(Direction direction);
    }
}

