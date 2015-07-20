using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Corridor
    {
        public List<Segment> Segments { get; set; }

        public Corridor()
        {
            Segments = new List<Segment>();
        }

        public Corridor(List<Segment> segments)
        {
            Segments = segments;
        }

        public static Corridor Build(Position start, Position end)
        {
            // Note that this algorithm favors going horizontally and then vertically.
            // TODO: Should I just use the PathFinder here? 

            Corridor corridor = new Corridor();
            // TODO: Use a utility class to reorder these positions as well as the positions in construction of a Rectangle
            // Reorder the start and end points of the corridor so that the start is always to left of the end point
            if (start.X > end.X)
            {
                Swap(ref start, ref end);
            }

            //// Start and end positions are in the same horizontal line
            //if (start.Y == end.Y)
            //{
            //    Position segmentStart = new Position(start.X + 1, start.Y);
            //    Position segmentEnd = new Position(end.X - 1, end.Y);
            //    corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            //}

            //// Start and end positions are in the same vertical line
            //if (start.X == end.X)
            //{
            //    Position segmentStart = new Position(start.X, start.Y + 1);
            //    Position segmentEnd = new Position(end.X, end.Y - 1);
            //    corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            //}

            //// L shape corridors
            //if (start.X != end.X && start.Y != end.Y)
            //{
            //    // First horizontal segment
            //    Position segmentStart = new Position(start.X + 1, start.Y);
            //    Position segmentEnd = new Position(end.X - 1, start.Y);
            //    corridor.Segments.Add(new Segment(segmentStart, segmentEnd));

            //    // Second vertical segment
            //    if (end.Y > start.Y)
            //    {
            //        segmentStart = new Position(end.X - 1, start.Y + 1);
            //        segmentEnd = new Position(end.X - 1, end.Y);
            //    }
            //    else
            //    {
            //        segmentStart = new Position(end.X - 1, start.Y - 1);
            //        segmentEnd = new Position(end.X - 1, end.Y);
            //    }
            //    corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            //}
            Position segmentStart = null;
            Position segmentEnd = null;

            // Move right first (if needed)
            if (start.X != end.X)
            {
                segmentStart = new Position(start.X + 1, start.Y);
                segmentEnd = new Position(end.X - 1, start.Y);
                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }

            // Then move up
            if (start.Y < end.Y)
            {
                if (segmentStart == null) // We need a vertical corridor
                {
                    segmentStart = new Position(start.X, start.Y + 1);
                    segmentEnd = new Position(start.X, end.Y - 1);
                }
                else
                {
                    segmentStart = new Position(segmentEnd.X, segmentStart.Y + 1);
                    segmentEnd = new Position(segmentEnd.X, end.Y);
                }
                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }
            // or down
            if (start.Y > end.Y)
            {
                if (segmentStart == null) // We need a vertical corridor
                {
                    segmentStart = new Position(start.X, start.Y - 1);
                    segmentEnd = new Position(start.X, end.Y + 1);
                }
                else
                {
                    segmentStart = new Position(segmentEnd.X, segmentStart.Y - 1);
                    segmentEnd = new Position(segmentEnd.X, end.Y);
                }
                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }

            return corridor;
        }

        private static void Swap(ref Position firstPosition, ref Position secondPosition)
        {
            Position tempPosition = firstPosition;
            firstPosition = secondPosition;
            secondPosition = tempPosition;
        }
    }
}
