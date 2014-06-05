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
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
        public int ViewportMapOriginX { get; set; }
        public int ViewportMapOriginY { get; set; }

        public LevelSetUpParams()
        {
            PlayerX = -1;
            PlayerY = -1;
            ViewportWidth = -1;
            ViewportHeight = -1;
            ViewportMapOriginX = -1;
            ViewportMapOriginY = -1;
        }

        public bool IsPlayerPositionSet()
        {
            return PlayerX != -1 && PlayerY != -1;
        }

        public bool IsViewportSizeSet()
        {
            return ViewportWidth != -1 && ViewportHeight != -1;
        }

        public bool IsViewportParamsSet()
        {
            return ViewportWidth != -1 && ViewportHeight != -1 && ViewportMapOriginX != -1 && ViewportMapOriginY != -1;
        }
    }
}
