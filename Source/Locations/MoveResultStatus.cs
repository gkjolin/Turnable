using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Locations
{
    public enum MoveResultStatus
    {
        Success,
        HitObstacle,
        HitCharacter,
        OutOfBounds
    }
}
