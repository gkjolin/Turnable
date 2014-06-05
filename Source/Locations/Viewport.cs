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
            int x, y;

            x = center.X - Width / 2;
            y = center.Y - Height / 2;

            if ((Width / 2 + center.X) > Level.Map.Width)
            {
                x -= (Width / 2 - (Level.Map.Width - center.X - 1));
            }
            if ((Height / 2 + center.Y) > Level.Map.Height)
            {
                y -= (Height / 2 - (Level.Map.Height- center.Y - 1));
            }

            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }

            MapOrigin = new Position(x, y);
        }
    }
}
