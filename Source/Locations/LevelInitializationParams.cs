using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Locations
{
    public class LevelInitializationParams
    {
        public string TmxPath { get; set; }
        public bool AllowDiagonalMovement { get; set; }

        public LevelInitializationParams()
        {
        }
    }
}
