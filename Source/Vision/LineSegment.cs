using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Utilities;

namespace Turnable.Vision
{
    public class LineSegment
    {
        // http://trystans.blogspot.com/2011/09/roguelike-tutorial-08-vision-line-of.html
        public List<Position> Points { get; private set; }

        public LineSegment(Position start, Position end)
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
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public Position Start
        {
            get
            {
                return Points.First<Position>();
            }
        }

        public Position End
        {
            get
            {
                return Points.Last<Position>();
            }
        }

        public Position GetRandomPoint()
        {
            Random random = new Random();

            return Points[random.Next(0, Points.Count)];
        }

        public Position GetMidpoint()
        {
            int midpointIndex = (int)Math.Ceiling((double)Points.Count / 2.0d) - 1;

            return Points[midpointIndex];
        }

        public bool Intersects(Rectangle rectangle, bool excludeStartAndEndPoints = false)
        {
            foreach (Position point in Points)
            {
                if (rectangle.Contains(point))
                {
                    if (excludeStartAndEndPoints && point == Start || excludeStartAndEndPoints && point == End)
                    {
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsParallelTo(LineSegment other)
        {
            return (IsVertical() && other.IsVertical() || IsHorizontal() && other.IsHorizontal());
        }

        public int DistanceBetween(LineSegment other)
        {
            if (!IsParallelTo(other))
            {
                throw new ArgumentException();
            }

            if (IsVertical())
            {
                return Math.Abs(other.Start.X - Start.X);
            }

            return Math.Abs(other.Start.Y - Start.Y);
        }

        public bool IsTouching(LineSegment other)
        {
            if (!IsParallelTo(other))
            {
                return false;
            }

            if (IsVertical())
            {
                if (Math.Abs(Start.X - other.Start.X) == 1) 
                {
                    int minY, maxY, minOtherY, maxOtherY;
                    minY = Math.Min(Start.Y, End.Y);
                    maxY = Math.Max(Start.Y, End.Y);
                    minOtherY = Math.Min(other.Start.Y, other.End.Y);
                    maxOtherY = Math.Max(other.Start.Y, other.End.Y);

                    if (minY >= minOtherY && minY <= maxOtherY || maxY >= minOtherY && maxY <= maxOtherY
                        || minOtherY >= minY && minOtherY <= maxY || maxOtherY >= minY && maxOtherY <= maxY)
                    {
                        return true;
                    }
                }
            }

            if (IsHorizontal())
            {
                if (Math.Abs(Start.Y - other.Start.Y) == 1)
                {
                    int minX, maxX, minOtherX, maxOtherX;
                    minX = Math.Min(Start.X, End.X);
                    maxX = Math.Max(Start.X, End.X);
                    minOtherX = Math.Min(other.Start.X, other.End.X);
                    maxOtherX = Math.Max(other.Start.X, other.End.X);

                    if (minX >= minOtherX && minX <= maxOtherX || maxX >= minOtherX && maxX <= maxOtherX
                        || minOtherX >= minX && minOtherX <= maxX || maxOtherX >= minX && maxOtherX <= maxX)
                    {
                        return true;
                    } 
                }
            }

            return false;
        }

        public bool IsVertical()
        {
            return (Start.X == End.X);
        }

        public bool IsHorizontal()
        {
            return (Start.Y == End.Y);
        }

        public bool Equals(LineSegment other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Start == other.Start && this.End == other.End || this.Start == other.End && this.End == other.Start);
        }

        public override bool Equals(Object other)
        {
            LineSegment otherLineSegment = other as LineSegment;

            if (otherLineSegment == null)
            {
                return false;
            }
            else
            {
                return Equals(otherLineSegment);
            }
        }

        public static bool operator ==(LineSegment lineSegment1, LineSegment lineSegment2)
        {
            if ((object)lineSegment1 == null || ((object)lineSegment2) == null)
            {
                return Object.Equals(lineSegment1, lineSegment2);
            }

            return lineSegment1.Equals(lineSegment2);
        }

        public static bool operator !=(LineSegment lineSegment1, LineSegment lineSegment2)
        {
            if ((object)lineSegment1 == null || ((object)lineSegment2) == null)
            {
                return !Object.Equals(lineSegment1, lineSegment2);
            }

            return !(lineSegment1.Equals(lineSegment2));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                hash = hash * 16777619 ^ Start.GetHashCode();
                hash = hash * 16777619 ^ End.GetHashCode();

                return hash;
            }
        }

    }
}
