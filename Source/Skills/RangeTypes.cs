using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Skills
{
    [Flags]
    public enum RangeTypes
    {
        Any = 0,
        Adjacent = 1,
        Orthogonal = 2,
        Diagonal = 4, 
        Circle = 8
    }
}
