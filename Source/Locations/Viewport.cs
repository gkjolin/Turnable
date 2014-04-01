using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Randomization;

namespace TurnItUp.Locations
{
    public class Viewport
    {
        public Level Level { get; private set; }
        public Position MapOrigin { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<Position> AnchorPoints { get; private set; }

        public Viewport(Level level, int originX, int originY, int width, int height)
        {
            Level = level;
            MapOrigin = new Position(originX, originY);
            Width = width;
            Height = height;
            AnchorPoints = new List<Position>();
            CalculateAnchorPoints();
        }

        private void CalculateAnchorPoints()
        {
            if (Width % 2 == 1 && Height % 2 == 1)
            {
                AnchorPoints.Add(new Position(Width / 2, Height / 2));
            }

            if (Width % 2 == 0 && Height % 2 == 0)
            {
                AnchorPoints.Add(new Position((Width / 2) - 1, Height / 2));
                AnchorPoints.Add(new Position((Width / 2) - 1, (Height / 2) - 1));
                AnchorPoints.Add(new Position(Width / 2, (Height / 2) - 1));
                AnchorPoints.Add(new Position(Width / 2, Height / 2));
                return;
            }

            if (Width % 2 == 0)
            {
                AnchorPoints.Add(new Position((Width / 2) - 1, Height / 2));
                AnchorPoints.Add(new Position(Width / 2, Height / 2));
            }

            if (Height % 2 == 0)
            {
                AnchorPoints.Add(new Position(Width / 2, (Height / 2) - 1));
                AnchorPoints.Add(new Position(Width / 2, Height / 2));
            }
        }
    }
}
