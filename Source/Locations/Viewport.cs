using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Pathfinding;
using Turnable.Utilities;

namespace Turnable.Locations
{
    public class Viewport : IViewport, IBounded
    {
        public ILevel Level { get; set; }
        public Rectangle Bounds { get; set; }

        public Position MapLocation
        {
            get
            {
                return Bounds.BottomLeft;
            }
        }

        public Viewport(ILevel level)
        {
            Level = level;
            Bounds = new Rectangle(new Position(0, 0), level.Map.Width, level.Map.Height);
        }

        public Viewport(ILevel level, int width, int height) 
            : this(level)
        {
            Level = level;
            Bounds = new Rectangle(new Position(0, 0), width, height);
        }

        public Viewport(ILevel level, Position mapOrigin, int width, int height)
            : this(level, width, height)
        {
            Bounds = new Rectangle(mapOrigin, width, height);
        }

        public void Move(Direction direction)
        {
            Position oldMapLocation = Bounds.BottomLeft.Copy();
            Bounds.Move(Bounds.BottomLeft.NeighboringPosition(direction));

            // The viewport should be able to move in a certain direction as much as possible while still staying within bounds.
            // Example: The left edge of a Viewport is flush against the left edge of the Map. Trying to move the Viewport NW should still move the Viewport North.

            // If MapLocation.X is invalid, reset it
            if (Bounds.BottomLeft.X < 0 || (Bounds.BottomLeft.X + Bounds.Width) > Level.Map.Width)
            {
                Bounds.Move(new Position(oldMapLocation.X, Bounds.BottomLeft.Y));
            }

            // If MapOrigin.Y is invalid, reset it
            if (Bounds.BottomLeft.Y < 0 || (Bounds.BottomLeft.Y + Bounds.Height) > Level.Map.Height)
            {
                Bounds.Move(new Position(Bounds.BottomLeft.X, oldMapLocation.Y));
            }
        }

        public bool IsMapLocationValid()
        {
            // The MapLocation is considered valid if the viewport is within the bounds of the Map.
            // Or in other words, Do the Map Bounds contain the Viewport bounds?
            Rectangle mapBounds = ((IBounded)Level.Map).Bounds;

            return (mapBounds.Contains(Bounds));
        }

        public void CenterAt(Position center)
        {
            int x, y;

            x = center.X - Bounds.Width / 2;
            y = center.Y - Bounds.Height / 2;

            // When parts of the viewport are out of bounds of the Map, move the new MapOrigin to compensate
            if ((Bounds.Width / 2 + center.X) > Level.Map.Width)
            {
                x -= (Bounds.Width / 2 - (Level.Map.Width - center.X - 1));
            }
            if ((Bounds.Height / 2 + center.Y) > Level.Map.Height)
            {
                y -= (Bounds.Height / 2 - (Level.Map.Height - center.Y - 1));
            }
            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }

            Bounds.Move(new Position(x, y));
        }
    }
}
