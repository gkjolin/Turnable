using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Locations
{
    // TODO: Test this class
    public class MoveResult
    {
        public MoveResultStatus Status { get; set; }
        public List<Position> Path { get; set; }
    }
}
