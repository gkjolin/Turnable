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
        public bool UseVisionCalculator { get; set; }
        public string PlayerModel { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }

        public LevelSetUpParams()
        {
        }
    }
}
