using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Skills
{
    [Flags]
    public enum TargetTypes
    {
        Self,
        Enemies,
        Walkable,
        NonWalkable
    }
}
