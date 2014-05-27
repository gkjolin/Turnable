using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Interfaces
{
    public interface ITransitionPointManager
    {
        ILevel Level { get; set; }
        Position Entrance { get; set; }
        List<Position> Exits { get; set; }
    }
}
