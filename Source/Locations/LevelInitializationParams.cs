using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Locations
{
    public class LevelSetUpParams
    {
        public string TmxPath { get; set; }
        public bool AllowDiagonalMovement { get; set; }
        public bool UseFov { get; set; }

        public LevelSetUpParams()
        {
        }
    }
}
