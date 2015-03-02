using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Locations
{
    public enum MovementStatus
    {
        Success,
        HitObstacle,
        HitCharacter,
        OutOfBounds
    }
}