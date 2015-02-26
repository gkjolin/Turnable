using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.Vision
{
    public class Line
    {
        // http://trystans.blogspot.com/2011/09/roguelike-tutorial-08-vision-line-of.html
        public List<Position> Points { get; set; }

        public Line(Position start, Position end)
        {
            Points = new List<Position>();

            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;
            int x0 = start.X;
            int x1 = end.X;
            int y0 = start.Y;
            int y1 = end.Y;

            while (true)
            {
                Points.Add(new Position(x0, y0));

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = err * 2;
                if (e2 > -dx)
                {
                    err = -dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
    }
}
