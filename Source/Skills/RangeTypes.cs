using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Skills
{
    [Flags]
    public enum RangeTypes
    {
        Infinite,
        Adjacent,
        Orthogonal,
        Diagonal,
        Circle
    }
}
