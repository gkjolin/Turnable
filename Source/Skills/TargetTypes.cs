using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Skills
{
    [Flags]
    public enum TargetTypes
    {
        // Skills can target Any, Self, NotSelf, Walkable, NonWalkable
        // Skills can have a range of Any, Adjacent, Orthogonal, Diagonal, Circle

        Any = 0,
        Self = 1,
        NotSelf = 2,
        Walkable = 4,
        NonWalkable = 8
    }
}
