using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Pathfinding;

namespace Turnable.Locations
{
    public class Viewport : IViewport
    {
        public ILevel Level { get; set; }
        public Position MapOrigin { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Viewport(ILevel level)
        {
            Level = level;
            Width = level.Map.Width;
            Height = level.Map.Height;
            MapOrigin = new Position(0, 0);
        }

        public Viewport(ILevel level, int width, int height) 
            : this(level)
        {
            Level = level;
            Width = width;
            Height = height;
        }

        public Viewport(ILevel level, int mapOriginX, int mapOriginY, int width, int height)
            : this(level, width, height)
        {
            MapOrigin = new Position(mapOriginX, mapOriginY);
        }

        public void Move(Direction direction)
        {
            Position oldMapOrigin = MapOrigin.Copy();
            MapOrigin = MapOrigin.NeighboringPosition(direction);

            // The viewport should be able to move in a certain direction as much as possible while still staying within bounds.
            // Example: The left edge of a Viewport is flush against the left edge of the Map. Trying to move the Viewport NW should still move the Viewport North.

            // If MapOrigin.X is invalid, reset it
            if (MapOrigin.X < 0 || (MapOrigin.X + Width) > Level.Map.Width)
            {
                MapOrigin = new Position(oldMapOrigin.X, MapOrigin.Y);
            }

            // If MapOrigin.Y is invalid, reset it
            if (MapOrigin.Y < 0 || (MapOrigin.Y + Height) > Level.Map.Height)
            {
                MapOrigin = new Position(MapOrigin.X, oldMapOrigin.Y);
            }
        }

        public bool IsMapOriginValid()
        {
            // The MapOrigin should be located so that the entire Viewport will fit within the current Map. Otherwise the MapOrigin is considered to be invalid.
            return !(MapOrigin.X < 0 || MapOrigin.Y < 0 || (MapOrigin.X + Width) > (Level.Map.Width - 1) || (MapOrigin.Y + Height) > (Level.Map.Height - 1));
        }

        public void CenterAt(Position center)
        {
            int x, y;

            x = center.X - Width / 2;
            y = center.Y - Height / 2;

            // When parts of the viewport are out of bounds of the Map, move the new MapOrigin to compensate
            if ((Width / 2 + center.X) > Level.Map.Width)
            {
                x -= (Width / 2 - (Level.Map.Width - center.X - 1));
            }
            if ((Height / 2 + center.Y) > Level.Map.Height)
            {
                y -= (Height / 2 - (Level.Map.Height - center.Y - 1));
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
