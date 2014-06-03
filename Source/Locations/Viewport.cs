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
    public class Viewport : IViewport
    {
        public ILevel Level { get; set; }
        public Position MapOrigin { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Viewport(ILevel level, int width, int height)
        {
            Level = level;
            Width = width;
            Height = height;
        }

        public Viewport(ILevel level, int mapOriginX, int mapOriginY, int width, int height) : this(level, width, height)
        {
            MapOrigin = new Position(mapOriginX, mapOriginY);
        }

        public void Move(Direction direction)
        {
            Position oldMapOrigin = MapOrigin.DeepClone();

            MapOrigin = MapOrigin.InDirection(direction);

            // The code below will allow a MapOrigin to "slide" and move as much as possible. 
            // For example let's say the left edge of a Viewport is flush against the left edge of the Map.
            // Trying to move the Viewport NW should still move the Viewport North
            // If MapOrigin.X is invalid, reset it
            if (MapOrigin.X < 0 || (MapOrigin.X + Width) > (Level.Map.Width - 1))
            {
                MapOrigin.X = oldMapOrigin.X;
            }
            // If MapOrigin.Y is invalid, reset it
            if (MapOrigin.Y < 0 || (MapOrigin.Y + Height) > (Level.Map.Height - 1))
            {
                MapOrigin.Y = oldMapOrigin.Y;
            }
        }

        public bool IsMapOriginValid()
        {
            return !(MapOrigin.X < 0 || MapOrigin.Y < 0 || (MapOrigin.X + Width) > (Level.Map.Width - 1) || (MapOrigin.Y + Height) > (Level.Map.Height - 1));
        }

        public void CenterOn(Position center)
        {
            MapOrigin = new Position(center.X - Width / 2, center.Y - Height / 2);
        }
    }
}
