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
            //Position oldMapOrigin = MapOrigin.Copy();
            //MapOrigin = MapOrigin.NeighboringPosition(direction);

            //// The viewport should be able to move in a certain direction as much as possible while still staying within bounds.
            //// Example: The left edge of a Viewport is flush against the left edge of the Map. Trying to move the Viewport NW should still move the Viewport North.

            //// If MapOrigin.X is invalid, reset it
            //if (MapOrigin.X < 0 || (MapOrigin.X + Width) > Level.Map.Width)
            //{
            //    MapOrigin = new Position(oldMapOrigin.X, MapOrigin.Y);
            //}

            //// If MapOrigin.Y is invalid, reset it
            //if (MapOrigin.Y < 0 || (MapOrigin.Y + Height) > Level.Map.Height)
            //{
            //    MapOrigin = new Position(MapOrigin.X, oldMapOrigin.Y);
            //}
        }

        public bool IsMapOriginValid()
        {
            // The MapOrigin is considered valid if the viewport is within the bounds of the Map.
            // Or in other words, Do the Map Bounds contain the Viewport bounds?
            Rectangle mapBounds = ((IBounded)Level.Map).Bounds;

            return (mapBounds.Contains(Bounds));
            //return !(MapOrigin.X < 0 || MapOrigin.Y < 0 || (MapOrigin.X + Width) > (Level.Map.Width - 1) || (MapOrigin.Y + Height) > (Level.Map.Height - 1));
        }

        public void CenterAt(Position center)
        {
            //int x, y;

            //x = center.X - Width / 2;
            //y = center.Y - Height / 2;

            //// When parts of the viewport are out of bounds of the Map, move the new MapOrigin to compensate
            //if ((Width / 2 + center.X) > Level.Map.Width)
            //{
            //    x -= (Width / 2 - (Level.Map.Width - center.X - 1));
            //}
            //if ((Height / 2 + center.Y) > Level.Map.Height)
            //{
            //    y -= (Height / 2 - (Level.Map.Height - center.Y - 1));
            //}
            //if (x < 0)
            //{
            //    x = 0;
            //}
            //if (y < 0)
            //{
            //    y = 0;
            //}

            //MapOrigin = new Position(x, y);
        }
    }
}
